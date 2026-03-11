using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise_Domain.Entities
{
    public class Keyword
    {
        public Guid Id { get; private set; }

        public string KeywordName { get; private set; }

        public KeywordType KeywordType { get; private set; }

        private Keyword() { }

        public Keyword(string keywordName, KeywordType keywordType)
        {
            Id = Guid.NewGuid();
            KeywordName = keywordName;
            KeywordType = keywordType;
        }
    }

    public enum KeywordType
    {
        SchoolSubject,
        ExerciseType,
        SubjectArea
    }
}
