using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment_Domain.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GradesheetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsPublihsed = table.Column<bool>(type: "bit", nullable: true),
                    GradingPublished = table.Column<bool>(type: "bit", nullable: true),
                    Inactive = table.Column<bool>(type: "bit", nullable: true),
                    Title = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nchar(500)", fixedLength: true, maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ErrorName = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    ErrorDescription = table.Column<string>(type: "nchar(1000)", fixedLength: true, maxLength: 1000, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Title = table.Column<string>(type: "nchar(100)", fixedLength: true, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nchar(500)", fixedLength: true, maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignment_AssignmentSet",
                        column: x => x.AssignmentSetId,
                        principalTable: "AssignmentSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradeSheet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeSheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeSheet_AssignmentSet",
                        column: x => x.AssignmentSetId,
                        principalTable: "AssignmentSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    SnapshotTakenAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentExercise_Assignment",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubmittedAssignment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SelfEvaluationSubmitted = table.Column<bool>(type: "bit", nullable: true),
                    AssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmittedAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmittedAssignment_Assignment",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentQuestion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignmentExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentQuestion_AssignmentExercise_AssignmentExerciseId",
                        column: x => x.AssignmentExerciseId,
                        principalTable: "AssignmentExercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentFeedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SelfEvaluationSubmitted = table.Column<bool>(type: "bit", nullable: true),
                    SubmittedAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentFeedback_SubmittedAssignment",
                        column: x => x.SubmittedAssignmentId,
                        principalTable: "SubmittedAssignment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubmittedExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedAssignmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmittedExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmittedExercise_SubmittedAssignment",
                        column: x => x.SubmittedAssignmentId,
                        principalTable: "SubmittedAssignment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseFeedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Feedbacktext = table.Column<string>(type: "nchar(1000)", fixedLength: true, maxLength: 1000, nullable: true),
                    PointsGiven = table.Column<int>(type: "int", nullable: true),
                    ErrorType = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseFeedback_ErrorType",
                        column: x => x.ErrorType,
                        principalTable: "ErrorType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExerciseFeedback_SubmittedExercise",
                        column: x => x.SubmittedExerciseId,
                        principalTable: "SubmittedExercise",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SelfEvaluation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: true),
                    SubmittedExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfEvaluation_SubmittedExercise",
                        column: x => x.SubmittedExerciseId,
                        principalTable: "SubmittedExercise",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_AssignmentSetId",
                table: "Assignment",
                column: "AssignmentSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentExercise_AssignmentId",
                table: "AssignmentExercise",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentFeedback_SubmittedAssignmentId",
                table: "AssignmentFeedback",
                column: "SubmittedAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestion_AssignmentExerciseId",
                table: "AssignmentQuestion",
                column: "AssignmentExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseFeedback_ErrorType",
                table: "ExerciseFeedback",
                column: "ErrorType");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseFeedback_SubmittedExerciseId",
                table: "ExerciseFeedback",
                column: "SubmittedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeSheet_AssignmentSetId",
                table: "GradeSheet",
                column: "AssignmentSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfEvaluation_SubmittedExerciseId",
                table: "SelfEvaluation",
                column: "SubmittedExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedAssignment_AssignmentId",
                table: "SubmittedAssignment",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmittedExercise_SubmittedAssignmentId",
                table: "SubmittedExercise",
                column: "SubmittedAssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentFeedback");

            migrationBuilder.DropTable(
                name: "AssignmentQuestion");

            migrationBuilder.DropTable(
                name: "ExerciseFeedback");

            migrationBuilder.DropTable(
                name: "GradeSheet");

            migrationBuilder.DropTable(
                name: "SelfEvaluation");

            migrationBuilder.DropTable(
                name: "AssignmentExercise");

            migrationBuilder.DropTable(
                name: "ErrorType");

            migrationBuilder.DropTable(
                name: "SubmittedExercise");

            migrationBuilder.DropTable(
                name: "SubmittedAssignment");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "AssignmentSet");
        }
    }
}
