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
    public class QuestionSolutionService : IQuestionSolutionService
    {
        private readonly IQuestionSolutionRepository _repository;

        public QuestionSolutionService(IQuestionSolutionRepository repository)
        {
            _repository = repository;
        }

        public async Task<QuestionSolutionDTO> CreateQuestionSolutionAsync(CreateQuestionSolutionRequest dto)
        {
            try
            {
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The CreateQuestionSolutionRequest cannot be null."); }
                if (dto.QuestionId == Guid.Empty) { throw new ArgumentException("The QuestionId cannot be empty.", nameof(dto.QuestionId)); }
                if (string.IsNullOrWhiteSpace(dto.Content)) { throw new ArgumentException("The Content cannot be null or whitespace.", nameof(dto.Content)); }


                var questionSolution = new QuestionSolution(dto.QuestionId, dto.Content);
                var createdSolution = await _repository.AddQuestionSolutionAsync(questionSolution);

                if (createdSolution == null) {throw new Exception("Failed to create the question solution."); }

                return new QuestionSolutionDTO
                {
                    Id = createdSolution.Id,
                    QuestionId = createdSolution.QuestionId,
                    Content = createdSolution.Content
                };

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the question solution.", ex);
            }
        }

        public async Task<QuestionSolutionDTO?> GetQuestionSolutionByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty) { throw new ArgumentException("The Id cannot be empty.", nameof(id)); }

                var solution = await _repository.GetQuestionSolutionByIdAsync(id);

                if (solution == null) { return null; }
                return new QuestionSolutionDTO
                {
                    Id = solution.Id,
                    QuestionId = solution.QuestionId,
                    Content = solution.Content
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the question solution by Id.", ex);
            }
        }

        public async Task<QuestionSolutionDTO?> GetQuestionSolutionByQuestionIdAsync(Guid questionId)
        {
            try
            {
                if (questionId == Guid.Empty) { throw new ArgumentException("The QuestionId cannot be empty.", nameof(questionId)); }

                var solution = await _repository.GetQuestionSolutionByQuestionIdAsync(questionId);
                if (solution == null) { return null; }

                return new QuestionSolutionDTO
                {
                    Id = solution.Id,
                    QuestionId = solution.QuestionId,
                    Content = solution.Content
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the question solution by QuestionId.", ex);
            }
        }

        public async Task<QuestionSolutionDTO> UpdateQuestionSolutionAsync(UpdateQuestionSolutionRequest dto)
        {
            try
            {
                if (dto.Id == Guid.Empty) { throw new ArgumentException("The Id cannot be empty.", nameof(dto.Id)); }
                if (dto == null) { throw new ArgumentNullException(nameof(dto), "The UpdateQuestionSolutionRequest cannot be null."); }
                if (string.IsNullOrWhiteSpace(dto.Content)) { throw new ArgumentException("The Content cannot be null or whitespace.", nameof(dto.Content)); }

                var solutionToUpdate = await _repository.GetQuestionSolutionByIdAsync(dto.Id);

                if (solutionToUpdate == null) { throw new Exception("The question solution to update was not found."); }

                solutionToUpdate.Update(dto.Content);

                var updatedSolution = await _repository.UpdateQuestionSolutionAsync(solutionToUpdate, solutionToUpdate.RowVersion);

                return (new QuestionSolutionDTO
                {
                    Id = updatedSolution.Id,
                    QuestionId = updatedSolution.QuestionId,
                    Content = updatedSolution.Content
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the question solution.", ex);
            }
        }
    }
}
