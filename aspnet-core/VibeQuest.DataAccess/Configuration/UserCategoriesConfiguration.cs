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
    public class UserCategoriesConfiguration : IEntityTypeConfiguration<UserCategories>
    {
        public void Configure(EntityTypeBuilder<UserCategories> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
