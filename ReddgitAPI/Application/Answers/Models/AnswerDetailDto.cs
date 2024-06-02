namespace ReddgitAPI.Application.Answers.Models
{
    public class AnswerDetailDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
