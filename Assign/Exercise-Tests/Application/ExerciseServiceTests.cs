using Exercise_Application.DTO;
using Exercise_Application.Implementations;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;
using FluentAssertions.Common;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Application
{
    public class ExerciseServiceTests
    {

    }

    public class CreateExerciseServiceTests
    {
        public static IEnumerable<object[]> ExerciseTestData()
        {
            List<QuestionDTO> testQuestions = new List<QuestionDTO>();

            yield return new object[]
            {
            new CreateExerciseRequest
            {
                Title = "Title1",
                Content = "Content1",
                CreatedByTeacherId = Guid.NewGuid(),
            }
            };

            yield return new object[]
            {
            new CreateExerciseRequest
            {
                Title = "Title2",
                Content = "Content2",
                CreatedByTeacherId = Guid.NewGuid(),

            }
            };
        }

        [Theory]
        [MemberData(nameof(ExerciseTestData))]
        public async Task CreateExercise_ShouldCallRepositorySave(CreateExerciseRequest exerciseDto)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);

            // Assert

            mockRepo.Verify(r => r.AddAsync(It.IsAny<Exercise>()), Times.Once);
        }

        [Fact]
        public async Task CreateExercise_ShouldReturnCreatedExercise()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var exerciseDto = new CreateExerciseRequest
            {
                Title = "Title1",
                Content = "Content1",
                CreatedByTeacherId = Guid.NewGuid(),
            };
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExercise.Id, result.Id);
            Assert.Equal(expectedExercise.Title, result.Title);
            Assert.Equal(expectedExercise.Content, result.Content);
        }


        [Theory]
        [MemberData(nameof(ExerciseTestData))]
        public async Task CreateExercise_ShouldReturnCreatedExercise_DDT(CreateExerciseRequest exerciseDto)
        {
            //This test is like the one above, except it uses DDT, try to compare how fast they run.
            //Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExercise.Id, result.Id);
            Assert.Equal(expectedExercise.Title, result.Title);
            Assert.Equal(expectedExercise.Content, result.Content);
        }

        [Fact]
        public async Task CreateExercise_Should_Create_Multiple_Questions()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var questionDto1 = new CreateQuestionRequest { Title = "T1", Content = "C1" };
            var questionDto2 = new CreateQuestionRequest { Title = "T2", Content = "C2" };
            var exerciseDto = new CreateExerciseRequest
            {
                Title = "Exercise",
                Content = "Content",
                CreatedByTeacherId = Guid.NewGuid(),
            };
            exerciseDto.Questions.Add(questionDto1);
            exerciseDto.Questions.Add(questionDto2);

            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            expectedExercise.AddQuestion(questionDto1.Title, questionDto1.Content);
            expectedExercise.AddQuestion(questionDto2.Title, questionDto2.Content);

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            //act
            var result = await service.CreateExerciseAsync(exerciseDto);

            // Assert

            Assert.NotEmpty(result.Questions);
            for (int i = 0; i < result.Questions.Count; i++)
            {
                var question = result.Questions[i];
                var input = exerciseDto.Questions[i];

                Assert.Equal(input.Title, question.Title);
                Assert.Equal(input.Content, question.Content);
            }
        }

        [Fact]
        public async Task CreateExercise_Should_Create_QuestionSolution_for_Question()
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            var question = new Question("title", "content");
            question.SetSolution("solutionConent");

            var questionSolutionDto = new CreateQuestionSolutionRequest { Content = question.Solution.Content };
            var exerciseDto = new CreateExerciseRequest
            {
                Title = "Exercise",
                Content = "Content",
                CreatedByTeacherId = Guid.NewGuid(),
                Questions = { new CreateQuestionRequest
                {
                    Title = question.Title,
                    Content = question.Content,
                    Solution = questionSolutionDto
                } }
            };
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            expectedExercise.AddQuestion(question.Title, question.Content);
            expectedExercise.Questions.Last().SetSolution(question.Solution.Content);

            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);
            var questionResult = result.Questions.Single();

            // Assert
            Assert.NotNull(questionResult.Solution);
            Assert.Equal(questionResult.Solution.Content, exerciseDto.Questions.Single().Solution.Content);
        }

        [Fact]
        public async Task CreateExercise_Should_Create_ExerciseKeywords()
        {
            // Arrange
            var keyword = new Guid();
            var mockRepo = new Mock<IExerciseRepository>();
            var exerciseDto = new CreateExerciseRequest
            {
                Title = "Title1",
                Content = "Content1",
                CreatedByTeacherId = Guid.NewGuid(),
                ExerciseKeywords = { new CreateExerciseKeywordRequest { KeywordId = keyword} }
            };
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            expectedExercise.AddKeyword(keyword);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);
            var keywordResult = result.ExerciseKeywords.Single();

            // Assert
            Assert.NotEmpty(result.ExerciseKeywords);
            Assert.NotNull(keywordResult);
            Assert.Equal(keywordResult.KeywordId, exerciseDto.ExerciseKeywords.Single().KeywordId);
        }

        [Fact]
        public async Task CreateExercise_Should_Create_ExerciseSolution()
        {
            // Arrange
            var solution = new CreateExerciseSolutionRequest { Content = "Content", VideoUrl = "url" };
            var mockRepo = new Mock<IExerciseRepository>();
            var exerciseDto = new CreateExerciseRequest()
            {
                Title = "Title1",
                Content = "Content1",
                CreatedByTeacherId = Guid.NewGuid(),
                Solution = solution
            };
            var expectedExercise = new Exercise(exerciseDto.Title, exerciseDto.Content, exerciseDto.CreatedByTeacherId);
            expectedExercise.SetSolution(solution.Content, solution.VideoUrl);
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Exercise>())).ReturnsAsync(expectedExercise);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.CreateExerciseAsync(exerciseDto);

            // Assert
            Assert.NotNull(result.Solution);
            Assert.Equal(result.Solution.Content, exerciseDto.Solution.Content);
            Assert.Equal(result.Solution.VideoUrl, exerciseDto.Solution.VideoUrl);
        }
    }

    public class ReadExerciseServiceTests
    {
        private static readonly Guid Keyword1 = Guid.NewGuid();
        private static readonly Guid Keyword2 = Guid.NewGuid();
        private static readonly Guid Keyword3 = Guid.NewGuid();

        public static IEnumerable<object[]> KeywordSearchTestData()
        {
            var ex1 = new Exercise("Title1", "Content1", Guid.NewGuid());
            ex1.AddKeyword(Keyword1);

            var ex2 = new Exercise("Title2", "Content2", Guid.NewGuid());
            ex2.AddKeyword(Keyword2);

            var ex3 = new Exercise("Title3", "Content3", Guid.NewGuid());
            ex3.AddKeyword(Keyword3);

            yield return new object[]
            {
                new List<Guid> { Keyword1, Keyword2 },
                new List<Exercise> { ex1, ex2 }
            };

            yield return new object[]
            {
                new List<Guid> { Keyword3 },
                new List<Exercise> { ex3 }
            };

            yield return new object[]
            {
                new List<Guid>(),
                new List<Exercise>()
            };
        }

        [Theory]
        [MemberData(nameof(KeywordSearchTestData))]
        public async Task GetExerciseByExerciseKeywords_ShouldFilterCorrectly(List<Guid> keywordIds, List<Exercise> expected)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByKeywordsAsync(keywordIds))
                    .ReturnsAsync(expected);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExerciseByExerciseKeywords(keywordIds);

            // Assert
            Assert.Equal(expected.Count, result.Count());
            foreach (var exercise in expected)
            {
                Assert.Contains(result, r => r.Id == exercise.Id && r.Title == exercise.Title);
                Assert.Contains(result, r => r.ExerciseKeywords.Count() == exercise.ExerciseKeywords.Count());
            }
        }

        public static IEnumerable<object[]> TeacherExerciseTestData()
        {
            yield return new object[]
            {
                Guid.NewGuid(), //Teacher id
                new List<Exercise>
                {
                    new Exercise("Title1", "Content1", Guid.NewGuid()),
                    new Exercise("Title2", "Content2", Guid.NewGuid())
                }
            };
            yield return new object[]
            {
                Guid.NewGuid(), // teacher with no exercises
                new List<Exercise>()
            };
        }

        [Theory]
        [MemberData(nameof(TeacherExerciseTestData))]
        public async Task GetExercisesByTeacherIdAsync_ShouldReturnExpected(Guid teacherId, List<Exercise> repoResult)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByTeacherIdAsync(teacherId))
                    .ReturnsAsync(repoResult);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExercisesByTeacherIdAsync(teacherId);

            // Assert
            Assert.Equal(repoResult.Count, result.Count());
            foreach (var exercise in repoResult)
            {
                Assert.Contains(result, r => r.Id == exercise.Id && r.Title == exercise.Title);
            }
        }


        [Fact]
        public async Task GetExerciseByIdAsync_WhenExerciseExists_ReturnsExercise()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExerciseByIdAsync(exercise.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(exercise.Id, result.Id);
            Assert.Equal(exercise.Title, result.Title);
            Assert.Equal(exercise.Content, result.Content);
        }

        public static IEnumerable<object[]> ExerciseWithQuestionsTestData()
        {
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            exercise.AddQuestion("Question1", "Content1");

            yield return new object[]
            {
                exercise.Id,
                exercise
            };

            var exercise2 = new Exercise("Title2", "Content2", Guid.NewGuid());
            exercise2.AddQuestion("Question2", "Content2");
            exercise2.AddQuestion("Question3", "Content3");

            yield return new object[]
                {
                exercise2.Id,
                exercise2
            };

            var exercise3 = new Exercise("Title3", "Content3", Guid.NewGuid());
            yield return new object[]
            {
                exercise3.Id,
                exercise3
            };
        }

        [Theory]
        [MemberData(nameof(ExerciseWithQuestionsTestData))]
        public async Task GetExerciseByIdAsync_ShouldIncludeQuestions(Guid exerciseId, Exercise expected)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exerciseId))
                    .ReturnsAsync(expected);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.GetExerciseByIdAsync(exerciseId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.Content, result.Content);
            Assert.Equal(expected.Questions.Count(), result.Questions.Count());
            foreach (var question in expected.Questions)
            {
                Assert.Contains(result.Questions, q => q.Id == question.Id && q.Title == question.Title);
            }
        }

        public static IEnumerable<object[]> ExerciseWithSolutionTestData()
        {
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            exercise.SetSolution("Solution Content", "https://video.com");
            yield return new object[]
            {
                exercise.Id,
                exercise
            };
            var exercise2 = new Exercise("Title2", "Content2", Guid.NewGuid());
            yield return new object[]
            {
                exercise2.Id,
                exercise2
            };
        }

        [Theory]
        [MemberData(nameof(ExerciseWithSolutionTestData))]
        public async Task GetExerciseByIdAsync_ShouldIncludeSolution(Guid exerciseId, Exercise expected)
        {
            // Arrange
            var mockRepo = new Mock<IExerciseRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(exerciseId))
                    .ReturnsAsync(expected);
            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.GetExerciseByIdAsync(exerciseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.Content, result.Content);

            if (expected.Solution != null)
            {
                Assert.NotNull(result.Solution);
                Assert.Equal(expected.Solution.Content, result.Solution.Content);
                Assert.Equal(expected.Solution.VideoUrl, result.Solution.VideoUrl);
            }
            else
            {
                Assert.Null(result.Solution);
            }
        }
    }

    public class UpdateExerciseServiceTests
    {
        [Fact]
        public async Task UpdateExerciseAsync_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateExerciseRequest
            {
                Id = exercise.Id,
                Title = "Updated Title",
                Content = "Updated Content",
                RowVersion = exercise.RowVersion
            };

            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);

            var service = new ExerciseService(mockRepo.Object);

            // Act
            var result = await service.UpdateExerciseAsync(updateRequest);

            // Assert
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exercise>(), It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public async Task UpdateExerciseAsync_shouldUpdateExerciseProperties()
        {
            // Arrange
            var exercise = new Exercise("Title1", "Content1", Guid.NewGuid());
            var mockRepo = new Mock<IExerciseRepository>();
            var updateRequest = new UpdateExerciseRequest
            {
                Id = exercise.Id,
                Title = "Updated Title",
                Content = "Updated Content",
                RowVersion = exercise.RowVersion
            };
            mockRepo.Setup(r => r.GetByIdAsync(exercise.Id))
                    .ReturnsAsync(exercise);
            mockRepo.Setup(r => r.UpdateAsync(exercise, exercise.RowVersion))
                    .ReturnsAsync(exercise);
            var service = new ExerciseService(mockRepo.Object);
            // Act
            var result = await service.UpdateExerciseAsync(updateRequest);

            // Assert
            Assert.Equal(updateRequest.Title, exercise.Title);
            Assert.Equal(updateRequest.Content, exercise.Content);
        }
    }

}

