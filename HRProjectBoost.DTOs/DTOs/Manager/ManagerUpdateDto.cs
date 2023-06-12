using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Manager
{
    public class ManagerUpdateDto
    {
        public byte[]? ProfilePicture { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserName { get; set; }
    }
}
