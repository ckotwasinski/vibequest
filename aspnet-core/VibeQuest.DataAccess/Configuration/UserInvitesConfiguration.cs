using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeQuest.Model;

namespace VibeQuest.DataAccess.Configuration
{
    public class UserInvitesConfiguration : IEntityTypeConfiguration<UserInvites>
    {
        public void Configure(EntityTypeBuilder<UserInvites> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
