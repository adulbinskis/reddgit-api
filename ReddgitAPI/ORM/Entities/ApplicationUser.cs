using Microsoft.AspNetCore.Identity;
using ReddgitAPI.Application.Identity.Roles;

namespace ReddgitAPI.ORM.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
