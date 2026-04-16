using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class ExerciseRepository : IExerciseRepository
    {
        private DbContext dbContext;
        public ExerciseRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Exercise> AddExerciseAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> AddExerciseKeywordsASync(List<ExerciseKeyword> ExerciseKeywords)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> AddQuestionsASync(List<Question> Questions)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Exercise>> GetExercisesByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> RemoveExerciseSolutionAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> RemoveQuestionAsync(RemoveQuestionRequest dto)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> RemoveQuestionSolutionAsync(Guid questionId)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> SetExerciseSolution(ExerciseSolution ExerciseSolution)
        {
            throw new NotImplementedException();
        }

        public async Task<Exercise> UpdateExerciseAsync(Exercise exercise, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }

    }
}
