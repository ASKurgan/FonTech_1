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


        public async Task<TEntuty> CreateAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

         await _dbContext.AddAsync(entity);
         await _dbContext.SaveChangesAsync();

            return entity;
        }

        public IQueryable<TEntuty> GetAll()
        {
            return _dbContext.Set<TEntuty>();
        }

        public async Task<TEntuty> RemoveAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntuty> UpdateAsync(TEntuty entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entuty is null");
            }

           _dbContext.Update(entity);
           await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
