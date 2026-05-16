using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace Exercise_Application.Projections
    {
        public sealed record ExerciseSnapshotProjection(
            Guid Id,
            string Title,
            string Content,
            IReadOnlyList<QuestionSnapshotProjection> Questions);

        public sealed record QuestionSnapshotProjection(
            Guid Id,
            string Title,
            string Content);
    }

