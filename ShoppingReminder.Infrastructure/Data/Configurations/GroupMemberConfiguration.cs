using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMembers");

        builder.HasKey(gm => gm.Id);

        // Composite unique index - a user can only be in a group once
        builder.HasIndex(gm => new { gm.GroupId, gm.UserId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false"); // Only enforce when not deleted

        builder.HasIndex(gm => gm.GroupId);
        builder.HasIndex(gm => gm.UserId);
        builder.HasIndex(gm => gm.IsDeleted);

        // Relationships
        builder.HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.User)
            .WithMany(u => u.GroupMemberships)
            .HasForeignKey(gm => gm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.InvitedByUser)
            .WithMany()
            .HasForeignKey(gm => gm.InvitedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}