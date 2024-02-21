using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonTech.Domain.Interfaces.Databases
{
    public interface IUnitOfWork : IDisposable, IStateSaveChanges 
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IBaseRepository<User> Users { get; set; }
        IBaseRepository<Role> Roles { get; set; }
        IBaseRepository<UserRole> UserRoles { get; set; }
    }
}
