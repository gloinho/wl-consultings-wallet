using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WlConsultings.BankChallenge.Domain.Entities;

namespace WlConsultings.BankChallenge.Infra.Configuration.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(u => u.Wallet)
                .WithOne()
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.Id)
                .IsRequired();

            builder.HasIndex(t => t.Email)
                .IsUnique();

            builder.Property(t => t.Role)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .IsRequired();
        }
    }
}
