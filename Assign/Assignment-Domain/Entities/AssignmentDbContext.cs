using System;
using System.Collections.Generic;
using Assignment_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment_Api;

public partial class AssignmentDbContext : DbContext
{
    public AssignmentDbContext()
    {
    }

    public AssignmentDbContext(DbContextOptions<AssignmentDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentExercise> AssignmentExercises { get; set; }

    public virtual DbSet<AssignmentQuestion> AssignmentQuestions { get; set; }

    public virtual DbSet<AssignmentSet> AssignmentSets { get; set; }

    public virtual DbSet<ErrorType> ErrorTypes { get; set; }

    public virtual DbSet<GradeSheet> GradeSheets { get; set; }

    public virtual DbSet<SelfEvaluation> SelfEvaluations { get; set; }

    public virtual DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

    public virtual DbSet<SubmittedExercise> SubmittedExercises { get; set; }

    public virtual DbSet<SubmittedQuestion> SubmittedQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,14333;Database=AssignDB;User Id=sa;Password=Admin123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Danish_Norwegian_CI_AS");

        // ── Assignment ────────────────────────────────────────────────────────

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.ToTable("Assignment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.AssignmentSet).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.AssignmentSetId)
                .HasConstraintName("FK_Assignment_AssignmentSet");
        });

        // ── AssignmentExercise ────────────────────────────────────────────────

        modelBuilder.Entity<AssignmentExercise>(entity =>
        {
            entity.ToTable("AssignmentExercise");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AssignmentId).IsRequired();
            entity.Property(e => e.SourceExerciseId).IsRequired();
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content);
            entity.Property(e => e.Order).IsRequired();
            entity.Property(e => e.SnapshotTakenAt).IsRequired();
            entity.Property(e => e.RowVersion).IsRowVersion().IsConcurrencyToken();

            entity.HasOne(d => d.Assignment)
                .WithMany(p => p.AssignmentExercises)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AssignmentExercise_Assignment");

            entity.HasMany(ae => ae.Questions)
                .WithOne()
                .HasForeignKey(q => q.AssignmentExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Required because Questions is exposed via a read-only IReadOnlyList
            // backed by a private List field.
            entity.Navigation(ae => ae.Questions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        // ── AssignmentQuestion ────────────────────────────────────────────────

        modelBuilder.Entity<AssignmentQuestion>(entity =>
        {
            entity.ToTable("AssignmentQuestion");
            entity.HasKey(q => q.Id);

            entity.Property(q => q.Id).ValueGeneratedNever();
            entity.Property(q => q.AssignmentExerciseId).IsRequired();
            entity.Property(q => q.SourceQuestionId).IsRequired();
            entity.Property(q => q.Title).IsRequired().HasMaxLength(255);
            entity.Property(q => q.Content);
            entity.Property(q => q.Points).IsRequired();
            entity.Property(q => q.Order).IsRequired();
            entity.Property(q => q.RowVersion).IsRowVersion().IsConcurrencyToken();
        });

        // ── AssignmentSet ─────────────────────────────────────────────────────

        modelBuilder.Entity<AssignmentSet>(entity =>
        {
            entity.ToTable("AssignmentSet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength();
        });

        // ── ErrorType ─────────────────────────────────────────────────────────

        modelBuilder.Entity<ErrorType>(entity =>
        {
            entity.ToTable("ErrorType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(1000);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        // ── GradeSheet ────────────────────────────────────────────────────────

        modelBuilder.Entity<GradeSheet>(entity =>
        {
            entity.ToTable("GradeSheet");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.AssignmentSet).WithMany(p => p.GradeSheets)
                .HasForeignKey(d => d.AssignmentSetId)
                .HasConstraintName("FK_GradeSheet_AssignmentSet");
        });

        // ── SelfEvaluation (kept for future feature; not actively used) ───────
        // Configured without an inverse navigation because SubmittedExercise no
        // longer exposes a SelfEvaluations collection in the new domain model.
        // The FK still exists at the database level.

        modelBuilder.Entity<SelfEvaluation>(entity =>
        {
            entity.ToTable("SelfEvaluation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.SubmittedExercise).WithMany()
                .HasForeignKey(d => d.SubmittedExerciseId)
                .HasConstraintName("FK_SelfEvaluation_SubmittedExercise");
        });

        // ── SubmittedAssignment ───────────────────────────────────────────────

        modelBuilder.Entity<SubmittedAssignment>(entity =>
        {
            entity.ToTable("SubmittedAssignment");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AssignmentId).IsRequired();
            entity.Property(e => e.StudentId).IsRequired();
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();
            entity.Property(e => e.EvaluatorId);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Assignment).WithMany(p => p.SubmittedAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SubmittedAssignment_Assignment");

            entity.HasMany(s => s.SubmittedExercises)
                .WithOne(se => se.SubmittedAssignment!)
                .HasForeignKey(se => se.SubmittedAssignmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SubmittedExercise_SubmittedAssignment");

            // Required because SubmittedExercises is exposed via a read-only
            // IReadOnlyCollection backed by a private List field.
            entity.Navigation(s => s.SubmittedExercises)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        // ── SubmittedExercise ─────────────────────────────────────────────────

        modelBuilder.Entity<SubmittedExercise>(entity =>
        {
            entity.ToTable("SubmittedExercise");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SubmittedAssignmentId).IsRequired();
            entity.Property(e => e.AssignmentExerciseId).IsRequired();
            entity.Property(e => e.OverallComment)
                .HasMaxLength(1000);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasMany(se => se.SubmittedQuestions)
                .WithOne()
                .HasForeignKey(sq => sq.SubmittedExerciseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_SubmittedQuestion_SubmittedExercise");

            // Same reason as SubmittedAssignment.SubmittedExercises and
            // AssignmentExercise.Questions — backing field access required.
            entity.Navigation(se => se.SubmittedQuestions)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        // ── SubmittedQuestion (new) ───────────────────────────────────────────

        modelBuilder.Entity<SubmittedQuestion>(entity =>
        {
            entity.ToTable("SubmittedQuestion");
            entity.HasKey(sq => sq.Id);

            entity.Property(sq => sq.Id).ValueGeneratedNever();
            entity.Property(sq => sq.SubmittedExerciseId).IsRequired();
            entity.Property(sq => sq.AssignmentQuestionId).IsRequired();
            entity.Property(sq => sq.MaxPoints).IsRequired();
            entity.Property(sq => sq.PointsAwarded);
            entity.Property(sq => sq.Comment)
                .HasMaxLength(1000);
            entity.Property(sq => sq.ErrorTypeId);
            entity.Property(sq => sq.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            // Optional FK to ErrorType. SetNull on delete: if a teacher deletes
            // an error type that's already been used in graded submissions,
            // those references become null rather than blocking the delete.
            entity.HasOne<ErrorType>()
                .WithMany()
                .HasForeignKey(sq => sq.ErrorTypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_SubmittedQuestion_ErrorType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
