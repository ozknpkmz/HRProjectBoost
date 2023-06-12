using AutoMapper;
using HRProjectBoost.Business.Mappings.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Business.DependencyResolvers
{
    public static class DependencyExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
           
            var configuration = new MapperConfiguration(opt =>
            {
                opt.AddProfile(new ManagerProfile());
                opt.AddProfile(new PersonnelProfile());
                opt.AddProfile(new AuthenticationProfile());
                opt.AddProfile(new AllowanceProfile());
                opt.AddProfile(new AdminProfile());
                opt.AddProfile(new AdvanceProfile());

            });


            var mapper = configuration.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
