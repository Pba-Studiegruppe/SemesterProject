using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IKeywordRepository
    {
        Task<Keyword> AddKeywordAsync(Keyword keyword);
        Task<IEnumerable<Keyword>> GetAllKeywordsAsync();
        Task<Keyword> GetKeywordByIdAsync(Guid keywordId);
    }
}
