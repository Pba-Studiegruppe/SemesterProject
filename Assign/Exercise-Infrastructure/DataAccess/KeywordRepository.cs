using Exercise_Api;
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
    public class KeywordRepository : IKeywordRepository
    {
        private ExerciseDbContext _dbContext;
        public KeywordRepository(ExerciseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Keyword> AddKeywordAsync(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Keyword> GetKeywordByIdAsync(Guid keywordId)
        {
            throw new NotImplementedException();
        }
    }
}
