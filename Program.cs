using InsuranceBot.Data;
using InsuranceBot.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace InsuranceBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var configuration = host.Services.GetRequiredService<IConfiguration>();

            var ollamaApiUrl = configuration.GetValue<string>("Ollama:ApiUrl");
            var isEmbedEnabled = configuration.GetValue<bool>("Embeed:Enabled");
            Console.WriteLine($"Ollama API URL: {ollamaApiUrl}");

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    dbContext.Database.Migrate();

                    if (!dbContext.QuestionAnswers.Any())
                    {
                        var seeder = services.GetRequiredService<DatabaseSeeder>();
                        seeder.SeedData();
                    }

                    if (isEmbedEnabled)
                    {
                        var embeddingGenerator = services.GetRequiredService<EmbeddingGenerator>();
                        var questionAnswers = dbContext.QuestionAnswers.ToList().Where(t => t.Embedding == null);

                        embeddingGenerator.GenerateEmbeddingsForQuestionsAnswersAsync(questionAnswers).Wait();
                    }

                    var cache = MemoryCache.Default;
                    var cachedData = dbContext.QuestionAnswers.ToList();
                    cache.Set("questionAnswersCacheKey", cachedData, new CacheItemPolicy
                    {
                        AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred seeding the DB: " + ex.Message);
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
       
    }
}
