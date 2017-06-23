using System.Collections.Generic;
using System.Linq;
using Aura.Core.Interfaces;
using Aura.Core.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Aura.Infrastructure.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        public List<T> All()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            SaveChanges();

            return entity;
        }

        /// <summary>
        /// I dont like hard delete, if needed for future, uncomment it
        /// </summary>
        /// <param name="entity"></param>
        //public void Delete(T entity)
        //{
        //    _dbContext.Set<T>().Remove(entity);
        //    SaveChanges();
        //}

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}