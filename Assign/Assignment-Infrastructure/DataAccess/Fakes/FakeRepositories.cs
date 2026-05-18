using Assignment_Application.Interfaces.Repositories;
using Assignment_Domain.Entities;
using Assignment_Domain.SnapShots;

namespace Assignment_Infrastructure.DataAccess.Fakes
{
    // Made with Claude

    /// <summary>
    /// In-memory fake repository for Assignment. Use this while the real database
    /// is not yet available. Pre-seeded with a realistic spread of assignments:
    /// some with no exercises, some with one, some with many; some with points
    /// assigned to questions, some without.
    /// </summary>
    public class FakeAssignmentRepository : IAssignmentRepository
    {
        private readonly List<Assignment> _assignments;

        public FakeAssignmentRepository()
        {
            _assignments = new List<Assignment>();
            Seed();
        }

        // -----------------------------------------------------------------------
        // Seed
        // -----------------------------------------------------------------------
        private void Seed()
        {
            // ── Group 1: bare assignments, no exercises ──────────────────────────
            // Just a title + description. The teacher hasn't added exercises yet.

            var a1 = new Assignment(
                "Algebra Basics",
                "Introduction to algebraic equations and notation.");
            _assignments.Add(a1);

            var a2 = new Assignment(
                "World War II Overview",
                "Causes, key events, and aftermath.");
            _assignments.Add(a2);

            // ── Group 2: one exercise, single question, no points ────────────────

            var a3 = new Assignment(
                "Newton's First Law",
                "Quick check on classical mechanics.");
            a3.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Inertia",
                "Read the passage and answer the question below.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(),
                        "State Newton's First Law",
                        "In your own words, explain Newton's First Law of Motion."),
                }));
            _assignments.Add(a3);

            // ── Group 3: one exercise, single question, points set ───────────────

            var a4 = new Assignment(
                "Solving Linear Equations",
                "Solve the equation and show every step of your working.");
            var a4ex = a4.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Linear equations",
                "Show all work.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(),
                        "Solve 3x + 7 = 22",
                        "Show every step."),
                }));
            a4ex.SetQuestionPoints(a4ex.Questions[0].Id, 10);
            _assignments.Add(a4);

            // ── Group 4: one exercise, multiple questions, varied points ─────────

            var a5 = new Assignment(
                "Periodic Table Quiz",
                "Four short questions on group and period structure.");
            var a5ex = a5.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Groups and Periods",
                "Use the periodic table if you need to.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "What does the group number indicate?",
                        "One sentence."),
                    new(Guid.NewGuid(), "Name two elements in Group 1",
                        "Include their symbols."),
                    new(Guid.NewGuid(), "Define noble gas",
                        "Give one example."),
                    new(Guid.NewGuid(), "Order Cl, Na, Ar, Mg by atomic number",
                        "Smallest to largest."),
                }));
            var a5qs = a5ex.Questions.Select(q => q.Id).ToList();
            a5ex.SetQuestionPoints(a5qs[0], 2);
            a5ex.SetQuestionPoints(a5qs[1], 3);
            a5ex.SetQuestionPoints(a5qs[2], 5);
            a5ex.SetQuestionPoints(a5qs[3], 5);
            _assignments.Add(a5);

            // ── Group 5: multiple exercises, all with points ─────────────────────

            var a6 = new Assignment(
                "End-of-Term Math Test",
                "Covers algebra, geometry, and statistics.");

            var a6ex1 = a6.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Algebra",
                "Solve and simplify.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Solve 2x + 3 = 11", "Show working."),
                    new(Guid.NewGuid(), "Factor x² - 5x + 6", "Show your method."),
                }));
            foreach (var q in a6ex1.Questions)
                a6ex1.SetQuestionPoints(q.Id, 5);

            var a6ex2 = a6.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Geometry",
                "Use the formulas given in class.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Area of a circle, r = 6", "Two decimal places."),
                    new(Guid.NewGuid(), "Pythagoras for a = 3, b = 4", "Show working."),
                }));
            foreach (var q in a6ex2.Questions)
                a6ex2.SetQuestionPoints(q.Id, 4);

            var a6ex3 = a6.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Statistics",
                "Use the data set in the appendix.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Find the mean", "Sum ÷ count."),
                    new(Guid.NewGuid(), "Find the median", "Sort first."),
                    new(Guid.NewGuid(), "Find the mode", "Most frequent value."),
                }));
            foreach (var q in a6ex3.Questions)
                a6ex3.SetQuestionPoints(q.Id, 3);

            _assignments.Add(a6);

            // ── Group 6: edge cases ──────────────────────────────────────────────

            // Empty description
            var a7 = new Assignment(
                "Reading Comprehension - Romeo and Juliet",
                "");
            _assignments.Add(a7);

            // Long, multi-sentence description
            var a8 = new Assignment(
                "Climate Change Investigation",
                "Investigate the human and natural drivers of climate change over the " +
                "past century. Your final write-up should cover greenhouse gases, ocean " +
                "temperature trends, ice mass loss, and at least two mitigation strategies " +
                "currently being discussed in international policy.");
            _assignments.Add(a8);

            // Exercise with no questions at all (teacher added the exercise shell but
            // hasn't authored questions yet).
            var a9 = new Assignment(
                "Photosynthesis - draft",
                "Work in progress; questions to follow.");
            a9.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Light reactions",
                "Background reading on photosynthesis.",
                new List<QuestionSnapshotInput>()));
            _assignments.Add(a9);
        }

        // -----------------------------------------------------------------------
        // IAssignmentRepository
        // -----------------------------------------------------------------------

        public Task CreateAsync(Assignment assignment)
        {
            _assignments.Add(assignment);
            return Task.CompletedTask;
        }

        public Task<Assignment?> GetByIdAsync(Guid id)
        {
            var assignment = _assignments.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(assignment);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }


    /// <summary>
    /// In-memory fake repository for AssignmentSet. Use this while the real
    /// database is not yet available. Pre-seeded with sets in various lifecycle
    /// states (empty, in-progress, published, inactive, course-less) across
    /// three fake course IDs so that GetByCourseIdAsync returns meaningful
    /// subsets.
    /// </summary>
    public class FakeAssignmentSetRepository : IAssignmentSetRepository
    {
        // Three fake course IDs so GetByCourseIdAsync returns meaningful subsets.
        public static readonly Guid Course1 = new("22222222-0000-0000-0000-000000000001");
        public static readonly Guid Course2 = new("22222222-0000-0000-0000-000000000002");
        public static readonly Guid Course3 = new("22222222-0000-0000-0000-000000000003");

        private readonly List<AssignmentSet> _sets;

        public FakeAssignmentSetRepository()
        {
            _sets = new List<AssignmentSet>();
            Seed();
        }

        // -----------------------------------------------------------------------
        // Seed
        // -----------------------------------------------------------------------
        private void Seed()
        {
            // ── Course 1: a brand-new course with two unpublished sets ───────────

            // Empty set — no assignments yet.
            var s1 = new AssignmentSet(
                Course1,
                "Week 1 - Welcome",
                "Get the students settled in.");
            _sets.Add(s1);

            // Set with one in-progress assignment.
            var s2 = new AssignmentSet(
                Course1,
                "Week 2 - Foundations",
                "Build the foundation for the rest of the course.");
            var s2a = new Assignment(
                "Intro problem set",
                "A few warm-up problems.");
            s2a.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Warm-up",
                "Solve these.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "What is 7 × 8?", "Mental arithmetic.")
                }));
            s2.AddAssignment(s2a);
            _sets.Add(s2);

            // ── Course 2: a course mid-term with multiple sets ───────────────────

            // Published set with two assignments — students can work on it.
            var s3 = new AssignmentSet(
                Course2,
                "Mid-term assessment",
                "Combined algebra and geometry mid-term.");

            var s3a = new Assignment(
                "Mid-term part A",
                "Algebra: 30 minutes recommended.");
            s3a.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Equations",
                "Solve.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(), "Solve 2x - 5 = 9", "Show working.")
                }));
            s3.AddAssignment(s3a);

            var s3b = new Assignment(
                "Mid-term part B",
                "Geometry: 30 minutes recommended.");
            s3b.AddExerciseFromSnapshot(new ExerciseSnapshotInput(
                Guid.NewGuid(),
                "Triangles",
                "Use the diagrams.",
                new List<QuestionSnapshotInput>
                {
                    new(Guid.NewGuid(),
                        "Find the missing angle",
                        "Use the angle sum property.")
                }));
            s3.AddAssignment(s3b);

            // Publish() requires CourseId, at least one assignment, and every
            // assignment must have at least one exercise — all satisfied above.
            s3.Publish();
            _sets.Add(s3);

            // Another set for Course 2, still in draft.
            var s4 = new AssignmentSet(
                Course2,
                "Bonus assignments",
                "Optional enrichment problems.");
            _sets.Add(s4);

            // ── Course 3: a winding-down course with an inactive set ─────────────

            var s5 = new AssignmentSet(
                Course3,
                "End-of-year review",
                "Final review covering the year's material.");
            s5.AddAssignment(new Assignment(
                "Review sheet",
                "Twelve mixed problems."));
            _sets.Add(s5);

            // Inactive set: kept around from a previous semester.
            var s6 = new AssignmentSet(
                Course3,
                "Last semester archive",
                "Kept for reference only.");
            s6.Inactive = true;
            _sets.Add(s6);

            // ── No-course set: a template not bound to any course ────────────────
            // Useful for verifying that GetByCourseIdAsync correctly excludes
            // sets with null CourseId.
            var s7 = new AssignmentSet(
                null,
                "Template set",
                "Cloned by teachers when starting a new course.");
            _sets.Add(s7);
        }

        // -----------------------------------------------------------------------
        // IAssignmentSetRepository
        // -----------------------------------------------------------------------

        public Task CreateAsync(AssignmentSet assignmentSet)
        {
            _sets.Add(assignmentSet);
            return Task.CompletedTask;
        }

        public Task<AssignmentSet?> GetByIdAsync(Guid id)
        {
            var set = _sets.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(set);
        }

        public Task<IEnumerable<AssignmentSet>> GetByCourseIdAsync(Guid courseId)
        {
            IEnumerable<AssignmentSet> result =
                _sets.Where(s => s.CourseId == courseId).ToList();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<AssignmentSet>> GetAllAsync()
        {
            IEnumerable<AssignmentSet> result = _sets.ToList();
            return Task.FromResult(result);
        }

        public Task SaveChangesAsync() => Task.CompletedTask;
    }
}