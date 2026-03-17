using System;

namespace Exercise_Domain.Entities
{
    public class QuestionSolution
    {
        public Guid Id { get; private set; }

        public Guid QuestionId { get; private set; }

        public string Content { get; private set; }


        private QuestionSolution() { }

        public QuestionSolution(Guid questionId, string content)
        {
            Id = Guid.NewGuid();
            questionId = QuestionId;
            Content = content;
        }
    }
}