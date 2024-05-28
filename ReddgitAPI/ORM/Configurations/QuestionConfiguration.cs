using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.ORM.Configurations
{
    public class QuestionConfiguration : BaseEntityConfiguration<Question>
    {
        public override void Configure(EntityTypeBuilder<Question> builder)
        {
            base.Configure(builder);

            base.Configure(builder);

            builder.Property(q => q.Title).IsRequired();
            builder.Property(q => q.Content).IsRequired();

            builder.HasOne(q => q.ApplicationUser)
                   .WithMany(u => u.Questions)
                   .HasForeignKey(q => q.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Answers)
                   .WithOne(a => a.Question)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
