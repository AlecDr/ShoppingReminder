using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Common
{
    public interface IsAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime UpdatedBy { get; set; }
    }
}
