using System;

namespace VibeQuest.Dto
{
    public abstract class EntityDto<TKey>
    {
        public TKey Id { get; set; }
    }

    public abstract class CreationAuditedEntityDto<TKey> : EntityDto<TKey>
    {
        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }
    }

    public abstract class AuditedEntityDto<TKey> : CreationAuditedEntityDto<TKey>
    {
        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }
    }

    public abstract class FullAuditedEntityDto<TKey> : AuditedEntityDto<TKey>
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
