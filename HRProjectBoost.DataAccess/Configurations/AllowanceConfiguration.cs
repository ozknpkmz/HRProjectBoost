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
    public class AllowanceConfiguration : IEntityTypeConfiguration<Allowance>

    {
        public void Configure(EntityTypeBuilder<Allowance> builder)
        {
            builder.HasKey(x=> x.AllowanceId);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Allowances).HasForeignKey(x=> x.AppUserId);
        }
    }
}
