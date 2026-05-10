
using Assignment_Application.DTO;
using Assignment_Application.Interfaces;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repository;
        private readonly IExerciseProvider _exerciseProvider;

        public AssignmentService(IAssignmentRepository repository, IExerciseProvider exerciseProvider)
        {
            _repository = repository;
            _exerciseProvider = exerciseProvider;
        }

        public async Task<AssignmentDTO> CreateAssignmentAsync(CreateAssignmentRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title is required", nameof(request.Title));

            var assignment = new Assignment(request.Title, request.Description);

            // persist
            await _repository.CreateAsync(assignment);
            await _repository.SaveChangesAsync();

            return new AssignmentDTO
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                TotalPoints = assignment.TotalPoints,
                CreatedAt = assignment.CreatedAt,
            };
        }

        public async Task<AssignmentDTO> GetAssignmentAsync(Guid id)
        {
            var assignment = await _repository.GetByIdAsync(id);
            if (assignment is null) throw new KeyNotFoundException($"Assignment with id '{id}' not found.");

            return new AssignmentDTO
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                TotalPoints = assignment.TotalPoints,
                CreatedAt = assignment.CreatedAt,

            };
        }

        public async Task<AssignmentDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Title is required", nameof(request.Title));

            var assignment = await _repository.GetByIdAsync(assignmentId);
            if (assignment is null) throw new KeyNotFoundException($"Assignment with id '{assignmentId}' not found.");

            assignment.UpdateTitle(request.Title);
            assignment.UpdateDescription(request.Description);

            await _repository.SaveChangesAsync();

            return new AssignmentDTO
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description
            };
        }

        public async Task<AssignmentExerciseDTO> AddExerciseAsync(Guid assignmentId, CreateAssignmentExerciseRequest request)
        {
            throw new NotImplementedException();
            //if (request is null) throw new ArgumentNullException(nameof(request));

            //var assignment = await _repository.GetByIdAsync(assignmentId);
            //if (assignment is null) throw new KeyNotFoundException($"Assignment with id '{assignmentId}' not found.");

            //// domain may throw InvalidOperationException if duplicate
            //assignment.AddExercise(request.ExerciseId);

            //await _repository.SaveChangesAsync();

            //return new AssignmentExerciseDTO
            //{
            //    ExerciseId = request.ExerciseId
            //};
        }

        public async Task RemoveExerciseAsync(Guid assignmentId, RemoveAssignmentExerciseRequest request)
        {
            throw new NotImplementedException();
            //if (request is null) throw new ArgumentNullException(nameof(request));

            //var assignment = await _repository.GetByIdAsync(assignmentId);
            //if (assignment is null) throw new KeyNotFoundException($"Assignment with id '{assignmentId}' not found.");

            //// domain should throw if exercise not present
            //assignment.RemoveExercise(request.ExerciseId);

            //await _repository.SaveChangesAsync();
        }
    }
}
