using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Domain.Entities;
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

        public async Task<KeywordDTO> CreateKeywordAsync(CreateKeywordRequest dto)
        {
            try
            {
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The KeywordDTO cannot be null."); }
                if (string.IsNullOrWhiteSpace(dto.KeywordName)) { throw new ArgumentException("The KeywordName cannot be null or whitespace.", nameof(dto.KeywordName)); }
                if (!Enum.TryParse(dto.KeywordType, out KeywordType keywordType)) { throw new ArgumentException("Invalid KeywordType value.", nameof(dto.KeywordType)); }


                var keyword = new Keyword(dto.KeywordName, keywordType);


                var createdKeyword = await _keywordRepository.AddKeywordAsync(keyword);
                return new KeywordDTO
                {
                    Id = createdKeyword.Id,
                    KeywordName = createdKeyword.KeywordName,
                    KeywordType = createdKeyword.KeywordType.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the keyword.", ex);
            }
        }

        public async Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync()
        {
            try
            {
                var keywords = await _keywordRepository.GetAllKeywordsAsync();
                return keywords.Select(k => new KeywordDTO
                {
                    Id = k.Id,
                    KeywordName = k.KeywordName,
                    KeywordType = k.KeywordType.ToString()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all keywords.", ex);
            }
        }

        public async Task<KeywordDTO> GetKeywordAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty) { throw new ArgumentException("ID cannot be empty.", nameof(id)); }

                var keyword = await _keywordRepository.GetKeywordByIdAsync(id);
                if (keyword == null) { throw new KeyNotFoundException($"Keyword with ID {id} not found."); }

                return new KeywordDTO
                {
                    Id = keyword.Id,
                    KeywordName = keyword.KeywordName,
                    KeywordType = keyword.KeywordType.ToString()
                };


            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the keyword.", ex);

            }
        }
    }
}
