namespace ReddgitAPI.ORM.Entities
{
    public class Question : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
