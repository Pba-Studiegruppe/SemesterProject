using Exercise_Application.Interfaces.Repositories;
using Exercise_Domain.Entities;

namespace Exercise_Infrastructure.DataAccess.Fakes
{
    // Made with Claude

    /// <summary>
    /// In-memory fake repository for Keyword. Use this while the real database
    /// is not yet available. Pre-seeded with a set of keywords covering the
    /// three KeywordType categories.
    /// </summary>
    public class FakeKeywordRepository : IKeywordRepository
    {
        private readonly List<Keyword> _keywords;

        public FakeKeywordRepository()
        {
            _keywords = new List<Keyword>
            {
                // SchoolSubject
                new Keyword("Mathematics",  KeywordType.SchoolSubject),
                new Keyword("Physics",      KeywordType.SchoolSubject),
                new Keyword("Chemistry",    KeywordType.SchoolSubject),
                new Keyword("Biology",      KeywordType.SchoolSubject),
                new Keyword("History",      KeywordType.SchoolSubject),
                new Keyword("Geography",    KeywordType.SchoolSubject),
                new Keyword("Literature",   KeywordType.SchoolSubject),

                // ExerciseType
                new Keyword("Multiple Choice",  KeywordType.ExerciseType),
                new Keyword("Open Question",    KeywordType.ExerciseType),
                new Keyword("True / False",     KeywordType.ExerciseType),
                new Keyword("Fill in the Blank",KeywordType.ExerciseType),
                new Keyword("Essay",            KeywordType.ExerciseType),
                new Keyword("Calculation",      KeywordType.ExerciseType),

                // SubjectArea
                new Keyword("Algebra",          KeywordType.SubjectArea),
                new Keyword("Geometry",         KeywordType.SubjectArea),
                new Keyword("Statistics",       KeywordType.SubjectArea),
                new Keyword("Thermodynamics",   KeywordType.SubjectArea),
                new Keyword("Organic Chemistry",KeywordType.SubjectArea),
                new Keyword("World War II",     KeywordType.SubjectArea),
                new Keyword("Climate",          KeywordType.SubjectArea),
            };
        }

        public Task<Keyword> AddKeywordAsync(Keyword keyword)
        {
            _keywords.Add(keyword);
            return Task.FromResult(keyword);
        }

        public Task<IEnumerable<Keyword>> GetAllKeywordsAsync()
        {
            return Task.FromResult<IEnumerable<Keyword>>(_keywords.ToList());
        }

        public Task<Keyword> GetKeywordByIdAsync(Guid keywordId)
        {
            var keyword = _keywords.FirstOrDefault(k => k.Id == keywordId);
            return Task.FromResult(keyword);
        }
    }

    /// <summary>
    /// In-memory fake repository for Exercise. Use this while the real database
    /// is not yet available. Starts empty; exercises are added through the normal
    /// service layer just as they would be against a real database.
    /// </summary>
    /// <summary>
    /// In-memory fake repository for Exercise. Use this while the real database
    /// is not yet available. Pre-seeded with a realistic spread of exercises:
    /// some with no questions, some with one, some with many; some with solutions,
    /// some without; spread across multiple fake teacher IDs.
    /// </summary>
    public class FakeExerciseRepository : IExerciseRepository
    {
        private readonly List<Exercise> _exercises;

        // Three fake teacher IDs so GetByTeacherIdAsync returns meaningful subsets.
        public static readonly Guid Teacher1 = new("11111111-0000-0000-0000-000000000001");
        public static readonly Guid Teacher2 = new("11111111-0000-0000-0000-000000000002");
        public static readonly Guid Teacher3 = new("11111111-0000-0000-0000-000000000003");

        public FakeExerciseRepository()
        {
            _exercises = new List<Exercise>();
            Seed();
        }

