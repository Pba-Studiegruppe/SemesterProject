using Exercise_Domain.shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exercise_Domain.Entities
{

    public class ExerciseSolution : Entity
    {
        public Guid ExerciseId { get; private set; }
        public string Content { get; private set; }
        public string? VideoUrl { get; private set; }

        private ExerciseSolution() : base(Guid.NewGuid()) { }

        public ExerciseSolution(Guid exerciseId, string content, string? videoUrl) : base(Guid.NewGuid())
        {
            Id = Guid.NewGuid();
            ExerciseId = exerciseId;
            Content = content;
            VideoUrl = videoUrl;
        }

        public void Update(string content, string? videoUrl)
        {
            Content = content;
            VideoUrl = videoUrl;
        }
    }
}