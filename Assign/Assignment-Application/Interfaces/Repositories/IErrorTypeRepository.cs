using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Repositories
{
    public interface IErrorTypeRepository
    {
        Task<ErrorType?> GetByIdAsync(Guid id);
        Task<IEnumerable<ErrorType>> GetAllAsync();
        Task CreateAsync(ErrorType errorType);
        Task SaveChangesAsync();
    }
}
