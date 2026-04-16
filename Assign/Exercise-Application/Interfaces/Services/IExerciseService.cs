using Exercise_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    public interface IExerciseService
    {
        Task<ExerciseDTO> CreateExerciseAsync(CreateExerciseRequest dto);
        Task<IEnumerable<ExerciseDTO?>> GetExerciseByExerciseKeywords(List<Guid> keywordIds);
        Task<IEnumerable<ExerciseDTO?>> GetExercisesByTeacherIdAsync(Guid teacherId);
        Task<ExerciseDTO?> GetExerciseByIdAsync(Guid id);
        Task<ExerciseDTO?> UpdateExerciseAsync(UpdateExerciseRequest dto);

        Task<ExerciseDTO> AddQuestionsAsync(List<QuestionDTO> Questions);
        Task<ExerciseDTO> AddExerciseKeywordsAsync(List<ExerciseKeywordDTO> ExerciseKeywords);
        Task<ExerciseDTO> SetExerciseSolution(ExerciseSolutionDTO ExerciseSolution);


        Task<ExerciseDTO> UpdateQuestionAsync(UpdateQuestionRequest dto);
        Task<ExerciseDTO> UpdateExerciseSolutionAsync(UpdateExerciseSolutionRequest dto);
        Task<ExerciseDTO> UpdateQuestionSolutionAsync(UpdateQuestionSolutionRequest dto);

        Task<ExerciseDTO> RemoveQuestionAsync(RemoveQuestionRequest dto);
        Task<ExerciseDTO> RemoveExerciseSolutionAsync(Guid exerciseId);
        Task<ExerciseDTO> RemoveQuestionSolutionAsync(Guid questionId);
        Task<ExerciseDTO> RemoveExerciseKeywordAsync(RemoveExerciseKeywordRequest dto);

    }
}
