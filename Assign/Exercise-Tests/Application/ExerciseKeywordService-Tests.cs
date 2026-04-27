using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using Microsoft.Extensions.DependencyModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class AddExerciseKeyword_Tests
    {
        [Fact]
        public async Task AddExerciseKeyword_Should_Add_Keyword_To_Exercise()
        {
            //Arrange
            var keyword = new Keyword("keyword", KeywordType.SchoolSubject);
            var exercise = new Exercise("title", "content", new Guid());
            var keywordsRequests = new List<CreateExerciseKeywordRequest>
            {
                new CreateExerciseKeywordRequest
                {
                    KeywordId = keyword.Id
                }
            };
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id)).ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateAsync(exercise,exercise.RowVersion)).ReturnsAsync(exercise);
            var service = new ExerciseKeywordService(mockRepo.Object);

            //Act
            var result = await service.AddExerciseKeywordsAsync(exercise.Id, keywordsRequests);

            //Assert
            Assert.NotEmpty(result.ExerciseKeywords);
            Assert.Equal(result.ExerciseKeywords.Single().ExerciseId, exercise.ExerciseKeywords.Single().ExerciseId);
            Assert.Equal(result.ExerciseKeywords.Single().KeywordId, exercise.ExerciseKeywords.Single().KeywordId);
        }

        [Fact]
        public async Task AddExerciseKeyword_Should_Add_Multiple_Keywords_To_Exercise()
        {
            //Arrange
            var keyword1 = new Keyword("keyword1", KeywordType.SchoolSubject);
            var keyword2 = new Keyword("keyword2", KeywordType.SchoolSubject);
            var keyword3 = new Keyword("keyword1", KeywordType.SchoolSubject);
            var exercise = new Exercise("title", "content", new Guid());
            var keywordsRequests = new List<CreateExerciseKeywordRequest>
            {
                new CreateExerciseKeywordRequest {KeywordId = keyword1.Id},
                new CreateExerciseKeywordRequest { KeywordId = keyword2.Id},
                new CreateExerciseKeywordRequest{KeywordId = keyword3.Id},
            };

            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id)).ReturnsAsync(exercise);

            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);
            var service = new ExerciseKeywordService(mockRepo.Object);

            //Act
            var result = await service.AddExerciseKeywordsAsync(exercise.Id, keywordsRequests);

            //Assert
            Assert.Equal(keywordsRequests.Count, result.ExerciseKeywords.Count);
            foreach (var keyword in result.ExerciseKeywords)
            {
                Assert.Contains(keywordsRequests, ek => ek.KeywordId == keyword.KeywordId);
                Assert.Equal(keyword.ExerciseId, result.Id);
            }
        }


        [Fact]
        public async Task AddExerciseKeyword_Should_Not_Add_Duplicate_Keyword_To_Exercise()
        {
            // Arrange
            var keyword = new Keyword("math", KeywordType.SchoolSubject);
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            var keywordsRequests = new List<CreateExerciseKeywordRequest>
            {
                new CreateExerciseKeywordRequest { KeywordId = keyword.Id },
                new CreateExerciseKeywordRequest { KeywordId = keyword.Id }
            };
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id)).ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);
            var service = new ExerciseKeywordService(mockRepo.Object);

            // Act
            var result = await service.AddExerciseKeywordsAsync(exercise.Id, keywordsRequests);

            // Assert
            Assert.Single(result.ExerciseKeywords);
            Assert.Equal(keyword.Id, result.ExerciseKeywords.First().KeywordId);
        }
    }

    public class RemoveExerciseKeyword_Tests
    {
        [Fact]
        public async Task RemoveExerciseKeyword_Should_Remove_Keyword_From_Exercise()
        { 
            // Arrange
            var keyword = new Keyword("math", KeywordType.SchoolSubject);
            var exercise = new Exercise("title", "content", Guid.NewGuid());
            exercise.AddKeyword(keyword.Id);

            var keywordRequest = new RemoveExerciseKeywordRequest
            {
                ExerciseId = exercise.Id,
                KeywordId = keyword.Id
            };

            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            exercise.RemoveKeyword(keyword.Id) ;
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion)).ReturnsAsync(exercise);

            var service = new ExerciseKeywordService(mockRepo.Object);

            // Act
            var result = await service.RemoveExerciseKeywordAsync(exercise.Id, keywordRequest);

            // Assert
            Assert.Empty(result.ExerciseKeywords);
        }
    }
}