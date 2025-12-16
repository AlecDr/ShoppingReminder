using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShoppingReminder.Domain.Common;
using ShoppingReminder.Domain.Entities;

namespace ShoppingReminder.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets - these represent tables in the database
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingItem> ShoppingItems { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the Configurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply global query filter for soft deletes
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IsSoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                // Create the filter expression: e => !e.IsDeleted
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(IsSoftDeletable.IsDeleted));
                var filter = Expression.Lambda(
                    Expression.Equal(property, Expression.Constant(false)),
                    parameter
                );

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Get all entities that are being added, modified, or deleted
        var entries = ChangeTracker.Entries<BaseEntity>();
        var now = DateTime.UtcNow;

        // TODO: Get current user ID from a service (we'll create this later)
        Guid? currentUserId = null; // For now, we'll set this to null

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUserId;
                    break;

                case EntityState.Deleted:
                    // Convert hard delete to soft delete
                    if (entry.Entity is IsSoftDeletable)
                    {
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = now;
                        entry.Entity.DeletedBy = currentUserId;
                    }
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}