using Exercise_Api;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ExerciseDbContext dbContext;

        public ExerciseRepository(ExerciseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Exercise> AddAsync(Exercise exercise)
        {
            dbContext.Exercises.Add(exercise);
            await dbContext.SaveChangesAsync();
            return exercise;
        }

        public async Task<Exercise?> GetByIdAsync(Guid exerciseId)
        {
            return await dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .FirstOrDefaultAsync(e => e.Id == exerciseId);
        }

        public async Task<IEnumerable<Exercise>> GetByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            return await dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .Where(e => e.ExerciseKeywords.Any(ek => keywordIds.Contains(ek.KeywordId)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByTeacherIdAsync(Guid teacherId)
        {
            return await dbContext.Exercises
                .Include(e => e.Questions)
                .Include(e => e.ExerciseKeywords)
                .Include(e => e.Solution)
                .Where(e => e.CreatedByTeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<Exercise> UpdateAsync(Exercise exercise, byte[] rowVersion)
        {
            dbContext.Entry(exercise).OriginalValues["RowVersion"] = rowVersion;
            dbContext.Exercises.Update(exercise);
            await dbContext.SaveChangesAsync();
            return exercise;
        }
    }
}
