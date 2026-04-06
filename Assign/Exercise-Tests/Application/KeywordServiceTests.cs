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
    public class KeywordServiceTests
    {
    }

    public class CreateKeywordServiceTests
    {
        [Fact]
        public void CreateKeyword_ShouldCallRepositorySave()
        {
            // Arrange
            var mockRepo = new Mock<IKeywordRepository>();
            var service = new KeywordService(mockRepo.Object);
            // Act
            service.CreateKeyword("Keyword Name", Guid.NewGuid());
            // Assert
            mockRepo.Verify(
                r => r.AddKeyword(It.IsAny<Keyword>()),
                Times.Once
            );
        }
        [Fact]
        public void CreateKeyword_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<IKeywordRepository>();
            mockRepo.Setup(r => r.AddKeyword(It.IsAny<Keyword>()))
                    .Throws(new Exception("Database error"));
            var service = new KeywordService(mockRepo.Object);
            // Act & Assert
            Assert.Throws<Exception>(() => service.CreateKeyword("Keyword Name", Guid.NewGuid()));
        }
    }

    public class ReadKeywordServiceTests
    {
        [Fact]
        public void GetKeyword_ShouldCallRepositoryToGet()
        {
            // Arrange
            var mockRepo = new Mock<IKeywordRepository>();
            var service = new KeywordService(mockRepo.Object);
            var keywordId = Guid.NewGuid();
            // Act
            service.GetKeyword(keywordId);
            // Assert
            mockRepo.Verify(
                r => r.GetKeywordById(keywordId),
                Times.Once
            );
        }

        [Fact]
        public void GetAllKeywords_ShouldCallRepositoryToGetAll()
        {
            // Arrange
            var mockRepo = new Mock<IKeywordRepository>();
            var service = new KeywordService(mockRepo.Object);
            // Act
            service.GetAllKeywords();
            // Assert
            mockRepo.Verify(
                r => r.GetAllKeywords(),
                Times.Once
            );
        }
    }
}
