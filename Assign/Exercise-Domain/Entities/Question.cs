using Exercise_Domain.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Exercise_Domain.Entities
{
    public class Question : Entity
    {
        public Guid ExerciseId { get; private set; }

        public string Title { get; private set; }

        public string Content { get; private set; }
        public QuestionSolution? Solution { get; private set; }


        private Question() : base(Guid.NewGuid()) { }

        /// <summary>
        /// Represents a question associated with an exercise, containing a title, content, and an optional solution. Each question is linked to a specific exercise through the ExerciseId property and is created with a unique identifier.
        /// </summary>
        /// <param name="exerciseId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public Question(string title, string? content) : base(Guid.NewGuid())
        {
            Title = title;
            if (content != null)
            {
                Content = content;
            }
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
