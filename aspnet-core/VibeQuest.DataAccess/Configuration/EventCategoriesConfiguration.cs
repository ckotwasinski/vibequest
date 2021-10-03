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
    public class EventCategoriesConfiguration : IEntityTypeConfiguration<EventCategories>
    {
        public void Configure(EntityTypeBuilder<EventCategories> builder)
        {
            builder.HasIndex(x => x.IsDeleted);
        }
    }
}
