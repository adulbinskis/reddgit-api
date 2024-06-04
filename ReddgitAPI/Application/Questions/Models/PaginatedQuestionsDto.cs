
namespace ReddgitAPI.Application.Questions.Models
{
    public class PaginatedQuestionsDto
    {
        public int TotalPages { get; set; }
        public List<QuestionDetailDto> Questions { get; internal set; }
    }
}
