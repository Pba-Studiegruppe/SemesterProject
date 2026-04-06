
using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IKeywordService
    {
        Task<KeywordDTO> AddKeywordAsync(KeywordDTO dto);
        Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync();
    }
}