namespace InsuranceBot.Models
{
    public class QuestionAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public float[] Embedding { get; set; }
    }
}
