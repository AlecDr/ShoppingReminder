using ShoppingReminder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Entities
{
    public class ShoppingList : BaseEntity
    {
        // Basic Info
        public string Name { get; set; } = string.Empty;  // e.g., "Groceries", "Pharmacy"
        public string? Description { get; set; }

        // Belongs to which group?
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        // Who created it?
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; } = null!;

        // List Type
        public bool IsTemplate { get; set; }  // If true, it's a template list
        public Guid? TemplateSourceId { get; set; }  // If created from template, reference it

        // Status
        public bool IsArchived { get; set; }  // Completed lists can be archived
        public DateTime? ArchivedAt { get; set; }

        // Settings
        public string? Color { get; set; }  // For UI - hex color code
        public string? Icon { get; set; }    // For UI - emoji or icon name

        // Navigation Properties
        public ICollection<ShoppingItem> Items { get; set; } = new List<ShoppingItem>();
    }
}
