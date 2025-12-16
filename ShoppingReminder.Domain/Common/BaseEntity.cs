using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Common
{
    public abstract class BaseEntity : IsAuditable, IsSoftDeletable
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }


        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
