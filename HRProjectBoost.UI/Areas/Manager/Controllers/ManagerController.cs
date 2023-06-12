
using AutoMapper;
using FluentValidation;
using HRProjectBoost.Business.FluentValidations;
using HRProjectBoost.DataAccess.Context;
using HRProjectBoost.DTOs.DTOs.Advance;
using HRProjectBoost.DTOs.DTOs.Allowance;
using HRProjectBoost.DTOs.DTOs.Manager;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using HRProjectBoost.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Net.Mail;

namespace HRProjectBoost.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManagerController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly HRProjectContext _context;
        private readonly IMapper _mapper;

        public ManagerController(UserManager<AppUser> userManager, HRProjectContext context, IMapper mapper, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
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
        public async Task<IActionResult> Details(ManagerDetailsDto dto)
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            return View(_mapper.Map<ManagerDetailsDto>(datas));
        }

        [HttpGet]
        public async Task<ActionResult> GetPersonelDetails(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            var dto = _mapper.Map<PersonnelDetailsDTO>(user);
            return View("PersonnelDetails", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var datas = await _userManager.FindByNameAsync(this.User.Identity.Name);
            return View(_mapper.Map<ManagerUpdateDto>(datas));
        }

        [HttpPost]
        public async Task<object> Update(ManagerUpdateDto dto)
        {
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);
            var UpdateValidator = new ManagerUpdateDtoValidator();
            var validatorResult = UpdateValidator.Validate(dto);
            if (ModelState.IsValid && validatorResult.IsValid)
            {
                var files = Request.Form.Files;

                if (files.Count != 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        files[0].CopyTo(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                }

                user.Address = dto.Address;
                user.PhoneNumber = dto.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                _context.SaveChanges();

                if (result.Succeeded)
                    return RedirectToAction("Index", "Manager");
                else
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
            }
            else
            {
                //ModelState.AddModelError("", "Hatalar oluştu.");

                string errorMessage = string.Join(", ", validatorResult.Errors.Select(error => error.ErrorMessage));
                ViewBag.ErrorMessage = errorMessage; // Hata alabiliyoruz ancak msji ekrana veremedik henuz viewbag yerine Toastr kurulacak...
                return RedirectToAction("Update", dto);
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreatePersonnel()//GENERIC KURULACAK!!!
        {
            return View(new PersonnelCreateDTO() { UserName = "deneme" });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonnel(PersonnelCreateDTO dto)//GENERIC KURULACAK!!!
        {
            var password = GenerateRandomPassword();
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);
            if (ModelState.IsValid)
            {
                AppUser appuser = new()
                {
                    UserName = $"{dto.Name}.{dto.LastName}",
                    Name = dto.Name,
                    SecondName = dto.SecondName,
                    LastName = dto.LastName,
                    SecondLastName = dto.LastName,
                    Password = password,
                    PhoneNumber = dto.PhoneNumber,
                    BirthDate = dto.BirthDate,
                    BirthCity = dto.BirthCity,
                    IdentityNumber = dto.IdentityNumber,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    CompanyInfo = user.CompanyInfo,
                    Job = dto.Job,
                    Department = dto.Department,
                    Address = dto.Address,
                    Salary = dto.Salary,
                    Email = $"{dto.Name}{dto.LastName}@bilgeadamboost.com".ToLower(),
                };

                await SendPasswordEmail(appuser.Email, password);


                var result = await _userManager.CreateAsync(appuser, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appuser, "Personnel");
                    return RedirectToAction("GetPersonnelList"); //"Index");

                }
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError("", item.Description);
                }
            }
            return View(dto);
        }

        [Route("Manager/Manager/DeletePersonel/{Email?}")] //GENERIC KURULACAK!!!
        public async Task<IActionResult> DeletePersonnel(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("GetPersonnelList");
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonnelList(PersonnelDTO dto)
        {
            var activeManager = await _userManager.FindByNameAsync(this.User.Identity.Name);
            var users = await _context.Users.Where(x=>x.CompanyId== activeManager.CompanyId).ToListAsync();
            var map = _mapper.Map<List<PersonnelDTO>>(users);
            return View(map);

        }

        [HttpGet]
        public IActionResult GetAllowanceList()
        {
            List<Allowance> allowanceList = _context.Allowance.Include(x => x.AppUser).ToList();
            var dto = _mapper.Map<List<AllowanceDto>>(allowanceList);
            return View(dto);
        }

        [Route("Manager/{controller}/{action}/{id?}")]
        public async Task<IActionResult> DeclineAllowance(int id)
        {
            ViewBag.denied = "Allowance Denied";
            var allowance = await _context.Allowance.FindAsync(id);
            allowance.AllowanceStatus = Entities.Enums.AllowanceStatus.Denied;
            _context.Allowance.Update(allowance);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAllowanceList", TempData["denied"]);
        }

        [Route("Manager/{controller}/{action}/{id?}")]
        public async Task<IActionResult> AcceptAllowance(int id)
        {
            ViewBag.access = "Allowance Accepted";

            var allowance = await _context.Allowance.FindAsync(id);
            allowance.AllowanceStatus = Entities.Enums.AllowanceStatus.Accepted;
            _context.Allowance.Update(allowance);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetAllowanceList");
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvanceList()
        {
            List<Advance> advances = await _context.Advance.Include(x => x.AppUser).ToListAsync();
            var dto = _mapper.Map<List<AdvanceDto>>(advances);
            return View(dto);
        }

        [Route("Manager/{controller}/{action}/{id?}")]
        public async Task<IActionResult> AcceptAdvance(int Id)
        {
            ViewBag.access = "Allowance Accepted";

            var advance = await _context.Advance.FindAsync(Id);
            advance.AdvanceStatus = Entities.Enums.AdvanceStatus.Accepted;
            _context.Advance.Update(advance);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAdvanceList");
        }
        [Route("Manager/{controller}/{action}/{id?}")]
        public async Task<IActionResult> DeclineAdvance(int Id)
        {
            ViewBag.access = "Allowance Accepted";

            var advance = await _context.Advance.FindAsync(Id);
            advance.AdvanceStatus = Entities.Enums.AdvanceStatus.Denied;
            _context.Advance.Update(advance);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetAdvanceList");
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
