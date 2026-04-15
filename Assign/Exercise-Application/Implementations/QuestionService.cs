using Exercise_Application.DTO;
using Exercise_Application.Interfaces.Repositories;
using Exercise_Application.Interfaces.Services;
using Exercise_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionDTO> CreateQuestionAsync(CreateQuestionRequest dto)
        {
            try
            {
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The CreateQuestionRequest cannot be null."); }
                if (dto.ExerciseId == Guid.Empty) { throw new ArgumentException("The ExerciseId cannot be empty.", nameof(dto.ExerciseId)); }
                if (string.IsNullOrWhiteSpace(dto.Title)) { throw new ArgumentException("The Title cannot be null or whitespace.", nameof(dto.Title)); }
                if (string.IsNullOrWhiteSpace(dto.Content)) { throw new ArgumentException("The Content cannot be null or whitespace.", nameof(dto.Content)); }

                var question = new Question(dto.ExerciseId, dto.Title, dto.Content);
                var createdQuestion = await _questionRepository.AddQuestionAsync(question);
                return new QuestionDTO
                {
                    Id = createdQuestion.Id,
                    ExerciseId = createdQuestion.ExerciseId,
                    Title = createdQuestion.Title,
                    Content = createdQuestion.Content
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the question.", ex);
            }

        }

        public async Task<QuestionDTO?> GetQuestionByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty) { throw new ArgumentException("The Id cannot be empty.", nameof(id)); }

                var question = await _questionRepository.GetQuestionByIdAsync(id);
                if (question == null) { return null; }
                return new QuestionDTO
                {
                    Id = question.Id,
                    ExerciseId = question.ExerciseId,
                    Title = question.Title,
                    Content = question.Content
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the question.", ex);
            }
        }

        public async Task<IEnumerable<QuestionDTO>> GetQuestionsByExerciseIdAsync(Guid exerciseId)
        {
            try
            {
                if (exerciseId == Guid.Empty) { throw new ArgumentException("The ExerciseId cannot be empty.", nameof(exerciseId)); }

                var questions = await _questionRepository.GetQuestionsByExerciseIdAsync(exerciseId);
                return questions.Select(q => new QuestionDTO
                {
                    Id = q.Id,
                    ExerciseId = q.ExerciseId,
                    Title = q.Title,
                    Content = q.Content
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving questions by exercise ID.", ex);
            }
        }

        public async Task<QuestionDTO> UpdateQuestionAsync(Guid id, UpdateQuestionRequest dto, byte[] rowVersion)
        {
            try
            {
                if (id == Guid.Empty) { throw new ArgumentException("The Id cannot be empty.", nameof(id)); }
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The UpdateQuestionRequest cannot be null."); }
                if (string.IsNullOrWhiteSpace(dto.Title)) { throw new ArgumentException("The Title cannot be null or whitespace.", nameof(dto.Title)); }
                if (string.IsNullOrWhiteSpace(dto.Content)) { throw new ArgumentException("The Content cannot be null or whitespace.", nameof(dto.Content)); }

                var existingQuestion = await _questionRepository.GetQuestionByIdAsync(id);
                if (existingQuestion == null) { throw new KeyNotFoundException($"No question found with ID {id}."); }

                existingQuestion.Update(dto.Title, dto.Content);

                var updatedQuestion = await _questionRepository.UpdateQuestionAsync(existingQuestion, existingQuestion.RowVersion);
                return new QuestionDTO
                {
                    Id = updatedQuestion.Id,
                    ExerciseId = updatedQuestion.ExerciseId,
                    Title = updatedQuestion.Title,
                    Content = updatedQuestion.Content
                };

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the question.", ex);
            }
        }
    }
}
