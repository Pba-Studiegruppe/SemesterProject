using Exercise_Api;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ExerciseDbContext _dbContext;

        public ExerciseRepository(ExerciseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Exercise> AddAsync(Exercise exercise)
        {
            _dbContext.Exercises.Add(exercise);
            await _dbContext.SaveChangesAsync();
            return exercise;
        }

        public async Task<Exercise?> GetByIdAsync(Guid exerciseId)
        {
            return await _dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);
        }

        public async Task<IEnumerable<Exercise>> GetByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            return await _dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .Where(e => e.ExerciseKeywords.Any(ek => keywordIds.Contains(ek.KeywordId)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByTeacherIdAsync(Guid teacherId)
        {
            return await _dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .Where(e => e.CreatedByTeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<Exercise> UpdateAsync(Exercise exercise, byte[] rowVersion)
        {
            _dbContext.Entry(exercise).OriginalValues["RowVersion"] = rowVersion;
            _dbContext.Exercises.Update(exercise);
            await _dbContext.SaveChangesAsync();
            return exercise;
        }

        public async Task<Exercise?> GetForSnapshotAsync(Guid exerciseId)
        {
            // Solutions deliberately not loaded — wrong tool for the job.
            return await _dbContext.Exercises
                .AsNoTracking()
                .Include(e => e.Questions)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);
        }

        public async Task<Exercise?> GetForReviewAsync(Guid exerciseId)
        {
            return await _dbContext.Exercises
                .AsNoTracking()
                .Include(e => e.Solution)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.Solution)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);
        }

        public async Task<IEnumerable<Exercise>> GetAllAsync(string? search = null)
        {
            var query = _dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.Title.Contains(search) ||
                    e.Content.Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}
