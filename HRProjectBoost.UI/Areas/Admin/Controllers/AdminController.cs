
using AutoMapper;
using FluentValidation;
using HRProjectBoost.Business.FluentValidations;
using HRProjectBoost.DataAccess.Context;
using HRProjectBoost.DTOs.DTOs.Admin;
using HRProjectBoost.DTOs.DTOs.Allowance;
using HRProjectBoost.DTOs.DTOs.Manager;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using HRProjectBoost.Entities.Enums;
using HRProjectBoost.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace HRProjectBoost.UI.Areas.Manager.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly HRProjectContext _context;
        private readonly IMapper _mapper;

        public AdminController(UserManager<AppUser> userManager, HRProjectContext context, IMapper mapper, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            TempData["company"] = datas.CompanyInfo;
            return View(_mapper.Map<ManagerDto>(datas));
        }

        [HttpGet]
        public async Task<IActionResult> CreateCompany()
        {



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyCreateDto dto)
        {
            Company createdCompany = _mapper.Map<Company>(dto);


            var files = Request.Form.Files;
            if (files.Count != 0)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    files[0].CopyTo(memoryStream);
                    createdCompany.Logo = memoryStream.ToArray();
                }
            }


            createdCompany.AgreementStartDate = DateTime.Now;
            createdCompany.AgreementEndDate = createdCompany.AgreementStartDate.AddYears(2);
            createdCompany.CompanyEmail = "support@" + createdCompany.CompanyName.Trim().ToLower() + ".com";
            createdCompany.CompanyStatus = Entities.Enums.CompanyStatus.Active;


            _context.Company.Add(createdCompany);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListCompaniesAndManagers");

        }

        [HttpGet]
        public async Task<IActionResult> ListCompaniesAndManagers()
        {
            var company = _context.Company.Include(x => x.AppUser).ToList();

            List<CompanyDto> dto = _mapper.Map<List<CompanyDto>>(company);

            foreach (var item in dto)
            {
                if (item.AppUser != null)
                {
                    foreach (var manager in item.AppUser)
                    {
                        var IsManager = await _userManager.IsInRoleAsync(manager, "Manager");
                        if (IsManager)
                        {
                            item.Managers.Add(manager);
                        }
                    }
                }
            }
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> AddManager(string MersisNo)
        {
            Company company = _context.Company.FirstOrDefault(x => x.MersisNo == MersisNo);

            ManagerCreateDto dto = new ManagerCreateDto()
            {
                CompanyId = company.CompanyId
            };

            return View(dto);

        }

        [HttpPost]
        public async Task<IActionResult> AddManager(ManagerCreateDto manager)
        {
            AppUser newManager = _mapper.Map<AppUser>(manager);
            Company company = await _context.Company.FindAsync(manager.CompanyId);

            var password = GenerateRandomPassword();

            newManager.Company = company;
            newManager.CompanyInfo = company.CompanyName;
            newManager.UserName = $"{newManager.Name}.{newManager.LastName}";
            newManager.Password = password;
            newManager.Email = $"{newManager.Name}{newManager.LastName}@bilgeadamboost.com".ToLower();

            SendPasswordEmail(newManager.Email, password);

            var result = await _userManager.CreateAsync(newManager, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newManager, "Manager");

                // Company sınıfına yeni kullanıcıyı ekleyin
                company.AppUser.Add(newManager);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListCompaniesAndManagers");

        }



        [HttpPost]
        public async Task<IActionResult> DeleteCompany(string MersisNo)
        {
            var companyToBeDeleted = _context.Company.FirstOrDefault(x=> x.MersisNo== MersisNo);
            var appUsersToDelete = _context.Users.Where(u => u.CompanyId == companyToBeDeleted.CompanyId);
            _context.Users.RemoveRange(appUsersToDelete);
            _context.Company.Remove(companyToBeDeleted);
            _context.SaveChanges();

            foreach (var user in appUsersToDelete)
            {
                // Role, Allowance ve Advance tablolarındaki verileri sil
                var roleToDelete = _context.UserRoles.FirstOrDefault(r => r.UserId == user.Id);
                var allowanceToDelete = _context.Allowance.FirstOrDefault(a => a.AppUserId == user.Id);
                var advanceToDelete = _context.Advance.FirstOrDefault(ad => ad.AppUserId == user.Id);

                _context.UserRoles.Remove(roleToDelete);
                _context.Allowance.Remove(allowanceToDelete);
                _context.Advance.Remove(advanceToDelete);
                _context.SaveChanges();

            }


            return RedirectToAction("ListCompaniesAndManagers");
        }



        public string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, length)
                                                .Select(s => s[random.Next(s.Length)])
                                                .ToArray());
            return password;
        }

        public async Task SendPasswordEmail(string userEmail, string password)
        {
            var subject = "Yeni Şifreniz";
            var body = $"Yeni şifreniz: {password}";


            MailMessage mesaj = new MailMessage();
            mesaj.From = new MailAddress("ucsilahsorlerburger@gmail.com");
            mesaj.To.Add(userEmail);
            mesaj.Subject = subject;
            mesaj.Body = body;

            SmtpClient a = new SmtpClient();
            a.Credentials = new System.Net.NetworkCredential("ucsilahsorlerburger@gmail.com", "xdgpgeouvssyfpzk");
            a.Port = 587;
            a.Host = "smtp.gmail.com";
            a.EnableSsl = true;
            object userState = mesaj;
            //a.Send(mesaj);
            a.SendAsync(mesaj, userState);

        }

    }

}
