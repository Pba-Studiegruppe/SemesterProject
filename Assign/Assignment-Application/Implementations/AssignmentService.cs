
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

        public async Task<AssignmentExerciseDTO> AddExerciseAsync(
            Guid assignmentId,
            CreateAssignmentExerciseRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var assignment = await _repository.GetByIdAsync(assignmentId);
            if (assignment is null)
                throw new KeyNotFoundException($"Assignment with id '{assignmentId}' not found.");

            var snapshot = await _exerciseProvider.GetExerciseSnapshotAsync(request.ExerciseId);
            if (snapshot is null)
                throw new KeyNotFoundException(
                    $"Exercise with id '{request.ExerciseId}' not found.");

            // domain throws InvalidOperationException on duplicate
            var assignmentExercise = assignment.AddExerciseFromSnapshot(snapshot);

            await _repository.SaveChangesAsync();

            return ToDto(assignmentExercise);
        }

        public async Task RemoveExerciseAsync(
            Guid assignmentId,
            RemoveAssignmentExerciseRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var assignment = await _repository.GetByIdAsync(assignmentId);
            if (assignment is null)
                throw new KeyNotFoundException($"Assignment with id '{assignmentId}' not found.");

            // domain throws KeyNotFoundException if AE not in this assignment
            assignment.RemoveExercise(request.AssignmentExerciseId);

            await _repository.SaveChangesAsync();
        }

        private static AssignmentExerciseDTO ToDto(AssignmentExercise ae) =>
            new()
            {
                Id = ae.Id,
                AssignmentId = ae.AssignmentId,
                SourceExerciseId = ae.SourceExerciseId,
                Title = ae.Title,
                Content = ae.Content,
                Order = ae.Order,
                SnapshotTakenAt = ae.SnapshotTakenAt,
                TotalPoints = ae.TotalPoints,
                Questions = ae.Questions
                    .Select(q => new AssignmentQuestionDTO
                    {
                        Id = q.Id,
                        SourceQuestionId = q.SourceQuestionId,
                        Title = q.Title,
                        Content = q.Content,
                        Points = q.Points,
                        Order = q.Order
                    })
                    .ToList()
            };
    }
}
