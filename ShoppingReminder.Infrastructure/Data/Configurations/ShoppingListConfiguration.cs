using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingList>
{
    public void Configure(EntityTypeBuilder<ShoppingList> builder)
    {
        builder.ToTable("ShoppingLists");

        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(sl => sl.Description)
            .HasMaxLength(1000);

        builder.Property(sl => sl.Color)
            .HasMaxLength(7); // #RRGGBB

        builder.Property(sl => sl.Icon)
            .HasMaxLength(50);

        // Indexes
        builder.HasIndex(sl => sl.GroupId);
        builder.HasIndex(sl => sl.CreatedByUserId);
        builder.HasIndex(sl => sl.IsTemplate);
        builder.HasIndex(sl => sl.IsArchived);
        builder.HasIndex(sl => sl.IsDeleted);

        // Relationships
        builder.HasOne(sl => sl.Group)
            .WithMany(g => g.Lists)
            .HasForeignKey(sl => sl.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sl => sl.CreatedByUser)
            .WithMany(u => u.CreatedLists)
            .HasForeignKey(sl => sl.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sl => sl.Items)
            .WithOne(si => si.List)
            .HasForeignKey(si => si.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}