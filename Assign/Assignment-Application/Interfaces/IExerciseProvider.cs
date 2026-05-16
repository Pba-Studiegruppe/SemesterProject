using Assignment_Domain.SnapShots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Application.Interfaces
{
    public interface IExerciseProvider
    {
        Task<ExerciseSnapshotInput?> GetExerciseSnapshotAsync(Guid exerciseId);
    }
}
