using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;

namespace Assignment_Infrastructure.DataAccess.Fakes
{
    /// <summary>
    /// In-memory fake repository for SubmittedAssignment. Use this while the
    /// real database isn't available.
    ///
    /// Takes both IAssignmentRepository and IAssignmentSetRepository so the
    /// seeded demo assignment is reachable through normal UI navigation:
    /// AssignmentSets list → set detail → demo assignment → submissions.
    /// Without that, the demo assignment would be orphaned from the
    /// AssignmentSet branch and only reachable by typing its id into the URL.
    /// </summary>
    public class FakeSubmittedAssignmentRepository : ISubmittedAssignmentRepository
    {
        // Stable demo assignment ID exposed for tests / API exploration.
        public static readonly Guid DemoAssignmentId =
            new("33333333-0000-0000-0000-000000000001");

        // Stable host-set ID — only used if no draft set exists at seed time.
        public static readonly Guid DemoSetId =
            new("33333333-0000-0000-0000-000000000099");

        /// <summary>
        /// The set the demo assignment was attached to during seeding.
        /// Read after construction for convenience (e.g. for demo URLs).
        /// </summary>
        public Guid HostSetId { get; private set; }

        private readonly IAssignmentRepository _assignmentRepository;
        private readonly List<SubmittedAssignment> _submissions = new();

        public FakeSubmittedAssignmentRepository(
            IAssignmentRepository assignmentRepository,
            IAssignmentSetRepository assignmentSetRepository)
        {
            _assignmentRepository = assignmentRepository;
            Seed(assignmentSetRepository);
        }

        // -----------------------------------------------------------------------
        // Seed
        // -----------------------------------------------------------------------
        private void Seed(IAssignmentSetRepository assignmentSetRepository)
        {
            var demoAssignment = BuildDemoAssignment();

            // Find a draft, active set to host the demo assignment so the
            // UI navigation flow (list → set → assignment → submissions) works.
            var sets = assignmentSetRepository.GetAllAsync()
                .GetAwaiter().GetResult()
                .ToList();

            var hostSet = sets.FirstOrDefault(s =>
                s.IsPublihsed != true && s.Inactive != true);

            if (hostSet is not null)
            {
                hostSet.AddAssignment(demoAssignment);
                HostSetId = hostSet.Id;
            }
            else
            {
                // No existing set worked — fall back to creating a dedicated one.
                var fallback = new AssignmentSet(
                    courseId: Guid.NewGuid(),
                    title: "Demo (auto-created)",
                    description: "Auto-created so the demo submission flow is reachable.");

                // Force a stable id so demo URLs are predictable.
                typeof(AssignmentSet).GetProperty(nameof(AssignmentSet.Id))!
                    .SetValue(fallback, DemoSetId);

                fallback.AddAssignment(demoAssignment);
                assignmentSetRepository.CreateAsync(fallback).GetAwaiter().GetResult();
                assignmentSetRepository.SaveChangesAsync().GetAwaiter().GetResult();
                HostSetId = fallback.Id;
            }

            // Register with the assignment fake so the assignment endpoints work too.
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