        // -----------------------------------------------------------------------
        // Seed
        // -----------------------------------------------------------------------
        private void Seed()
        {
            // ── Group 1: No questions, no solution ───────────────────────────────
            // Just a title and content – the teacher hasn't added anything yet.

            var e1 = new Exercise(
                "Introduction to Algebra",
                "A brief overview of algebraic thinking and notation. " +
                "Students will get acquainted with variables, constants, and basic operations.",
                Teacher1);
            e1.SetCreatedAt(new DateTime(2025, 9, 1));
            _exercises.Add(e1);

            var e2 = new Exercise(
                "The Water Cycle",
                "This exercise introduces the water cycle: evaporation, condensation, " +
                "precipitation, and collection. Suitable for grade 6 geography.",
                Teacher2);
            e2.SetCreatedAt(new DateTime(2025, 9, 5));
            _exercises.Add(e2);

            // ── Group 2: Title only – content left empty ─────────────────────────
            // Teacher created a placeholder and will fill it in later.

            var e3 = new Exercise(
                "World War II – Causes and Consequences",
                "",   // content deliberately empty
                Teacher1);
            e3.SetCreatedAt(new DateTime(2025, 10, 2));
            _exercises.Add(e3);

            var e4 = new Exercise(
                "Organic Chemistry: Functional Groups",
                "",
                Teacher3);
            e4.SetCreatedAt(new DateTime(2025, 10, 14));
            _exercises.Add(e4);

            // ── Group 3: Single question, no solution anywhere ────────────────────

            var e5 = new Exercise(
                "Newton's First Law",
                "Read the passage below and answer the question.",
                Teacher2);
            e5.SetCreatedAt(new DateTime(2025, 9, 18));
            e5.AddQuestion(
                "State Newton's First Law",
                "In your own words, explain what Newton's First Law of Motion says " +
                "and give one everyday example.");
            _exercises.Add(e5);

            var e6 = new Exercise(
                "Photosynthesis",
                "Use what you know about plant biology to answer the following.",
                Teacher3);
            e6.SetCreatedAt(new DateTime(2025, 11, 3));
            e6.AddQuestion(
                "What are the reactants of photosynthesis?",
                "List the raw materials a plant needs to carry out photosynthesis " +
                "and state where each comes from.");
            _exercises.Add(e6);

            // ── Group 4: Single question WITH a question solution ─────────────────

            var e7 = new Exercise(
                "Solving Linear Equations",
                "Solve the equation and show every step of your working.",
                Teacher1);
            e7.SetCreatedAt(new DateTime(2025, 9, 22));
            e7.AddQuestion(
                "Solve: 3x + 7 = 22",
                "Show all steps.");
            e7.Questions.First().SetSolution(
                "3x + 7 = 22  →  3x = 15  →  x = 5");
            _exercises.Add(e7);

            var e8 = new Exercise(
                "True or False: Climate Facts",
                "Decide whether each statement is true or false.",
                Teacher2);
            e8.SetCreatedAt(new DateTime(2025, 10, 9));
            e8.AddQuestion(
                "The greenhouse effect is entirely man-made.",
                "Answer True or False and justify your answer in one sentence.");
            e8.Questions.First().SetSolution(
                "False. The greenhouse effect is a natural process; human activity has " +
                "intensified it, but it exists independently of human influence.");
            _exercises.Add(e8);

            // ── Group 5: Single question, exercise has a solution too ─────────────

            var e9 = new Exercise(
                "Area of a Circle",
                "Use the formula A = πr² to answer the question below.",
                Teacher1);
            e9.SetCreatedAt(new DateTime(2025, 9, 30));
            e9.AddQuestion(
                "Calculate the area when r = 6 cm",
                "Give your answer to two decimal places.");
            e9.Questions.First().SetSolution(
                "A = π × 6² = π × 36 ≈ 113.10 cm²");
            e9.SetSolution(
                "The formula A = πr² gives the area of a circle. " +
                "For r = 6: A = π × 36 ≈ 113.10 cm².",
                null);
            _exercises.Add(e9);

            // ── Group 6: Two questions, no solutions ──────────────────────────────

            var e10 = new Exercise(
                "The French Revolution",
                "Answer both questions using your textbook and class notes.",
                Teacher3);
            e10.SetCreatedAt(new DateTime(2025, 11, 11));
            e10.AddQuestion(
                "What were the main causes of the French Revolution?",
                "Discuss at least three political, social, or economic causes.");
            e10.AddQuestion(
                "Who was Robespierre?",
                "Describe his role during the Reign of Terror in 2–3 sentences.");
            _exercises.Add(e10);

            var e11 = new Exercise(
                "Reading Comprehension: Short Story",
                "Read the attached excerpt and answer the questions that follow.",
                Teacher2);
            e11.SetCreatedAt(new DateTime(2025, 12, 1));
            e11.AddQuestion(
                "What is the central theme of the excerpt?",
                "Identify the theme and support your answer with two quotations.");
            e11.AddQuestion(
                "Describe the protagonist",
                "Use evidence from the text to describe the protagonist's personality.");
            _exercises.Add(e11);

            // ── Group 7: Two questions, mixed solutions ───────────────────────────

            var e12 = new Exercise(
                "Speed, Distance, and Time",
                "Use the formula v = d/t to solve the problems below.",
                Teacher1);
            e12.SetCreatedAt(new DateTime(2025, 10, 20));
            e12.AddQuestion(
                "A car travels 150 km in 2 hours. What is its speed?",
                "Show your working.");
            e12.Questions.ToList()[0].SetSolution("v = 150 / 2 = 75 km/h");
            e12.AddQuestion(
                "How long does it take to travel 300 km at 60 km/h?",
                "Express your answer in hours and minutes.");
            // second question deliberately has no solution yet
            _exercises.Add(e12);

            // ── Group 8: Four questions, all with solutions ───────────────────────

            var e13 = new Exercise(
                "The Periodic Table – Groups and Periods",
                "Answer all four questions. You may use a periodic table.",
                Teacher3);
            e13.SetCreatedAt(new DateTime(2025, 11, 25));

            e13.AddQuestion(
                "What does the group number of an element indicate?",
                "One or two sentences.");
            e13.Questions.ToList()[0].SetSolution(
                "The group number indicates the number of valence electrons in the outer shell.");

            e13.AddQuestion(
                "Name two elements in Group 1",
                "State their symbols as well.");
            e13.Questions.ToList()[1].SetSolution(
                "Lithium (Li) and Sodium (Na) are both in Group 1 (the alkali metals).");

            e13.AddQuestion(
                "What is a noble gas? Give one example.",
                "Explain why noble gases are chemically unreactive.");
            e13.Questions.ToList()[2].SetSolution(
                "Noble gases have a full outer electron shell and therefore do not need to " +
                "gain or lose electrons. Example: Helium (He).");

            e13.AddQuestion(
                "Arrange the following in order of increasing atomic number: Cl, Na, Ar, Mg",
                "Write the full name and atomic number of each.");
            e13.Questions.ToList()[3].SetSolution(
                "Na (11) → Mg (12) → Cl (17) → Ar (18)");

            e13.SetSolution(
                "Full answer key: Group number = valence electrons; Group 1 examples: Li, Na; " +
                "Noble gases have full outer shells; Ordered: Na(11), Mg(12), Cl(17), Ar(18).",
                "https://www.youtube.com/watch?v=0RRVV4Diomg");
            _exercises.Add(e13);

            // ── Group 9: Many questions (7), no solutions ────────────────────────

            var e14 = new Exercise(
                "Statistics Revision",
                "A comprehensive revision exercise covering mean, median, mode, range, " +
                "and basic probability. Show all working.",
                Teacher1);
            e14.SetCreatedAt(new DateTime(2026, 1, 8));

            var statQuestions = new[]
            {
                ("What is the mean of 4, 8, 6, 5, 3, 2, 8, 9, 2, 5?",
                 "Sum all values then divide by the count."),
                ("Find the median of the same data set.",
                 "Sort the values first."),
                ("What is the mode?",
                 "There may be more than one."),
                ("What is the range?",
                 "Range = max − min."),
                ("If a bag contains 3 red and 7 blue marbles, what is the probability of drawing a red marble?",
                 "Express as a fraction and a percentage."),
                ("Are the events 'drawing red' and 'drawing blue' mutually exclusive? Explain.",
                 "One sentence is sufficient."),
                ("A student scores 72, 85, 90, 68, and 95. What score do they need on the next test to have a mean of 85?",
                 "Set up an equation and solve."),
            };

            foreach (var (title, content) in statQuestions)
                e14.AddQuestion(title, content);

            _exercises.Add(e14);

            // ── Group 10: Many questions (5), exercise-level solution + video ─────

            var e15 = new Exercise(
                "Thermodynamics: The Laws",
                "This exercise covers all three laws of thermodynamics. " +
                "Ideal for end-of-unit revision.",
                Teacher2);
            e15.SetCreatedAt(new DateTime(2026, 2, 14));

            e15.AddQuestion("State the Zeroth Law of thermodynamics.",
                "One sentence definition.");
            e15.Questions.ToList()[0].SetSolution(
                "If system A is in thermal equilibrium with system B, and B with C, then A is in thermal equilibrium with C.");

            e15.AddQuestion("State the First Law of thermodynamics.",
                "Include the mathematical form ΔU = Q − W.");
            e15.Questions.ToList()[1].SetSolution(
                "Energy cannot be created or destroyed; ΔU = Q − W, where Q is heat added and W is work done by the system.");

            e15.AddQuestion("What does the Second Law say about entropy?",
                "Explain in plain language.");
            e15.Questions.ToList()[2].SetSolution(
                "The total entropy of an isolated system can only increase over time or remain constant; it never decreases spontaneously.");

            e15.AddQuestion("Give a real-world example illustrating the Second Law.",
                "Two to three sentences.");
            e15.Questions.ToList()[3].SetSolution(
                "An ice cube melting in a warm drink: heat flows from the warm liquid to the ice, increasing overall entropy. The reverse — the drink spontaneously freezing — never happens.");

            e15.AddQuestion("Why is a perpetual motion machine of the second kind impossible?",
                "Relate your answer to the Second Law.");
            e15.Questions.ToList()[4].SetSolution(
                "Such a machine would need to convert heat entirely into work with no waste, violating the Second Law's requirement that entropy must increase.");

            e15.SetSolution(
                "Full answer key included in the teacher notes. See also the linked video for a visual walkthrough of all three laws.",
                "https://www.khanacademy.org/science/physics/thermodynamics");
            _exercises.Add(e15);

            // ── Group 11: 10 questions, no solutions, no exercise solution ────────

            var e16 = new Exercise(
                "Literature Essay Preparation: Romeo and Juliet",
                "Work through these questions to build your essay on fate vs free will in Romeo and Juliet.",
                Teacher3);
            e16.SetCreatedAt(new DateTime(2026, 3, 3));

            var litQuestions = new[]
            {
                ("What is the central argument of the play regarding fate?",
                 "Refer to the Prologue."),
                ("How does Romeo's impulsiveness drive the plot?",
                 "Give two specific examples."),
                ("What role does Friar Lawrence play in the tragedy?",
                 "Consider both his intentions and the consequences of his actions."),
                ("How is Juliet's agency portrayed across the five acts?",
                 "Track how her independence grows."),
                ("What does the feud between the Montagues and Capulets represent thematically?",
                 "Think about social structures and inherited hatred."),
                ("Analyse the balcony scene (Act 2, Scene 2).",
                 "Focus on the use of language and its dramatic purpose."),
                ("How does Shakespeare use light and dark imagery throughout the play?",
                 "Cite at least three examples."),
                ("Compare Romeo and Juliet's attitudes to death.",
                 "What does each character's attitude reveal about them?"),
                ("To what extent are Romeo and Juliet victims of circumstance?",
                 "Consider alternative choices they could have made."),
                ("Write a thesis statement arguing either for or against fate as the primary cause of the tragedy.",
                 "One sentence; it must be arguable and specific."),
            };

            foreach (var (title, content) in litQuestions)
                e16.AddQuestion(title, content);

            _exercises.Add(e16);

            // ── Group 12: No content, no questions, has an exercise solution ──────
            // Edge case: teacher wrote the answer key before finishing the exercise.

            var e17 = new Exercise(
                "Geometry: Pythagoras' Theorem",
                "",
                Teacher1);
            e17.SetCreatedAt(new DateTime(2026, 4, 1));
            e17.SetSolution(
                "For a right-angled triangle with legs a and b and hypotenuse c: a² + b² = c². " +
                "Example: a=3, b=4 → c=√(9+16)=√25=5.",
                null);
            _exercises.Add(e17);

            // ── Group 13: Three questions, only the middle one has a solution ──────

            var e18 = new Exercise(
                "The Digestive System",
                "Answer the questions below using correct biological terminology.",
                Teacher2);
            e18.SetCreatedAt(new DateTime(2026, 2, 28));

            e18.AddQuestion(
                "List the organs of the digestive system in order.",
                "Start from the mouth and end at the large intestine.");
            // no solution

            e18.AddQuestion(
                "What is the role of enzymes in digestion?",
                "Name at least two specific enzymes and what they break down.");
            e18.Questions.ToList()[1].SetSolution(
                "Enzymes are biological catalysts. Amylase (in saliva) breaks down starch into sugars. " +
                "Protease (in the stomach and small intestine) breaks proteins into amino acids.");

            e18.AddQuestion(
                "What is the difference between mechanical and chemical digestion?",
                "Give one example of each.");
            // no solution

            _exercises.Add(e18);
        }

