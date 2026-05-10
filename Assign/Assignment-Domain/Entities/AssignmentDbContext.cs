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

    public virtual DbSet<AssignmentFeedback> AssignmentFeedbacks { get; set; }

    public virtual DbSet<AssignmentSet> AssignmentSets { get; set; }

    public virtual DbSet<ErrorType> ErrorTypes { get; set; }

    public virtual DbSet<ExerciseFeedback> ExerciseFeedbacks { get; set; }

    public virtual DbSet<GradeSheet> GradeSheets { get; set; }

    public virtual DbSet<SelfEvaluation> SelfEvaluations { get; set; }

    public virtual DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

    public virtual DbSet<SubmittedExercise> SubmittedExercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,14333;Database=AssignDB;User Id=sa;Password=Admin123!;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Danish_Norwegian_CI_AS");

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

        modelBuilder.Entity<AssignmentExercise>(entity =>
        {
            entity.HasKey(e => new { e.AssignmentId, e.SourceExerciseId });

            entity.ToTable("AssignmentExercise");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentExercises)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentExercise_Assignment");
        });

        modelBuilder.Entity<AssignmentFeedback>(entity =>
        {
            entity.ToTable("AssignmentFeedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.SubmittedAssignment).WithMany(p => p.AssignmentFeedbacks)
                .HasForeignKey(d => d.SubmittedAssignmentId)
                .HasConstraintName("FK_AssignmentFeedback_SubmittedAssignment");
        });

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

        modelBuilder.Entity<ErrorType>(entity =>
        {
            entity.ToTable("ErrorType");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ErrorDescription)
                .HasMaxLength(1000)
                .IsFixedLength();
            entity.Property(e => e.ErrorName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ExerciseFeedback>(entity =>
        {
            entity.ToTable("ExerciseFeedback");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Feedbacktext)
                .HasMaxLength(1000)
                .IsFixedLength();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.ErrorTypeNavigation).WithMany(p => p.ExerciseFeedbacks)
                .HasForeignKey(d => d.ErrorType)
                .HasConstraintName("FK_ExerciseFeedback_ErrorType");

            entity.HasOne(d => d.SubmittedExercise).WithMany(p => p.ExerciseFeedbacks)
                .HasForeignKey(d => d.SubmittedExerciseId)
                .HasConstraintName("FK_ExerciseFeedback_SubmittedExercise");
        });

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

        modelBuilder.Entity<SelfEvaluation>(entity =>
        {
            entity.ToTable("SelfEvaluation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.SubmittedExercise).WithMany(p => p.SelfEvaluations)
                .HasForeignKey(d => d.SubmittedExerciseId)
                .HasConstraintName("FK_SelfEvaluation_SubmittedExercise");
        });

        modelBuilder.Entity<SubmittedAssignment>(entity =>
        {
            entity.ToTable("SubmittedAssignment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.Assignment).WithMany(p => p.SubmittedAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .HasConstraintName("FK_SubmittedAssignment_Assignment");
        });

        modelBuilder.Entity<SubmittedExercise>(entity =>
        {
            entity.ToTable("SubmittedExercise");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.SubmittedAssignment).WithMany(p => p.SubmittedExercises)
                .HasForeignKey(d => d.SubmittedAssignmentId)
                .HasConstraintName("FK_SubmittedExercise_SubmittedAssignment");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
