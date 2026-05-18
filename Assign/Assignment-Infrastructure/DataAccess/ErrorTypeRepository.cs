using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Infrastructure.DataAccess
{
    public class ErrorTypeRepository : IErrorTypeRepository
    {
        private readonly AssignmentDbContext _dbContext;

        public ErrorTypeRepository(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorType?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ErrorTypes
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<ErrorType>> GetAllAsync()
        {
            return await _dbContext.ErrorTypes
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public Task CreateAsync(ErrorType errorType)
        {
            _dbContext.ErrorTypes.Add(errorType);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
