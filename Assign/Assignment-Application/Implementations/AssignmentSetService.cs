using Assignment_Api;
using Assignment_Application.DTO;
using Assignment_Application.Interfaces.Repositories;
using Assignment_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_Application.Implementations
{
    public class AssignmentSetService: IAssignmentSetService
    {
        private readonly IAssignmentSetRepository _repository;

        public AssignmentSetService(IAssignmentSetRepository repository)
        {
            _repository = repository;
        }

        public async Task<AssignmentSetDTO> CreateAssignmentSetAsync(CreateAssignmentSetRequest request)
        {
            if (request.CourseId == Guid.Empty)
                throw new ArgumentException("CourseId is invalid.", nameof(request.CourseId));

            var assignmentSet = new AssignmentSet(
                request.CourseId,
                request.Title,
                request.Description);

            await _repository.CreateAsync(assignmentSet);
            await _repository.SaveChangesAsync();

            return ToDto(assignmentSet);
        }

        public async Task<AssignmentSetDTO> GetAssignmentSetsAsync(Guid id)
        {
            var assignmentSet = await _repository.GetByIdAsync(id);
            if (assignmentSet is null)
                throw new KeyNotFoundException("AssignmentSet not found.");

            return ToDto(assignmentSet);
        }

        public async Task<IEnumerable<AssignmentSetDTO>> GetAssignmentSetsByCourseIdAsync(Guid courseId)
        {
            var sets = await _repository.GetByCourseIdAsync(courseId);
            return sets.Select(ToDto);
        }

        public async Task<AssignmentSetDTO> UpdateAssignmentSetAsync(Guid assignmentSetId, UpdateAssignmentSetRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.", nameof(request.Title));

            var assignmentSet = await _repository.GetByIdAsync(assignmentSetId);
            if (assignmentSet is null)
                throw new KeyNotFoundException("AssignmentSet not found.");

            // update properties on the entity
            assignmentSet.Title = request.Title;
            assignmentSet.Description = request.Description;

            await _repository.SaveChangesAsync();

            return ToDto(assignmentSet);
        }

        private static AssignmentSetDTO ToDto(AssignmentSet model)
        {
            return new AssignmentSetDTO
            {
                Id = model.Id,
                CourseId = model.CourseId,
                Title = model.Title,
                Description = model.Description,
                Assignments = (model.Assignments ?? new List<Assignment>())
                    .Select(a => new AssignmentDTO
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Description = a.Description,
                        Exercises = (a.AssignmentExercises ?? new List<AssignmentExercise>())
                            .Select(e => new AssignmentExerciseDTO())
                            .ToList()
                    })
                    .ToList()
            };
        }
    }
}
