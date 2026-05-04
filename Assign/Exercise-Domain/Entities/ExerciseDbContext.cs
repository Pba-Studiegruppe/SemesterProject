using Microsoft.EntityFrameworkCore;
using Exercise_Domain.Entities;

namespace Exercise_Api
{
    public partial class ExerciseDbContext : DbContext
    {
        public ExerciseDbContext() { }

        public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options)
            : base(options) { }

        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<ExerciseSolution> ExerciseSolutions { get; set; }
        public virtual DbSet<QuestionSolution> QuestionSolutions { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<ExerciseKeyword> ExerciseKeywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exercise
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedByTeacherId).IsRequired();
                entity.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

                entity.HasMany(e => e.Questions)
                      .WithOne()
                      .HasForeignKey(q => q.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.ExerciseKeywords)
                      .WithOne()
                      .HasForeignKey(ek => ek.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Solution)
                      .WithOne()
                      .HasForeignKey<ExerciseSolution>(es => es.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Question
            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Id).ValueGeneratedNever();
                entity.Property(q => q.Title).IsRequired().HasMaxLength(255);
                entity.Property(q => q.Content).IsRequired();
                entity.Property(q => q.ExerciseId).IsRequired();
                entity.Property(q => q.RowVersion).IsRowVersion().IsConcurrencyToken();

                entity.HasOne(q => q.Solution)
                      .WithOne()
                      .HasForeignKey<QuestionSolution>(qs => qs.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ExerciseSolution
            modelBuilder.Entity<ExerciseSolution>(entity =>
            {
                entity.HasKey(es => es.Id);
                entity.Property(es => es.Id).ValueGeneratedNever();
                entity.Property(es => es.ExerciseId).IsRequired();
                entity.Property(es => es.Content).IsRequired();
                entity.Property(es => es.VideoUrl).HasMaxLength(1024);
                entity.Property(es => es.RowVersion).IsRowVersion().IsConcurrencyToken();
            });

            // QuestionSolution
            modelBuilder.Entity<QuestionSolution>(entity =>
            {
                entity.HasKey(qs => qs.Id);
                entity.Property(qs => qs.Id).ValueGeneratedNever();
                entity.Property(qs => qs.QuestionId).IsRequired();
                entity.Property(qs => qs.Content).IsRequired();
                entity.Property(qs => qs.RowVersion).IsRowVersion().IsConcurrencyToken();
            });

            // Keyword
            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.Property(k => k.Id).ValueGeneratedNever();
                entity.Property(k => k.KeywordName).IsRequired().HasMaxLength(255);
                entity.Property(k => k.KeywordType).IsRequired();
                entity.Property(k => k.RowVersion).IsRowVersion().IsConcurrencyToken();
            });

            // ExerciseKeyword (join table)
            modelBuilder.Entity<ExerciseKeyword>(entity =>
            {
                entity.HasKey(ek => new { ek.ExerciseId, ek.KeywordId });
                entity.Property(ek => ek.ExerciseId).IsRequired();
                entity.Property(ek => ek.KeywordId).IsRequired();

                entity.HasOne<Exercise>()
                      .WithMany()
                      .HasForeignKey(ek => ek.ExerciseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Keyword>()
                      .WithMany()
                      .HasForeignKey(ek => ek.KeywordId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}