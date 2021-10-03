using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using VibeQuest.Model;
using VibeQuest.Utility.Helpers;

namespace VibeQuest.DataAccess.Configuration
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => new { x.IsActive, x.IsDeleted });
            var hashPassword = PBKDF2.HashPassword("sadmin@123");
            builder.HasData(new Users
            {
                Id = new Guid("0d50f040-9e48-45a7-8dd7-4b26322384db"),
                FullName = "SAdmin",
                Email = "sadmin@gmail.com",
                Password= hashPassword.Item2,
                PasswordSalt = hashPassword.Item1,
                IsActive = true,
                CreatedDate = new DateTime(2021, 4, 9)
            });
        }
    }
}
