using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.Entities.Enums
{
    public enum Department
    {
        [Display(Name = "Human Resources")]
        HumanResources = 1,
        Engineer = 2,
        IT = 3,
    }
}
