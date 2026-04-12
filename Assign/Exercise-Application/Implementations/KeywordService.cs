using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class KeywordService : IKeywordService
    {
        private readonly IKeywordRepository _keywordRepository;

        public KeywordService(IKeywordRepository keywordRepository)
        {
            _keywordRepository = keywordRepository;
        }

        public async Task<KeywordDTO> AddKeywordAsync(KeywordDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
