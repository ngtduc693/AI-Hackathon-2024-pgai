using InsuranceBot.Data;
using InsuranceBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace InsuranceBot.Services
{
    public class EmbeddingGenerator
    {
        private readonly EmbeddingService _embeddingService;
        private readonly ApplicationDbContext _dbContext;
        private readonly MemoryCache _cache;

        public EmbeddingGenerator(EmbeddingService embeddingService, ApplicationDbContext dbContext)
        {
            _embeddingService = embeddingService;
            _dbContext = dbContext;
            _cache = MemoryCache.Default;
        }

        public async Task GenerateEmbeddingsForQuestionsAnswersAsync(IEnumerable<QuestionAnswer> questionAnswers)
        {
            foreach (var qa in questionAnswers)
            {
                var questionEmbedding = await _embeddingService.GetEmbeddingsAsync(qa.Question);

                qa.Embedding = questionEmbedding;
                await SaveEmbeddingsToDatabase(qa);
            }
        }

        internal async Task<QuestionAnswer> FindMostSimilarQuestionAsync(string userQuery)
        {
            float[] userQueryEmbedding = await _embeddingService.GetEmbeddingsAsync(userQuery);

            var cacheKey = "questionAnswersCacheKey";
            var cachedData = _cache.Get(cacheKey) as List<QuestionAnswer>;

            List<QuestionAnswer> questionAnswers;

            if (cachedData != null)
            {
                questionAnswers = cachedData;
            }
            else
            {
                questionAnswers = await _dbContext.QuestionAnswers.ToListAsync();
            }

            QuestionAnswer mostSimilarQuestion = null;
            float highestSimilarity = -1;

            Parallel.ForEach(questionAnswers, (qa) =>
            {
                float similarity = CalculateCosineSimilarity(userQueryEmbedding, qa.Embedding);

                lock (this)
                {
                    if (similarity > highestSimilarity)
                    {
                        highestSimilarity = similarity;
                        mostSimilarQuestion = qa;
                    }
                }
            });

            return mostSimilarQuestion;
        }

        internal float CalculateCosineSimilarity(float[] vector1, float[] vector2)
        {
            if (vector1.Length != vector2.Length)
                throw new ArgumentException("Vectors must be the same length");

            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitudeA += vector1[i] * vector1[i];
                magnitudeB += vector2[i] * vector2[i];
            }

            return dotProduct / (float)(Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }

        private async Task SaveEmbeddingsToDatabase(QuestionAnswer qa)
        {
            var existingQa = await _dbContext.QuestionAnswers
                                              .FirstOrDefaultAsync(q => q.Question == qa.Question);

            if (existingQa != null)
            {                
                existingQa.Embedding = qa.Embedding;
            }
            else
            {
                _dbContext.QuestionAnswers.Add(qa);
            }

            await _dbContext.SaveChangesAsync();
        }
    }


}
