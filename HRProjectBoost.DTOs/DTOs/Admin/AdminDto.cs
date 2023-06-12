using HRProjectBoost.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Admin
{
    public class AdminDto
    {
        public byte[]? ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }
        public Department Department { get; set; }
    }
}
