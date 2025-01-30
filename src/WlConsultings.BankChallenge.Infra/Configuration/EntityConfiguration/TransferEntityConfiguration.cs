using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WlConsultings.BankChallenge.Domain.Entities;

public class TransferEntityConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).IsRequired();

        builder.HasOne(t => t.SenderWallet)
            .WithMany(w => w.TransfersSent)
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ReceiverWallet)
            .WithMany(w => w.TransfersReceived)
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(12, 2);

        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();

        // Seed Transfers
        builder.HasData(
            new Transfer
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                SenderWalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                ReceiverWalletId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Amount = 150m,
                CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            },
            new Transfer
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                SenderWalletId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                ReceiverWalletId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Amount = 300m,
                CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}