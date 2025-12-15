using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class PurchaseHistoryConfiguration : IEntityTypeConfiguration<PurchaseHistory>
{
    public void Configure(EntityTypeBuilder<PurchaseHistory> builder)
    {
        builder.ToTable("PurchaseHistories");

        builder.HasKey(ph => ph.Id);

        builder.Property(ph => ph.ItemName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ph => ph.Category)
            .HasMaxLength(100);

        builder.Property(ph => ph.Notes)
            .HasMaxLength(500);

        // Indexes - optimized for frequent queries
        builder.HasIndex(ph => new { ph.UserId, ph.GroupId, ph.ItemName });
        builder.HasIndex(ph => new { ph.GroupId, ph.LastPurchasedAt });
        builder.HasIndex(ph => ph.IsDeleted);

        // Relationships
        builder.HasOne(ph => ph.User)
            .WithMany()
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ph => ph.Group)
            .WithMany()
            .HasForeignKey(ph => ph.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}