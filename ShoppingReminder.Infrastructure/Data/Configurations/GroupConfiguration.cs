using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("Groups");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(g => g.Description)
            .HasMaxLength(1000);

        builder.Property(g => g.InviteCode)
            .HasMaxLength(10);

        builder.Property(g => g.AllowMembersToInvite)
            .HasDefaultValue(true);

        builder.Property(g => g.MaxMembers)
            .HasDefaultValue(10);

        // Indexes
        builder.HasIndex(g => g.InviteCode)
            .IsUnique()
            .HasFilter("\"InviteCode\" IS NOT NULL"); // Partial index

        builder.HasIndex(g => g.OwnerId);
        builder.HasIndex(g => g.IsDeleted);

        // Relationships
        builder.HasOne(g => g.Owner)
            .WithMany()
            .HasForeignKey(g => g.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(g => g.Members)
            .WithOne(gm => gm.Group)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Lists)
            .WithOne(sl => sl.Group)
            .HasForeignKey(sl => sl.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Invitations)
            .WithOne(i => i.Group)
            .HasForeignKey(i => i.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}