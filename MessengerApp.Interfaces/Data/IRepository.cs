using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApp.Interfaces.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> GetEntities();
        T GetEntity(int id);
        void Create(T item);
        void Create(IEnumerable<T> items);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
