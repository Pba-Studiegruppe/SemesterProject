using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;

namespace Assignment_Infrastructure.DataAccess.Fakes
{
    /// <summary>
    /// In-memory fake repository for SubmittedAssignment. Use this while the real
    /// database isn't available.
    ///
    /// Takes IAssignmentRepository in its constructor so it can register a demo
    /// Assignment with the assignment fake during seeding. This means the
    /// "list submissions by assignment" endpoint actually works end-to-end —
    /// the service can look up the parent assignment and produce a fully-
    /// populated DTO with denormalized source fields.
    ///
    /// Pre-seeded with three submissions in different statuses (Pending,
    /// InProgress, Completed) against the same demo assignment so the
    /// teacher's evaluation UI has something realistic to render.
    /// </summary>
    public class FakeSubmittedAssignmentRepository : ISubmittedAssignmentRepository
    {
        // Stable demo assignment ID exposed for tests / API exploration.
        public static readonly Guid DemoAssignmentId =
            new("33333333-0000-0000-0000-000000000001");

        private readonly IAssignmentRepository _assignmentRepository;
        private readonly List<SubmittedAssignment> _submissions = new();

        public FakeSubmittedAssignmentRepository(IAssignmentRepository assignmentRepository)
        {
            _assignmentRepository = assignmentRepository;
            Seed();
        }

        // -----------------------------------------------------------------------
        // Seed
        // -----------------------------------------------------------------------
        private void Seed()
        {
            var demoAssignment = BuildDemoAssignment();

            // Register with the assignment fake so the service can look it up
            // through IAssignmentRepository.GetByIdAsync during the flow.
            // (.GetAwaiter().GetResult() is acceptable here — the fake is fully
            // synchronous under the hood and this only runs once at startup.)
            _assignmentRepository.CreateAsync(demoAssignment).GetAwaiter().GetResult();
            _assignmentRepository.SaveChangesAsync().GetAwaiter().GetResult();

            // Submission 1: Pending — student has submitted, teacher hasn't started.
            var pending = SubmittedAssignment.CreateFromAssignment(
                demoAssignment,
                new Guid("44444444-0000-0000-0000-000000000001"));
            pending.Assignment = demoAssignment;
            _submissions.Add(pending);

            // Submission 2: InProgress — one question scored, one comment set.
            var inProgress = SubmittedAssignment.CreateFromAssignment(
                demoAssignment,
                new Guid("44444444-0000-0000-0000-000000000002"));
            inProgress.Assignment = demoAssignment;
            var firstExercise = inProgress.SubmittedExercises.First();
            var firstQuestion = firstExercise.SubmittedQuestions.First();
            inProgress.ScoreQuestion(firstQuestion.Id, 4, "Good approach, minor slip", null);
            inProgress.SetExerciseComment(firstExercise.Id, "Solid first attempt.");
            _submissions.Add(inProgress);

            // Submission 3: Completed — all questions scored.
            var completed = SubmittedAssignment.CreateFromAssignment(
                demoAssignment,
                new Guid("44444444-0000-0000-0000-000000000003"));
            completed.Assignment = demoAssignment;
            foreach (var q in completed.SubmittedExercises.SelectMany(se => se.SubmittedQuestions))
            {
                completed.ScoreQuestion(q.Id, q.MaxPoints, "Correct.", null);
            }
            completed.MarkEvaluated(new Guid("55555555-0000-0000-0000-000000000001"));
            _submissions.Add(completed);
        }

        private static Assignment BuildDemoAssignment()
        {
            var assignment = new Assignment(
                "Demo evaluation target",
                "Pre-seeded assignment used by the submission fake.");

            // Reflect the well-known demo ID so callers can navigate to it directly.
            typeof(Assignment).GetProperty(nameof(Assignment.Id))!
                .SetValue(assignment, DemoAssignmentId);

            var ex1 = assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Basic arithmetic",
                "Solve each problem and show your working.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "What is 7 + 5?", "Show working."),
                    new(Guid.NewGuid(), "What is 12 × 4?", "Show working."),
                }));
            foreach (var q in ex1.Questions)
                ex1.SetQuestionPoints(q.Id, 5);

            var ex2 = assignment.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Word problems",
                "Read carefully before answering.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(),
                        "A train travels 60 km in 1 hour. How far in 2.5 hours?",
                        "Assume constant speed."),
                }));
            ex2.SetQuestionPoints(ex2.Questions[0].Id, 10);

            return assignment;
        }

        // -----------------------------------------------------------------------
        // ISubmittedAssignmentRepository
        // -----------------------------------------------------------------------

        public Task<SubmittedAssignment?> GetByIdAsync(Guid id)
        {
            var submission = _submissions.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(submission);
        }

        public Task<IEnumerable<SubmittedAssignment>> GetByAssignmentIdAsync(Guid assignmentId)
        {
            IEnumerable<SubmittedAssignment> result =
                _submissions.Where(s => s.AssignmentId == assignmentId).ToList();
            return Task.FromResult(result);
        }

        public Task CreateAsync(SubmittedAssignment submittedAssignment)
        {
            _submissions.Add(submittedAssignment);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }
}
