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

        public Task<Exercise> AddAsync(Exercise exercise)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise?> GetByIdAsync(Guid exerciseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exercise>> GetByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exercise>> GetByTeacherIdAsync(Guid teacherId)
        {
            throw new NotImplementedException();
        }

        public Task<Exercise> UpdateAsync(Exercise exercise, byte[] rowVersion)
        {
            throw new NotImplementedException();
        }
    }
}
