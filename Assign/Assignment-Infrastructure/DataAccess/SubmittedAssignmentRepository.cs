using Assignment_Api;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Infrastructure.DataAccess
{
    public class SubmittedAssignmentRepository : ISubmittedAssignmentRepository
    {
        private readonly AssignmentDbContext _dbContext;

        public SubmittedAssignmentRepository(AssignmentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Eager-loads the full graph the service needs: the submission's
        /// children (SubmittedExercises → SubmittedQuestions) for the
        /// scoring/comment data, plus the parent Assignment graph
        /// (AssignmentExercises → Questions) for the denormalized source
        /// fields on the DTO. AsSplitQuery prevents a Cartesian explosion
        /// across the two independent collection branches.
        /// </summary>
        public async Task<SubmittedAssignment?> GetByIdAsync(Guid id)
        {
            return await _dbContext.SubmittedAssignments
                .Include(s => s.Assignment)
                    .ThenInclude(a => a!.AssignmentExercises)
                        .ThenInclude(ae => ae.Questions)
                .Include(s => s.SubmittedExercises)
                    .ThenInclude(se => se.SubmittedQuestions)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Returns every submission for the given assignment. The parent
        /// Assignment is not eager-loaded here — the service fetches that
        /// once via IAssignmentRepository and reuses it across all submissions
        /// in the listing.
        /// </summary>
        public async Task<IEnumerable<SubmittedAssignment>> GetByAssignmentIdAsync(Guid assignmentId)
        {
            return await _dbContext.SubmittedAssignments
                .Include(s => s.SubmittedExercises)
                    .ThenInclude(se => se.SubmittedQuestions)
                .AsSplitQuery()
                .Where(s => s.AssignmentId == assignmentId)
                .ToListAsync();
        }

        public Task CreateAsync(SubmittedAssignment submittedAssignment)
        {
            _dbContext.SubmittedAssignments.Add(submittedAssignment);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