        // -----------------------------------------------------------------------
        // IExerciseRepository
        // -----------------------------------------------------------------------

        public Task<Exercise> AddAsync(Exercise exercise)
        {
            _exercises.Add(exercise);
            return Task.FromResult(exercise);
        }

        public Task<Exercise?> GetByIdAsync(Guid exerciseId)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == exerciseId);
            return Task.FromResult(exercise);
        }

        public Task<IEnumerable<Exercise>> GetByTeacherIdAsync(Guid teacherId)
        {
            var result = _exercises.Where(e => e.CreatedByTeacherId == teacherId);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Exercise>> GetByKeywordsAsync(IEnumerable<Guid> keywordIds)
        {
            var ids = keywordIds.ToHashSet();
            var result = _exercises
                .Where(e => e.ExerciseKeywords.Any(ek => ids.Contains(ek.KeywordId)));
            return Task.FromResult(result);
        }

        public Task<Exercise> UpdateAsync(Exercise exercise, byte[] rowVersion)
        {
            // In-memory: the object is already updated in place — nothing extra needed.
            return Task.FromResult(exercise);
        }

        public Task<Exercise?> GetForSnapshotAsync(Guid exerciseId)
        {
            // Mimics the real repo: returns the exercise without solutions.
            // In memory we can't actually strip navigation props, but the caller
            // (ExerciseSnapshotQueryService) only reads Title, Content, and Questions,
            // so this is fine for development/testing purposes.
            var exercise = _exercises.FirstOrDefault(e => e.Id == exerciseId);
            return Task.FromResult(exercise);
        }

        public Task<Exercise?> GetForReviewAsync(Guid exerciseId)
        {
            var exercise = _exercises.FirstOrDefault(e => e.Id == exerciseId);
            return Task.FromResult(exercise);
        }

        public Task<IEnumerable<Exercise>> GetAllAsync(string? search = null)
        {
            var result = _exercises.Any()
                ? _exercises.Where(e =>
                    string.IsNullOrEmpty(search) ||
                    e.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    e.Content.Contains(search, StringComparison.OrdinalIgnoreCase))
                : _exercises.AsEnumerable();
            return Task.FromResult(result);
        }
    }
}
