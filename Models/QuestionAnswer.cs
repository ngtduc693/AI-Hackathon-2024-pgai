using System.ComponentModel.DataAnnotations;

namespace InsuranceBot.Models
{
    public class QuestionAnswer
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public float[] Embedding { get; set; }
    }
}
