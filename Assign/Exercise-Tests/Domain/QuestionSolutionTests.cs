using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Domain
{
    public class QuestionSolutionTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var content = "This is a solution to the question.";
            // Act
            var solution = new QuestionSolution(questionId, content);
            // Assert
            Assert.NotEqual(Guid.Empty, solution.Id);
            Assert.Equal(questionId, solution.QuestionId);
            Assert.Equal(content, solution.Content);
        }

        [Fact]
        public void Update_Should_Modify_Content()
        {
            // Arrange
            var questionId = Guid.NewGuid();
            var solution = new QuestionSolution(questionId, "Initial content");
            var newContent = "Updated content";
            // Act
            solution.Update(newContent);
            // Assert
            Assert.Equal(newContent, solution.Content);
        }
    }
}
