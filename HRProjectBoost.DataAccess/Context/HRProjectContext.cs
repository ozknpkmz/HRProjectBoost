using HRProjectBoost.DataAccess.Configurations;
using HRProjectBoost.Entities.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRProjectBoost.DataAccess.Context
{
    public class HRProjectContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public HRProjectContext(DbContextOptions<HRProjectContext> options) : base(options)
        {

        }

        public DbSet<Allowance> Allowance { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Advance> Advance { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region SeedData

            builder.Entity<AppRole>().HasData(
                new AppRole() { Id = 1, Name = "Admin", NormalizedName = "ADMIN", },
                new AppRole() { Id = 2, Name = "Manager", NormalizedName = "MANAGER", },
                new AppRole() { Id = 3, Name = "Personnel", NormalizedName = "PERSONNEL", }
                );


            Company seedCompany = new Company()
            {
                CompanyId = 1,
                CompanyName = "TestCompany",
                CompanyTitle = "TC",
                MersisNo = "123456",
                TaxNo = "123456",
                TaxAdministration = "DenemeVergiDairesi",
                CompanyPhoneNumber = "+9050012312312",
                CompanyAddress = "Adress Deneme",
                CompanyEmail = "test.company@test.com",
                EstablishDate = DateTime.Now.AddYears(-2),
                AgreementStartDate = DateTime.Now,
                AgreementEndDate = DateTime.Now.AddYears(2),
                CompanyStatus = Entities.Enums.CompanyStatus.Active,
            };


            AppUser seedAdmin = new AppUser()
            {
                Id = 1,
                UserName = "Admin",
                NormalizedUserName = "Admin".ToUpper(),
                Name = "Admin",
                SecondName = "Admin",
                LastName = "Admin",
                Password = "123456aA-",
                SecondLastName = "Admin",
                PhoneNumber = "12345678901",
                BirthDate = DateTime.Now,
                BirthCity = "Admin",
                IdentityNumber = "12345678998",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CompanyInfo = "Admin",
                Job = "Admin",
                Department = Entities.Enums.Department.Engineer,
                Address = "İstanbul/Maltepe",
                Salary = 16500,
                Email = "admin.admin@bilgeadamboost.com",
                NormalizedEmail = "admin.admin@bilgeadamboost.com".ToUpper(),
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),

            };

            AppUser seedManager = new AppUser()
            {
                Id = 2,
                UserName = "Manager",
                NormalizedUserName = "Manager".ToUpper(),
                Name = "Manager",
                SecondName = "Manager",
                LastName = "Manager",
                Password = "123456aA-",
                SecondLastName = "Manager",
                PhoneNumber = "12345678901",
                BirthDate = DateTime.Now,
                BirthCity = "Manager",
                IdentityNumber = "12345678998",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CompanyInfo = "Manager",
                Job = "Manager",
                Department = Entities.Enums.Department.Engineer,
                Address = "İstanbul/Maltepe",
                Salary = 16500,
                Email = "manager.manager@bilgeadamboost.com",
                NormalizedEmail = "manager.manager@bilgeadamboost.com".ToUpper(),
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CompanyId = 1,
                //Company = seedCompany,
            };


            AppUser seedPersonel = new AppUser()
            {
                Id = 3,
                UserName = "Burak61",
                NormalizedUserName = "Burak61".ToUpper(),
                Name = "Burak",
                SecondName = "",
                LastName = "Ayan",
                Password = "123456aA-",
                SecondLastName = "",
                PhoneNumber = "905423985612",
                BirthDate = DateTime.Now,
                BirthCity = "Balıkesir",
                IdentityNumber = "41104925332",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CompanyInfo = "IT",
                Job = "Back End Developer",
                Department = Entities.Enums.Department.Engineer,
                Address = "İstanbul/Maltepe",
                Salary = 16500,
                Email = "burakayan@bilgeadamboost.com",
                NormalizedEmail = "burakayan@bilgeadamboost.com".ToUpper(),
                LockoutEnabled = false,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                CompanyId = 1,
                //Company = seedCompany,
            };


            //seed userin rolu manager olacak EKLENECEK!!

            PasswordHasher<AppUser> passwordHasherAdmin = new PasswordHasher<AppUser>();
            seedAdmin.PasswordHash = passwordHasherAdmin.HashPassword(seedAdmin, "123456aA-");

            PasswordHasher<AppUser> passwordHasherManager = new PasswordHasher<AppUser>();
            seedManager.PasswordHash = passwordHasherManager.HashPassword(seedManager, "123456aA-");

            PasswordHasher<AppUser> passwordHasherPersonel = new PasswordHasher<AppUser>();
            seedPersonel.PasswordHash = passwordHasherPersonel.HashPassword(seedPersonel, "123456aA-");

            IdentityUserRole<int> seedAdminRole = new IdentityUserRole<int>() { RoleId = 1, UserId = 1};
            builder.Entity<IdentityUserRole<int>>().HasData(seedAdminRole);

            IdentityUserRole<int> seedManagerRole = new IdentityUserRole<int>() { RoleId = 2, UserId = 2 };
            builder.Entity<IdentityUserRole<int>>().HasData(seedManagerRole);

            IdentityUserRole<int> seedPersonelRole = new IdentityUserRole<int>() { RoleId = 3, UserId = 3 };
            builder.Entity<IdentityUserRole<int>>().HasData(seedPersonelRole);



            builder.Entity<Company>().HasData(seedCompany);
            builder.Entity<AppUser>().HasData(seedAdmin);
            builder.Entity<AppUser>().HasData(seedPersonel);
            builder.Entity<AppUser>().HasData(seedManager);


            #endregion

            var decimalProps = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            base.OnModelCreating(builder);

        }

    }
}
