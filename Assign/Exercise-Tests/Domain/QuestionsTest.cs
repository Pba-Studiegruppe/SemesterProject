using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Domain
{
    public class QuestionsTest
    {
        [Fact]
        public void CreateQuestion_ShouldInitializeProperties()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var title = "Sample Question";
            var content = "What is the capital of France?";
            // Act
            var question = new Exercise_Domain.Entities.Question(exerciseId, title, content);
            // Assert
            Assert.NotEqual(Guid.Empty, question.Id);
            Assert.Equal(exerciseId, question.ExerciseId);
            Assert.Equal(title, question.Title);
            Assert.Equal(content, question.Content);
            Assert.Null(question.Solution);
        }

        [Fact]
        public void Update_ShouldChangeTitleAndContent()
        {
            // Arrange
            var question = new Exercise_Domain.Entities.Question(Guid.NewGuid(), "Old Title", "Old Content");
            var newTitle = "New Title";
            var newContent = "New Content";
            // Act
            question.Update(newTitle, newContent);
            // Assert
            Assert.Equal(newTitle, question.Title);
            Assert.Equal(newContent, question.Content);
        }

        [Fact]
        public void SetSolution_ShouldInitializeSolution()
        {
            // Arrange
            var question = new Exercise_Domain.Entities.Question(Guid.NewGuid(), "Sample Question", "Sample Content");
            var solutionContent = "The capital of France is Paris.";
            // Act
            question.SetSolution(solutionContent);
            // Assert
            Assert.NotNull(question.Solution);
            Assert.Equal(question.Id, question.Solution!.QuestionId);
            Assert.Equal(solutionContent, question.Solution.Content);
        }
    }
}
