using System;

namespace VibeQuest.Model
{
    public interface ICreationAuditedEntity
    {
        DateTime CreatedDate { get; set; }
        Guid? CreatedBy { get; set; }
    }

    public interface IAuditedEntity : ICreationAuditedEntity
    {
        DateTime? UpdatedDate { get; set; }

        Guid? UpdatedBy { get; set; }
    }

    public interface IFullAuditedEntity : IAuditedEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedDate { get; set; }
        Guid? DeletedBy { get; set; }
    }

    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }
    }

    public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedEntity
    {
        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }
    }

    public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedEntity
    {
        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }

    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IFullAuditedEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
