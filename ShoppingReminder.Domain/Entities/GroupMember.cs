using ShoppingReminder.Domain.Common;
using ShoppingReminder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class GroupMember : BaseEntity
    {
        // Foreign Keys
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Role and Permissions
        public GroupRole Role { get; set; } = GroupRole.Member;

        // Metadata
        public DateTime JoinedAt { get; set; }
        public Guid? InvitedBy { get; set; }  // Who invited this user
        public User? InvitedByUser { get; set; }

        // Last Activity (for "active members" feature)
        public DateTime? LastActivityAt { get; set; }
    }
}
