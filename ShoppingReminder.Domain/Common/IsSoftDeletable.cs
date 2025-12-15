using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingReminder.Domain.Common
{
    internal interface IsSoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        Guid? DeletedBy { get; set; }
    }
}
