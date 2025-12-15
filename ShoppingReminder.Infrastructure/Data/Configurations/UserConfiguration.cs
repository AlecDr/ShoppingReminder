using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table name
        builder.ToTable("Users");

        // Primary key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Language)
            .HasMaxLength(10)
            .HasDefaultValue("en");

        builder.Property(u => u.NotificationPreferences)
            .HasColumnType("jsonb"); // PostgreSQL JSON type

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.IsDeleted);

        // Relationships
        builder.HasMany(u => u.GroupMemberships)
            .WithOne(gm => gm.User)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.SentInvitations)
            .WithOne(i => i.InvitedByUser)
            .HasForeignKey(i => i.InvitedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.CreatedLists)
            .WithOne(sl => sl.CreatedByUser)
            .HasForeignKey(sl => sl.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.CreatedItems)
            .WithOne(si => si.AddedByUser)
            .HasForeignKey(si => si.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}