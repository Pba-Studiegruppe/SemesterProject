using Exercise_Api;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exercise_Infrastructure.DataAccess
{
    public class KeywordRepository : IKeywordRepository
    {
        private readonly ExerciseDbContext _dbContext;

        public KeywordRepository(ExerciseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Keyword> AddKeywordAsync(Keyword keyword)
        {
            _dbContext.Keywords.Add(keyword);
            await _dbContext.SaveChangesAsync();
            return keyword;
        }

        public async Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            return await _dbContext.Keywords.ToListAsync();
        }

        public async Task<Keyword> GetKeywordByIdAsync(Guid keywordId)
        {
            var keywords = await _dbContext.Keywords.FirstOrDefaultAsync(k => k.Id == keywordId);
            if (keywords == null) {throw new Exception("Keyword not found");}
            return keywords;
        }
    }
}