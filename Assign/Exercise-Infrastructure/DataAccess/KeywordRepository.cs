using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Infrastructure.DataAccess
{
    public class KeywordRepository : IKeywordRepository
    {
        public Task<Keyword> AddKeywordAsync(Keyword keyword)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Keyword> GetKeywordByIdAsync(Guid keywordId)
        {
            throw new NotImplementedException();
        }
    }
}
