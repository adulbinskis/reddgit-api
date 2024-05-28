namespace ReddgitAPI.ORM.Entities
{
    public class Answer : BaseEntity
    {
        public string Content { get; set; }

        public string QuestionId { get; set; }

        public string UserId { get; set; }

        public Question Question { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
