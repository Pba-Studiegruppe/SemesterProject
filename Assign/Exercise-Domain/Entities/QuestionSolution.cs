using System;
using System.ComponentModel.DataAnnotations;

namespace Exercise_Domain.Entities
{
    public class QuestionSolution
    {
        public Guid Id { get; private set; }
        public Guid QuestionId { get; private set; }
        public string Content { get; private set; }
        [Timestamp] public byte[] RowVersion { get; private set; } = [];


        private QuestionSolution() { }

        public QuestionSolution(Guid questionId, string content)
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