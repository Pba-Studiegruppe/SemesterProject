using Exercise_Application.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    /// <summary>
    /// Projection of an Exercise including its solutions, for any teacher
    /// browsing the exercise catalog to review an exercise before adding it
    /// to one of their assignments.
    /// </summary>
    public interface IExerciseReviewQueryService
    {
        Task<ExerciseReviewProjection?> GetForReviewAsync(Guid exerciseId);
    }
}
