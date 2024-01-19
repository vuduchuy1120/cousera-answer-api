namespace Cousera.DTO
{
    public class QuestionRequest
    {
        public string? QuestionDescription { get; set; }
        public List<AnswerRequest> Answers { get; set; }
    }
}
