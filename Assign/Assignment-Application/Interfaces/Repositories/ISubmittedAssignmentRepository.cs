using Assignment_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces.Repositories
{
    public interface ISubmittedAssignmentRepository
    {
        /// <summary>
        /// Loads a submission with its SubmittedExercises and SubmittedQuestions
        /// eager-loaded. Implementations should also include the parent Assignment
        /// with its AssignmentExercises and AssignmentQuestions, since the
        /// service needs the source data to populate denormalized DTO fields.
        /// </summary>
        Task<SubmittedAssignment?> GetByIdAsync(Guid id);

        /// <summary>
        /// Returns every submission for an assignment. The parent Assignment is
        /// not eager-loaded here — the caller already has it in hand when
        /// listing submissions for a given assignment.
        /// </summary>
        Task<IEnumerable<SubmittedAssignment>> GetByAssignmentIdAsync(Guid assignmentId);

        Task CreateAsync(SubmittedAssignment submittedAssignment);

        Task SaveChangesAsync();
    }
}
