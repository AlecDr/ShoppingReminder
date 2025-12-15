using ShoppingReminder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class PurchaseHistory : BaseEntity
    {
        // For which user and group?
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        // Item Info
        public string ItemName { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? Notes { get; set; }

        // Frequency
        public int PurchaseCount { get; set; }  // How many times bought
        public DateTime LastPurchasedAt { get; set; }
        public DateTime? FirstPurchasedAt { get; set; }

        // Average frequency (for smart suggestions)
        public int? AverageDaysBetweenPurchases { get; set; }
    }
}
