using HRProjectBoost.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DataAccess.Configurations
{
    public class AdvanceConfiguration : IEntityTypeConfiguration<Advance>
    {
        public void Configure(EntityTypeBuilder<Advance> builder)
        {
            builder.HasKey(x => x.AdvanceId);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Advances).HasForeignKey(x => x.AppUserId);
        }
    }
}
