using Exercise_Application.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    /// <summary>
    /// Teacher-authorized projection of an Exercise that includes solutions.
    /// Implementation is responsible for verifying the requester is permitted
    /// to see solution data.
    /// </summary>
    public interface IExerciseEvaluationQueryService
    {
        Task<ExerciseEvaluationProjection?> GetForEvaluationAsync(
            Guid exerciseId,
            Guid teacherId);
    }
}
