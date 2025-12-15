using ShoppingReminder.Domain.Common;
using ShoppingReminder.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class Invitation : BaseEntity
    {
        // What group is this for?
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        // Who's being invited?
        public string InvitedEmail { get; set; } = string.Empty;
        public Guid? InvitedUserId { get; set; }  // If user already exists
        public User? InvitedUser { get; set; }

        // Who sent the invite?
        public Guid InvitedBy { get; set; }
        public User InvitedByUser { get; set; } = null!;

        // Invitation Details
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public string Token { get; set; } = string.Empty;  // Unique token for the link
        public DateTime ExpiresAt { get; set; }

        // Optional Message
        public string? Message { get; set; }

        // Response Tracking
        public DateTime? RespondedAt { get; set; }
    }
}
