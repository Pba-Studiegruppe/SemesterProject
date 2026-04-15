using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Tests.Domain
{
    public class KeywordTests
    {
        [Fact]
        public void Constructor_Should_Set_Properties_Correctly()
        {
            // Arrange
            var name = "Sample Keyword";
            KeywordType keywordType = KeywordType.SchoolSubject;
            // Act
            var keyword = new Keyword(name, keywordType);
            // Assert
            Assert.NotEqual(Guid.Empty, keyword.Id);
            Assert.Equal(name, keyword.KeywordName);
            Assert.Equal(keywordType, keyword.KeywordType);
        }
    }
}
