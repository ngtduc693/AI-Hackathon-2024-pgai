using InsuranceBot.Data;
using InsuranceBot.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace InsuranceBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

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
