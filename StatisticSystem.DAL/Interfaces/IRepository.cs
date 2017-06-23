using StatisticSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StatisticSystem.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        void Create(T item);
        T Find(Func<T, bool> predicate);
        IDictionary<int, IEnumerable<T>> GetSpan(int skipNum, int sizeNum);
        IEnumerable<T> GetAll(Expression<Func<T, string>> expression);
    }
}
