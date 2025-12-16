using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Common
{
    public interface IsAuditable
    {
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
