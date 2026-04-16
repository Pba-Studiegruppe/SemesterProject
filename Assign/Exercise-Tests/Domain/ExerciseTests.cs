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
        public void AddQuestion_ShouldAddQuestionToExercise()
        {
            // Arrange
            var exercise = new Exercise("Test", "Test", Guid.NewGuid());
            var title = "Sample Question";
            var content = "What is 2 + 2?";
            // Act
            exercise.AddQuestion(title, content);
            // Assert
            Assert.Single(exercise.Questions);
            Assert.Equal(title, exercise.Questions.First().Title);
            Assert.Equal(content, exercise.Questions.First().Content);
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
                exercise.AddQuestion(title, content);

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
                    exercise.AddQuestion($"Q{i}", "Content");
                }

                // Assert
                Assert.Equal(numberOfQuestions, exercise.Questions.Count);
            }
        }

    }
}
