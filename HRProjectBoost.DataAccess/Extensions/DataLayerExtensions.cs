using HRProjectBoost.DataAccess.Context;
using HRProjectBoost.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DataAccess.Extensions
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtensions(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<HRProjectContext>(opt => opt.UseSqlServer(config.GetConnectionString("OzkanDB")));


            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<HRProjectContext>();

            return services;

        }
    }
}
