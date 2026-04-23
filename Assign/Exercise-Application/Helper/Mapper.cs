using Exercise_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Helper
{
    internal class Mapper
    {
        public static ExerciseDTO MapToDTO(Exercise exercise)
        {
            return new ExerciseDTO
            {
                Id = exercise.Id,
                Title = exercise.Title,
                Content = exercise.Content,
                CreatedAt = exercise.CreatedAt,
                CreatedByTeacherId = exercise.CreatedByTeacherId,
                Questions = exercise.Questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content,
                    Solution = q.Solution != null ? new QuestionSolutionDTO
                    {
                        Id = q.Solution.Id,
                        Content = q.Solution.Content
                    } : null
                }).ToList(),
                ExerciseKeywords = exercise.ExerciseKeywords.Select(ek => new ExerciseKeywordDTO
                {
                    ExerciseId = ek.ExerciseId,
                    KeywordId = ek.KeywordId
                }).ToList(),
                Solution = exercise.Solution != null ? new ExerciseSolutionDTO
                {
                    Id = exercise.Solution.Id,
                    Content = exercise.Solution.Content,
                    VideoUrl = exercise.Solution.VideoUrl
                } : null
            };
        }
    }
}
