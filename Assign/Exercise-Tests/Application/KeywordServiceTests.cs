using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class ReadKeywordServiceTests
    {
        [Fact]
        public async Task GetKeywordById_ReturnsKeyword_WhenKeywordExists()
        {
            // Arrange
            KeywordType keywordType = KeywordType.SchoolSubject;
            var expectedKeyword = new Keyword ("Mathematics", keywordType);

            var mockRepository = new Mock<IKeywordRepository>();
            mockRepository.Setup(repo => repo.GetKeywordByIdAsync(expectedKeyword.Id))
                          .ReturnsAsync(expectedKeyword);
            var service = new KeywordService(mockRepository.Object);
            // Act
            var result = await service.GetKeywordAsync(expectedKeyword.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedKeyword.Id, result.Id);
            Assert.Equal(expectedKeyword.KeywordName, result.KeywordName);
            Assert.Equal(expectedKeyword.KeywordType.ToString(), result.KeywordType);
        }

        [Fact]
        public async Task GetAllKeywords_ReturnsListOfKeywords()
        {
            // Arrange
            var keywords = new List<Keyword>
            {
                new Keyword("Mathematics", KeywordType.SchoolSubject),
                new Keyword("Algebra", KeywordType.ExerciseType)
            };
            var mockRepository = new Mock<IKeywordRepository>();
            mockRepository.Setup(repo => repo.GetAllKeywordsAsync())
                          .ReturnsAsync(keywords);
            var service = new KeywordService(mockRepository.Object);
            // Act
            var result = await service.GetAllKeywordsAsync();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

    }

    public class CreateKeywordServiceTests
    {
        [Fact]
        public async Task CreateKeyword_ReturnsCreatedKeyword()
        {
            // Arrange
            KeywordType keywordType = KeywordType.SchoolSubject;
            var newKeyword = new Keyword("Mathematics", keywordType);
            CreateKeywordRequest createRequest = new CreateKeywordRequest
            {
                KeywordName = "Geometry",
                KeywordType = keywordType.ToString()
            };


            var mockRepository = new Mock<IKeywordRepository>();
            mockRepository.Setup(repo => repo.AddKeywordAsync(It.IsAny<Keyword>()))
                          .ReturnsAsync(newKeyword);

            var service = new KeywordService(mockRepository.Object);

            // Act
            var result = await service.CreateKeywordAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newKeyword.Id, result.Id);
            Assert.Equal(newKeyword.KeywordName, result.KeywordName);
            Assert.Equal(newKeyword.KeywordType.ToString(), result.KeywordType);

        }
    }
}
