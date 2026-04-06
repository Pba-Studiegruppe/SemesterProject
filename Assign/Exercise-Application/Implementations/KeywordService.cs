using Exercise_Application.DTO;
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
        public Task<KeywordDTO> AddKeywordAsync(KeywordDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
