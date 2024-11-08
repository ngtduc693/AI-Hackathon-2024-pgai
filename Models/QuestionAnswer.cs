using Npgsql;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace InsuranceBot.Models
{
    public class QuestionAnswer
    {
        [Key]
        public string Question { get; set; }
        public string Answer { get; set; }
        public float[] Embedding { get; set; }
    }
}
