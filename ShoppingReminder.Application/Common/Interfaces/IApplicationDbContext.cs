using Microsoft.EntityFrameworkCore;
using ShoppingReminder.Domain.Entities;
using System.Collections.Generic;

namespace ShoppingReminder.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Group> Groups { get; }
    DbSet<GroupMember> GroupMembers { get; }
    DbSet<Invitation> Invitations { get; }
    DbSet<ShoppingList> ShoppingLists { get; }
    DbSet<ShoppingItem> ShoppingItems { get; }
    DbSet<PurchaseHistory> PurchaseHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}