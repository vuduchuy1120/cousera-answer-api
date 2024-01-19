namespace Cousera.DTO
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public string? QuestionDescription { get; set; }
        public List<AnswerResponse> Answers { get; set; }
    }
}
