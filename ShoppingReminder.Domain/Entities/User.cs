using ShoppingReminder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public  class User : BaseEntity
    {
        // basic info
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        // email verification
        public bool IsEmailVerified { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }

        // password reset
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        // settings
        public string? NotificationPreferences { get; set; } // JSON: { "urgent": true, "digest": true }
        public string? Language { get; set; } = "pt-br"; // e.g., "en", "es"


        // Navigation properties
        // Navigation Properties (EF Core relationships)
        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();
        public ICollection<ShoppingList> CreatedLists { get; set; } = new List<ShoppingList>();
        public ICollection<ShoppingItem> CreatedItems { get; set; } = new List<ShoppingItem>();
    }
}
