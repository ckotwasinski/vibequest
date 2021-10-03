using VibeQuest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using VibeQuest.DataAccess.Configuration;
using VibeQuest.Utility.JWT;
using VibeQuest.Utility.Extensions;
using System.Threading.Tasks;
using System.Threading;

namespace VibeQuest.DataAccess
{
    public class VibeQuestDbContext : DbContext
    {
        public readonly ICurrentUser _currentUser;

        public DbSet<AuditLogs> AuditLogs { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<EmailHistory> EmailHistory { get; set; }
        public DbSet<EmailTemplates> EmailTemplates { get; set; }
        public DbSet<CommonLookup> CommonLookups { get; set; }
        public DbSet<PermissionGrants> PermissionGrants { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<UserCategories> UserCategories { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<EventCategories> EventCategories { get; set; }
        public DbSet<EventMedia> EventMedia { get; set; }
        public DbSet<EventAttendees> EventAttendees { get; set; }
        public DbSet<UserFriends> UserFriends { get; set; }
        public DbSet<UserInvites> UserInvites { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        public VibeQuestDbContext(DbContextOptions<VibeQuestDbContext> context,
            ICurrentUser currentUser) : base(context)
        {
            _currentUser = currentUser;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuditLogsConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new EmailHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new EmailTemplatesConfiguration());
            modelBuilder.ApplyConfiguration(new CommonLookupConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionGrantsConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new UserCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new EventsConfiguration());
            modelBuilder.ApplyConfiguration(new EventCategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new EventMediaConfiguration());
            modelBuilder.ApplyConfiguration(new EventAttendeesConfiguration());
            modelBuilder.ApplyConfiguration(new UserFriendsConfiguration());
            modelBuilder.ApplyConfiguration(new UserInvitesConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationsConfiguration());
        }

        public override int SaveChanges()
        {
            UpdateAuditableEntity();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableEntity();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public Guid? GetCurrentUserId()
        {
            return _currentUser.Id.IsNullOrWhiteSpace() ? null : new Guid(_currentUser.Id);
        }

        private void UpdateAuditableEntity()
        {
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is ICreationAuditedEntity entity)
                        {
                            entity.CreatedBy = GetCurrentUserId();
                            entity.CreatedDate = DateTime.UtcNow;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IFullAuditedEntity auditedEntity)
                        {
                            auditedEntity.UpdatedBy = GetCurrentUserId();
                            auditedEntity.UpdatedDate = DateTime.UtcNow;

                            if (auditedEntity.IsDeleted)
                            {
                                auditedEntity.DeletedBy = GetCurrentUserId();
                                auditedEntity.DeletedDate = DateTime.UtcNow;
                            }
                        }
                        break;
                }
            }
        }
    }
}
