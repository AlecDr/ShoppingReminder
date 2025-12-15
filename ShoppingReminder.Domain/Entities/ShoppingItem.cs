using ShoppingReminder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class ShoppingItem : BaseEntity
    {
        // Which list does this belong to?
        public Guid ListId { get; set; }
        public ShoppingList List { get; set; } = null!;

        // Item Details
        public string Name { get; set; } = string.Empty;  // e.g., "Milk"
        public int Quantity { get; set; } = 1;
        public string? Notes { get; set; }  // e.g., "organic", "the blue bottle"
        public string? Category { get; set; }  // e.g., "Dairy", "Produce"

        // Priority
        public bool IsUrgent { get; set; }

        // Purchase Status
        public bool IsPurchased { get; set; }
        public DateTime? PurchasedAt { get; set; }
        public Guid? PurchasedBy { get; set; }
        public User? PurchasedByUser { get; set; }

        // For partial purchases
        public int? PurchasedQuantity { get; set; }  // If bought 2 out of 5

        // Who added this item?
        public Guid AddedByUserId { get; set; }
        public User AddedByUser { get; set; } = null!;

        // Sync Support (for offline-first)
        public int Version { get; set; } = 1;  // Incremented on each update
        public bool IsSynced { get; set; } = true;

        // Display Order (for drag-and-drop reordering)
        public int DisplayOrder { get; set; }
    }
}
