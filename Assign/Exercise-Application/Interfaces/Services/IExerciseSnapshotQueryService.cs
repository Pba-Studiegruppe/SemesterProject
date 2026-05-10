using Exercise_Application.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise_Application.Interfaces.Services
{
    /// <summary>
    /// Read-only projection of an Exercise without solutions.
    /// Used by Assignment when taking a snapshot — and by anything else that
    /// must not see solution data.
    /// </summary>
    public interface IExerciseSnapshotQueryService
    {
        Task<ExerciseSnapshotProjection?> GetForSnapshotAsync(Guid exerciseId);
    }
}