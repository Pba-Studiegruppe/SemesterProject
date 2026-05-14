using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_Domain.SnapShots
{
    public sealed record ExerciseSnapshotInput(
        Guid SourceExerciseId,
        string Title,
        string Content,
        IReadOnlyList<QuestionSnapshotInput> Questions);

    public sealed record QuestionSnapshotInput(
        Guid SourceQuestionId,
        string Title,
        string Content);
}
