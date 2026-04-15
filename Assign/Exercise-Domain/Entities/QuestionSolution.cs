using Exercise_Domain.shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exercise_Domain.Entities
{
    public class QuestionSolution: Entity
    {
        public Guid QuestionId { get; private set; }
        public string Content { get; private set; }

        internal QuestionSolution() : base(Guid.NewGuid()) { }

        public QuestionSolution(Guid questionId, string content) : base(Guid.NewGuid())
        {
            Id = Guid.NewGuid();
            QuestionId = questionId;
            Content = content;
        }

        public void Update(string content)
        {
            Content = content;
        }
    }
}