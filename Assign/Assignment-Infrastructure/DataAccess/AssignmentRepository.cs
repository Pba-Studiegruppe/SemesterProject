using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Infrastructure.DataAccess
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AssignmentDbContext _dbContext;

        public AssignmentRepository(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Task CreateAsync(Assignment assignment)
        {
            _dbContext.Assignments.Add(assignment);
            return Task.CompletedTask;
        }

        public async Task<Assignment?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Assignments
                .Include(a => a.AssignmentExercises)
                    .ThenInclude(ae => ae.Questions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}