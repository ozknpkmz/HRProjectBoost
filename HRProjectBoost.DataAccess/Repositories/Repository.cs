﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRProjectBoost.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
    }
}
