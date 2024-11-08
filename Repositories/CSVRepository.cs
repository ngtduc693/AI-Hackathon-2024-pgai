using CsvHelper.Configuration;
using CsvHelper;
using InsuranceBot.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace InsuranceBot.Repositories
{
    public class CSVRepository
    {
        private readonly string _filePath;

        public CSVRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<QuestionAnswer> GetQuestionAnswers()
        {
            var questionAnswers = new List<QuestionAnswer>();


            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, config))
            {
                questionAnswers = csv.GetRecords<QuestionAnswer>().ToList();
            }

            return questionAnswers;
        }
    }
}
