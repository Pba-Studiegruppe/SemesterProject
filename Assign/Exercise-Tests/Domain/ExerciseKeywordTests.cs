using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Domain
{
    public class ExerciseKeywordTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var exerciseId = Guid.NewGuid();
            var keywordId = Guid.NewGuid();
            
            // Act
            var exerciseKeyword = new ExerciseKeyword(exerciseId, keywordId);
            
            // Assert
            Assert.Equal(exerciseId, exerciseKeyword.ExerciseId);
            Assert.Equal(keywordId, exerciseKeyword.KeywordId);

        }
    }
}
