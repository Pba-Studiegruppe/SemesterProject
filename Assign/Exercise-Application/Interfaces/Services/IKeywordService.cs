
using Exercise_Application.DTO;

namespace Exercise_Application.Interfaces.Services
{
    public interface IKeywordService
    {
        Task<KeywordDTO> CreateKeywordAsync(CreateKeywordRequest dto);
        Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync();
        Task<KeywordDTO> GetKeywordAsync(Guid id);
    }
}