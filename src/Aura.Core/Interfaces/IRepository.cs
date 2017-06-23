using System.Collections.Generic;
using Aura.Core.SharedKernel;

namespace Aura.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        List<T> All();
        T Add(T entity);
        void Update(T entity);

        /// <summary>
        /// I dont like hard delete, if needed for future, uncomment it
        /// </summary>
        // void Delete(T entity);

        void SaveChanges();
    }
}