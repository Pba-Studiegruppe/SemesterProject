using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Exercise_Domain.Entities
{
    public class Question
    {
        public Guid Id { get; private set; }

        public Guid ExerciseId { get; private set; }

        public string Title { get; private set; }

        public string Content { get; private set; }
        public QuestionSolution? Solution { get; private set; }
        [Timestamp] public byte[] RowVersion { get; private set; } = [];


        private Question() { }

        public Question(Guid exerciseId, string title, string content)
        {
            Id = Guid.NewGuid();
            ExerciseId = exerciseId;
            Title = title;
            Content = content;
        }

        public void Update(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public void SetSolution(string content)
        {
            Solution = new QuestionSolution(this.Id, content);
        }
    }
}
