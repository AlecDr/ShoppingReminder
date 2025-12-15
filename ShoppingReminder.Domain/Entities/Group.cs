using ShoppingReminder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class Group : BaseEntity
    {
        // Basic Info
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Owner (the user who created the group)
        public Guid OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // Invite Code (optional - for easy joining)
        public string? InviteCode { get; set; }  // e.g., "ABC123"
        public DateTime? InviteCodeExpires { get; set; }

        // Settings
        public bool AllowMembersToInvite { get; set; } = true;
        public int MaxMembers { get; set; } = 10;  // For SaaS tiers

        // Navigation Properties
        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<ShoppingList> Lists { get; set; } = new List<ShoppingList>();
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
