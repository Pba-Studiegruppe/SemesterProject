using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercise_Application.DTO;
using Exercise_Domain.Entities;

namespace Exercise_Application.Interfaces.Repositories
{
    public interface IExerciseRepository
    {
        Task<Exercise> AddExerciseAsync(Exercise exercise);
        Task<IEnumerable<Exercise>> GetExercisesByKeywordsAsync(IEnumerable<Guid> keywordIds);
        Task<IEnumerable<Exercise>> GetExercisesByTeacherIdAsync(Guid teacherId);
        Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId);
        Task<Exercise> UpdateExerciseAsync(Exercise exercise, byte[] rowVersion);

        Task<Exercise> AddQuestionsASync(List<Question> Questions);
        Task<Exercise> AddExerciseKeywordsASync(List<ExerciseKeyword> ExerciseKeywords);
        Task<Exercise> SetExerciseSolution(ExerciseSolution ExerciseSolution);

        Task<Exercise> RemoveQuestionAsync(RemoveQuestionRequest dto);
        Task<Exercise> RemoveExerciseSolutionAsync(Guid exerciseId);
        Task<Exercise> RemoveQuestionSolutionAsync(Guid questionId);
        Task<Exercise> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto);
    }
}
