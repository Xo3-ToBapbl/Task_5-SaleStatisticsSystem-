﻿using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        void Create(T item);
        T Find(Func<T, bool> predicate);
    }
}
