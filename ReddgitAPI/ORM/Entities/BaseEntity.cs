using ReddgitAPI.ORM.Entities.Interfaces;

namespace ReddgitAPI.ORM.Entities
{
    public abstract class BaseEntity : IDeletable, IAuditable
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string UpdatedById { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
    }
}
