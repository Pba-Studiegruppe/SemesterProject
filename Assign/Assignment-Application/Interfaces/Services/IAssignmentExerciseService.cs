using Assignment_Application.DTO;

namespace Assignment_Application.Interfaces.Services
{
    public interface IAssignmentExerciseService
    {
        Task<AssignmentExerciseDTO> AddAssignmentExerciseAsync(Guid assignmentId, CreateAssignmentExerciseRequest request);
        Task<AssignmentExerciseDTO> RemoveAssignmentExerciseAsync(Guid assignmentId, RemoveAssignmentExerciseRequest request);

    }
}