using VibeQuest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.DataAccess.Configuration
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.RoleId);
            builder.HasData(new UserRoles
            {
                Id = new Guid("d262b103-658b-46ed-855f-908966a03832"),
                UserId = new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                RoleId = new Guid("76c80161-67ee-4f05-9fc3-f376a1f06844"),
                CreatedDate = new DateTime(2021, 4, 9),
            });
        }
    }
}
