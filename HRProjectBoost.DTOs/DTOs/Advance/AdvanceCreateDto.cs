using HRProjectBoost.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DTOs.DTOs.Advance
{
    public class AdvanceCreateDto
    {
        public int Total { get; set; }
        public decimal Salary { get; set; }
        public string? Description { get; set; }
        public AdvanceStatus AdvanceStatus { get; set; } = AdvanceStatus.Waiting;
        public AdvanceType AdvanceType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
