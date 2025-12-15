using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitations");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvitedEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.Token)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Message)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(i => i.Token)
            .IsUnique();

        builder.HasIndex(i => i.InvitedEmail);
        builder.HasIndex(i => i.GroupId);
        builder.HasIndex(i => i.Status);
        builder.HasIndex(i => i.ExpiresAt);

        // Relationships
        builder.HasOne(i => i.Group)
            .WithMany(g => g.Invitations)
            .HasForeignKey(i => i.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.InvitedByUser)
            .WithMany(u => u.SentInvitations)
            .HasForeignKey(i => i.InvitedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.InvitedUser)
            .WithMany()
            .HasForeignKey(i => i.InvitedUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}