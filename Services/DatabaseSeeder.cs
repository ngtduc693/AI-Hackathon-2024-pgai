using InsuranceBot.Data;
using InsuranceBot.Models;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System;
using System.Linq;
using CsvHelper;

namespace InsuranceBot.Services
{
    public class DatabaseSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedData()
        {
            try
            {
                if (_dbContext.QuestionAnswers.Any())
                {
                    Console.WriteLine("Database already seeded.");
                    return;
                }

                var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "questions_answers.csv");

                using var reader = new StreamReader(csvFilePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var questionAnswers = csv.GetRecords<QuestionAnswer>().ToList();

                _dbContext.QuestionAnswers.AddRange(questionAnswers);
                _dbContext.SaveChanges();

                Console.WriteLine("Database seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred seeding the DB: " + ex.Message);
            }
        }
    }
}
