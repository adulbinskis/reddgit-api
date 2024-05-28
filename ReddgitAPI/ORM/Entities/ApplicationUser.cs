using Microsoft.AspNetCore.Identity;

namespace ReddgitAPI.ORM.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
