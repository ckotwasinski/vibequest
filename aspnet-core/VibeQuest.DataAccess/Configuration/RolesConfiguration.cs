using VibeQuest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace VibeQuest.DataAccess.Configuration
{
    public class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.Code);
            
            builder.HasData(new Roles
            {
                Id = new Guid("76c80161-67ee-4f05-9fc3-f376a1f06844"),
                Code = "sadmin",
                Name = "Admin",
                CreatedDate = new DateTime(2021, 4, 9),
            }, new Roles
            {
                Id = new Guid("f254b3a8-7374-4f3e-936b-b511bb212786"),
                Code = "AppUser",
                Name = "App User",
                CreatedDate = new DateTime(2021, 4, 9),
            }
            );
        }
    }
}
