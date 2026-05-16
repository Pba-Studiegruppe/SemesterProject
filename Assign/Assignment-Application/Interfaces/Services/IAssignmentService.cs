using Assignment_Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Services
{
    public interface IAssignmentService
    {
        Task<AssignmentDTO> GetAssignmentAsync(Guid id);
        Task<AssignmentDTO> CreateAssignmentAsync(CreateAssignmentRequest request);
        Task<AssignmentDTO> UpdateAssignmentAsync(Guid assignmentId, UpdateAssignmentRequest request);
        Task<AssignmentExerciseDTO> AddExerciseAsync(Guid assignmentId, CreateAssignmentExerciseRequest request);
        Task RemoveExerciseAsync(Guid assignmentId, RemoveAssignmentExerciseRequest request);
        Task SetQuestionPointsAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId, SetQuestionPointsRequest request);
        Task RemoveQuestionAsync(Guid assignmentId, Guid assignmentExerciseId, Guid questionId);
    }
}
