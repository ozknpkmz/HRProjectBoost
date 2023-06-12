using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Personnel
{
    public class PersonnelUpdateDTO
    {
        public byte[]? ProfilePicture { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserName { get; set; }
    }
}
