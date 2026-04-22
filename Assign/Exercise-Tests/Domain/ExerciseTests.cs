using Exercise_Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Domain
{
    public class ExerciseTests
    {
        [Theory]
        [InlineData("Math", "Solve equation")]
        [InlineData("Physics", "Calculate force")]
        [InlineData("Chemistry", "Balance reaction")]
        public void CreateExercise_ShouldSetTitleAndContent(
        string title,
        string content)
        {
            // Arrange
            var teacherId = Guid.NewGuid();

            // Act
            var exercise = new Exercise(title, content, teacherId);

            // Assert
            Assert.Equal(title, exercise.Title);
            Assert.Equal(content, exercise.Content);
        }

        [Fact]
        public void UpdateContent_ShouldChangeExerciseContent()
        {
            // Arrange
            var exercise = new Exercise("Test", "Old Content", Guid.NewGuid());
            var newContent = "New Content";
            // Act
            exercise.UpdateContent(newContent);
            // Assert
            Assert.Equal(newContent, exercise.Content);
        }

        [Fact]
        public void UpdateTitle_ShouldChangeExerciseTitle()
        {
            // Arrange
            var exercise = new Exercise("Old Title", "Test", Guid.NewGuid());
            var newTitle = "New Title";
            // Act
            exercise.UpdateTitle(newTitle);
            // Assert
            Assert.Equal(newTitle, exercise.Title);
        }

        [Fact]
        public void SetCreatedAt_Should_SetCreatedAt()
        {
            // Arrange
            var exercise = new Exercise("Title","Content",Guid.NewGuid());
            var createdAt = DateTime.UtcNow;

            // Act
            exercise.SetCreatedAt(createdAt);

            //Assert
            Assert.Equal(createdAt, exercise.CreatedAt);

        }
    }

    public class ExerciseTests_KeywordTests
    {

        [Fact]
        public void AddKeyword_ShouldAddKeywordToExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var keywordId = Guid.NewGuid();
            // Act
            exercise.AddKeyword(keywordId);
            // Assert
            Assert.Single(exercise.ExerciseKeywords);
            Assert.Equal(keywordId, exercise.ExerciseKeywords.First().KeywordId);
        }

        [Fact]
        public void RemoveKeyword_ShouldRemoveKeywordFromExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var keywordId = Guid.NewGuid();
            exercise.AddKeyword(keywordId);
            // Act
            exercise.RemoveKeyword(keywordId);
            // Assert
            Assert.Empty(exercise.ExerciseKeywords);
        }
    }

    public class ExerciseTests_QuestionTests
    {


        [Fact]
        public void AddQuestion_ShouldAddQuestionToExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var title = "Sample Question";
            var content = "What is 2 + 2?";
            // Act
            exercise.AddQuestion(title, content, null);
            // Assert
            Assert.Single(exercise.Questions);
            Assert.Equal(title, exercise.Questions.First().Title);
            Assert.Equal(content, exercise.Questions.First().Content);
        }

        [Fact]
        public void AddQuestion_ShouldAddQuestionToExercise_AndQuestionSolutionToQuestion()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var title = "Sample Question";
            var content = "What is 2 + 2?";
            var solutionContent = "Sample Solution";
            // Act
            exercise.AddQuestion(title, content, solutionContent);
            // Assert
            Assert.Single(exercise.Questions);
            Assert.Equal(title, exercise.Questions.First().Title);
            Assert.Equal(content, exercise.Questions.First().Content);
            Assert.NotNull(exercise.Questions.First().Solution);
            Assert.Equal(solutionContent, exercise.Questions.First().Solution!.Content);
        }

        [Fact]
        public void RemoveQuestion_ShouldRemoveQuestionFromExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var title = "Sample Question";
            var content = "What is 2 + 2?";
            exercise.AddQuestion(title, content, null);
            var questionId = exercise.Questions.First().Id;
            // Act
            exercise.RemoveQuestion(questionId);
            // Assert
            Assert.Empty(exercise.Questions);
        }

        [Fact]
        public void RemoveQuestion_ShouldThrow_WhenQuestionNotFound()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var nonExistentQuestionId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                exercise.RemoveQuestion(nonExistentQuestionId));
        }

        [Fact]
        public void UpdateQuestion_ShouldUpdateQuestion()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var title = "Sample Question";
            var content = "What is 2 + 2?";
            exercise.AddQuestion(title, content, null);
            var questionId = exercise.Questions.First().Id;
            var newTitle = "Updated Question";
            var newContent = "What is 3 + 3?";
            // Act
            exercise.UpdateQuestion(questionId, newTitle, newContent);
            // Assert
            Assert.Equal(newTitle, exercise.Questions.First().Title);
            Assert.Equal(newContent, exercise.Questions.First().Content);
        }

        [Fact]
        public void UpdateQuestion_ShouldThrow_WhenQuestionNotFound()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var nonExistentQuestionId = Guid.NewGuid();
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                exercise.UpdateQuestion(nonExistentQuestionId, "Title", "Content"));
        }
    }


    public class ExerciseTests_SolutionTests
    {
        [Fact]
        public void SetSolution_ShouldInitializeExerciseSolution()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var solutionContent = "This is the solution.";
            var videoUrl = "https://video.com";
            // Act
            exercise.SetSolution(solutionContent, videoUrl);
            // Assert
            Assert.NotNull(exercise.Solution);
            Assert.Equal(exercise.Id, exercise.Solution.ExerciseId);
            Assert.Equal(solutionContent, exercise.Solution.Content);
            Assert.Equal(videoUrl, exercise.Solution.VideoUrl);
        }

        [Fact]
        public void RemoveSolution_ShouldRemoveSolutionFromExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var solutionContent = "This is the solution.";
            var videoUrl = "https://video.com";
            exercise.SetSolution(solutionContent, videoUrl);
            // Act
            exercise.RemoveSolution();
            // Assert
            Assert.Null(exercise.Solution);
        }
    }





    public class ExerciseMemberDataTests
    {
        public static IEnumerable<object[]> QuestionData =>
            new List<object[]>
            {
                    new object[] { "Q1", "Content 1" },
                    new object[] { "Q2", "Content 2" },
                    new object[] { "Q3", "Content 3" }
            };

        [Theory]
        [MemberData(nameof(QuestionData))]
        public void AddQuestion_ShouldIncreaseCount(
            string title,
            string content)
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());

            // Act
            exercise.AddQuestion(title, content, null);

            // Assert
            Assert.Single(exercise.Questions);
        }
    }

    public class ExerciseTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 3 };
            yield return new object[] { 5 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ExerciseClassDataTests
    {
        [Theory]
        [ClassData(typeof(ExerciseTestData))]
        public void AddMultipleQuestions_ShouldMatchCount(int numberOfQuestions)
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());

            // Act
            for (int i = 0; i < numberOfQuestions; i++)
            {
                exercise.AddQuestion($"Q{i}", "Content", null);
            }

            // Assert
            Assert.Equal(numberOfQuestions, exercise.Questions.Count);
        }
    }
}
