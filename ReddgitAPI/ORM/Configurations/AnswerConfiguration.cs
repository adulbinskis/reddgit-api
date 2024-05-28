using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReddgitAPI.ORM.Entities;

namespace ReddgitAPI.ORM.Configurations
{
    public class AnswerConfiguration : BaseEntityConfiguration<Answer>
    {
        public override void Configure(EntityTypeBuilder<Answer> builder)
        {
            base.Configure(builder);

            builder.Property(a => a.Content).IsRequired();

            builder.HasOne(a => a.ApplicationUser)
                   .WithMany(u => u.Answers)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Question)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
