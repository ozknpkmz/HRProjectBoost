using HRProjectBoost.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Manager
{
    public class ManagerDetailsDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthCity { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CompanyInfo { get; set; }
        public string Job { get; set; }
        public Department Department { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public Status Status { get; set; }
    }
}
