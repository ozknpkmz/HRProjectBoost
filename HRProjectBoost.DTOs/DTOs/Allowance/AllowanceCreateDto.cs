using HRProjectBoost.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Allowance
{
    public class AllowanceCreateDto
    {
        public AllowanceType AllowanceType { get; set; }
        public int Total { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public AllowanceStatus AllowanceStatus { get; set; } = AllowanceStatus.Waiting;
        public DateTime AllowanceCreatedTime { get; set; } = DateTime.Parse(DateTime.Now.ToShortDateString());
        public string Path { get; set; }
        public IFormFile? AllowanceFile { get; set; }
       
    }
}
