using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class ShoppingItemConfiguration : IEntityTypeConfiguration<ShoppingItem>
{
    public void Configure(EntityTypeBuilder<ShoppingItem> builder)
    {
        builder.ToTable("ShoppingItems");

        builder.HasKey(si => si.Id);

        builder.Property(si => si.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(si => si.Notes)
            .HasMaxLength(500);

        builder.Property(si => si.Category)
            .HasMaxLength(100);

        builder.Property(si => si.Quantity)
            .HasDefaultValue(1);

        builder.Property(si => si.Version)
            .HasDefaultValue(1)
            .IsConcurrencyToken(); // For optimistic concurrency

        builder.Property(si => si.IsSynced)
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(si => si.ListId);
        builder.HasIndex(si => si.AddedByUserId);
        builder.HasIndex(si => si.IsPurchased);
        builder.HasIndex(si => si.IsUrgent);
        builder.HasIndex(si => si.IsDeleted);
        builder.HasIndex(si => new { si.ListId, si.DisplayOrder });

        // Relationships
        builder.HasOne(si => si.List)
            .WithMany(sl => sl.Items)
            .HasForeignKey(si => si.ListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(si => si.AddedByUser)
            .WithMany(u => u.CreatedItems)
            .HasForeignKey(si => si.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(si => si.PurchasedByUser)
            .WithMany()
            .HasForeignKey(si => si.PurchasedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}