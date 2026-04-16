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
    public class AddQuestionTests
    {
        [Fact]
        public async Task AddQuestionAsync_ShouldCallRepositoryAddQuestions()
        {
           
        }

        [Fact]
        public async Task AddQuestionAsync_Should_Add_Question_To_Exercise()
        {

        }
        [Fact]
        public async Task AddQuestionAsync_Should_Add_Multiple_Questions_To_Exercise()
        {

        }
        [Fact]
        public async Task AddQuestionAsync_Should_Not_Add_Question_With_Empty_Title()
        {

        }
        [Fact]
        public async Task AddQuestionAsync_Should_Not_Add_Question_With_Empty_Content()
        {

        }
    }

    public class UpdateQuestionTests
    {
        [Fact]
        public async Task UpdateQuestionAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            exercise.AddQuestion("Question1", "Content1");
            var questionToUpdate = exercise.Questions.First();
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateQuestionRequest
            {
                Id = questionToUpdate.Id,
                Title = "Updated Question Title",
                Content = "Updated Question Content",
                RowVersion = questionToUpdate.RowVersion
            };
            mockRepo.Setup(r => r.GetExerciseByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateExerciseAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.UpdateQuestionAsync(updateRequest);
            // Assert
            mockRepo.Verify(r => r.UpdateExerciseAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Once);
        }
        [Fact]
        public async Task UpdateQuestionAsync_Should_Update_Question_Properties()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            exercise.AddQuestion("Question1", "Content1");
            var questionToUpdate = exercise.Questions.First();
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateQuestionRequest
            {
                Id = questionToUpdate.Id,
                Title = "Updated Question Title",
                Content = "Updated Question Content",
                RowVersion = questionToUpdate.RowVersion
            };
            mockRepo.Setup(r => r.GetExerciseByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateExerciseAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.UpdateQuestionAsync(updateRequest);
            // Assert
            Assert.Equal(updateRequest.Title, questionToUpdate.Title);
            Assert.Equal(updateRequest.Content, questionToUpdate.Content);
        }

        public async Task UpdateQuestionAsync_Should_Throw_When_Updating_Nonexistent_Question()
        {
        }
        public async Task UpdateQuestionAsync_Should_Update_Only_Targeted_Question() { }
    }

    public class RemoveQuestionsTests
    {
        public async Task RemoveQuestionAsync_Should_Remove_Question_From_Exercise()
        {
        }
        public async Task RemoveQuestionAsync_Should_Throw_When_Removing_Nonexistent_Question()
        {
        }
        public async Task RemoveQuestionAsync_Should_Remove_Only_Targeted_Question()
        {
        }
    }
}

