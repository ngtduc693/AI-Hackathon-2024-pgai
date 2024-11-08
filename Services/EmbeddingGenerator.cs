using InsuranceBot.Data;
using InsuranceBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InsuranceBot.Services
{
    public class EmbeddingGenerator
    {
        private readonly EmbeddingService _embeddingService;
        private readonly ApplicationDbContext _dbContext;

        public EmbeddingGenerator(EmbeddingService embeddingService, ApplicationDbContext dbContext)
        {
            _embeddingService = embeddingService;
            _dbContext = dbContext;
        }

        public async Task GenerateEmbeddingsForQuestionsAnswersAsync(IEnumerable<QuestionAnswer> questionAnswers)
        {
            foreach (var qa in questionAnswers)
            {
                var questionEmbedding = await _embeddingService.GetEmbeddingsAsync(qa.Question);
                var answerEmbedding = await _embeddingService.GetEmbeddingsAsync(qa.Answer);


                qa.Embedding = questionEmbedding;
                await SaveEmbeddingsToDatabase(qa);
            }
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
