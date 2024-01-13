using FonTech.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.DAL.Repositories
{
    public class BaseRepository<TEntuty> : IBaseRepository<TEntuty> where TEntuty : class
    {

        private readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task<TEntuty> CreateAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);
        }

        public IQueryable<TEntuty> GetAll()
        {
            return _dbContext.Set<TEntuty>();
        }

        public Task<TEntuty> RemoveAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

            _dbContext.Remove(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<TEntuty> UpdateAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return Task.FromResult(entity);
        }
    }
}
