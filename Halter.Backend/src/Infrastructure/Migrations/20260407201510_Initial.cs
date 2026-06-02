using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MuscleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MuscleGroupId = table.Column<int>(type: "integer", nullable: false),
                    TimeConstraint = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_MuscleGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RoutineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Routines_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    ExerciseType = table.Column<string>(type: "text", nullable: false),
                    Values = table.Column<int[]>(type: "integer[]", nullable: false),
                    IsometricHoldSeconds = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExecutedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    SetsCompleted = table.Column<int>(type: "integer", nullable: false),
                    RepsCompleted = table.Column<int>(type: "integer", nullable: false),
                    MaxWeight = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseLogs_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseLogs_WorkoutLogs_WorkoutLogId",
                        column: x => x.WorkoutLogId,
                        principalTable: "WorkoutLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MuscleGroups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Peitoral" },
                    { 2, "Bíceps" },
                    { 3, "Tríceps" },
                    { 4, "Dorsal" },
                    { 5, "Abdômen" },
                    { 6, "Quadríceps" },
                    { 7, "Posterior de Coxa" },
                    { 8, "Glúteo" },
                    { 9, "Lombar" },
                    { 10, "Antebraço" },
                    { 11, "Ombro" },
                    { 12, "Panturrilha" },
                    { 13, "Trapézio" },
                    { 14, "Cardio" }
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "MuscleGroupId", "Name", "TimeConstraint" },
                values: new object[,]
                {
                    { 1, 1, "Supino reto", "Forbidden" },
                    { 2, 1, "Supino inclinado", "Forbidden" },
                    { 3, 1, "Supino declinado", "Forbidden" },
                    { 4, 1, "Crucifixo reto", "Forbidden" },
                    { 5, 1, "Cruxifixo inclinado", "Forbidden" },
                    { 6, 1, "Cross over", "Forbidden" },
                    { 7, 1, "Peck deck", "Forbidden" },
                    { 8, 1, "Flexão de braço", "Forbidden" },
                    { 9, 1, "Pullover", "Forbidden" },
                    { 10, 1, "Paralela", "Forbidden" },
                    { 11, 2, "Rosca direta", "Forbidden" },
                    { 12, 2, "Rosca alternada", "Forbidden" },
                    { 13, 2, "Rosca martelo", "Forbidden" },
                    { 14, 2, "Rosca concentrada", "Forbidden" },
                    { 15, 2, "Rosca scott", "Forbidden" },
                    { 16, 2, "Rosca 21", "Forbidden" },
                    { 17, 3, "Tríceps pulley", "Forbidden" },
                    { 18, 3, "Tríceps corda", "Forbidden" },
                    { 19, 3, "Tríceps francês", "Forbidden" },
                    { 20, 3, "Tríceps testa", "Forbidden" },
                    { 21, 3, "Mergulho", "Forbidden" },
                    { 22, 3, "Extensão de tríceps", "Forbidden" },
                    { 23, 3, "Kickback", "Forbidden" },
                    { 24, 4, "Puxada frontal", "Forbidden" },
                    { 25, 4, "Puxada posterior", "Forbidden" },
                    { 26, 4, "Remada curvada", "Forbidden" },
                    { 27, 4, "Remada unilateral", "Forbidden" },
                    { 28, 4, "Remada cavalinho", "Forbidden" },
                    { 29, 4, "Barra fixa", "Forbidden" },
                    { 30, 4, "Serrote", "Forbidden" },
                    { 31, 4, "Remada máquina", "Forbidden" },
                    { 32, 11, "Desenvolvimento com barra", "Forbidden" },
                    { 33, 11, "Desenvolvimento com halteres", "Forbidden" },
                    { 34, 11, "Elevação lateral", "Forbidden" },
                    { 35, 11, "Elevação frontal", "Forbidden" },
                    { 36, 11, "Encolhimento", "Forbidden" },
                    { 37, 11, "Cruxifixo invertido", "Forbidden" },
                    { 38, 11, "Arnold press", "Forbidden" },
                    { 39, 5, "Abdominal crunch", "Forbidden" },
                    { 40, 5, "Abdominal infra", "Forbidden" },
                    { 41, 5, "Prancha", "Forbidden" },
                    { 42, 5, "Abdominal oblíquo", "Forbidden" },
                    { 43, 5, "Abdominal máquina", "Forbidden" },
                    { 44, 5, "Roda abdominal", "Forbidden" },
                    { 45, 5, "Elevação de pernas", "Forbidden" },
                    { 46, 6, "Agachamento livre", "Forbidden" },
                    { 47, 6, "Agachamento hack", "Forbidden" },
                    { 48, 6, "Leg press", "Forbidden" },
                    { 49, 6, "Cadeira extensora", "Forbidden" },
                    { 50, 6, "Avanço", "Forbidden" },
                    { 51, 6, "Agachamento búlgaro", "Forbidden" },
                    { 52, 6, "Agachamento sumô", "Forbidden" },
                    { 53, 7, "Mesa flexora", "Forbidden" },
                    { 54, 7, "Cadeira flexora", "Forbidden" },
                    { 55, 7, "Stiff", "Forbidden" },
                    { 56, 7, "Levantamento terra", "Forbidden" },
                    { 57, 7, "Agachamento romeno", "Forbidden" },
                    { 58, 8, "Elevação pélvica", "Forbidden" },
                    { 59, 8, "Abdução de quadril", "Forbidden" },
                    { 60, 8, "Kick back glúteo", "Forbidden" },
                    { 61, 12, "Panturrilha em pé", "Forbidden" },
                    { 62, 12, "Panturrilha sentado", "Forbidden" },
                    { 63, 12, "Panturrilha leg press", "Forbidden" },
                    { 64, 9, "Bom dia", "Forbidden" },
                    { 65, 9, "Hiperextensão", "Forbidden" },
                    { 66, 13, "Encolhimento com barra", "Forbidden" },
                    { 67, 13, "Encolhimento com halteres", "Forbidden" },
                    { 68, 13, "Remada alta", "Forbidden" },
                    { 69, 10, "Rosca de punho", "Forbidden" },
                    { 70, 10, "Rosca de punho invertida", "Forbidden" },
                    { 71, 10, "Farmer walk", "Forbidden" },
                    { 72, 10, "Rotação de punho", "Forbidden" },
                    { 73, 7, "Agachamento smith", "Forbidden" },
                    { 74, 7, "Flexão nórdica", "Forbidden" },
                    { 75, 10, "Rosca inversa", "Forbidden" },
                    { 76, 8, "Afundo", "Forbidden" },
                    { 77, 4, "Puxada neutra", "Forbidden" },
                    { 78, 11, "Face pull", "Forbidden" },
                    { 79, 7, "Stiff unilateral", "Forbidden" },
                    { 80, 14, "Polichinelo", "Forbidden" },
                    { 81, 14, "Corrida", "Forbidden" },
                    { 82, 14, "Caminhada", "Forbidden" },
                    { 83, 14, "Ciclismo", "Forbidden" },
                    { 84, 14, "Corda", "Forbidden" },
                    { 85, 14, "Elíptico", "Forbidden" },
                    { 87, 14, "Burpee", "Forbidden" },
                    { 88, 14, "Mountain climber", "Forbidden" },
                    { 89, 5, "Prancha lateral", "Forbidden" },
                    { 90, 5, "Prancha com elevação de braço", "Forbidden" },
                    { 91, 6, "Isometria de agachamento", "Forbidden" },
                    { 92, 6, "Isometria de avanço", "Forbidden" },
                    { 93, 14, "Escada", "Forbidden" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_ExerciseId",
                table: "ExerciseLogs",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutLogId",
                table: "ExerciseLogs",
                column: "WorkoutLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_MuscleGroupId",
                table: "Exercises",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_Name",
                table: "Exercises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routines_UserId",
                table: "Routines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_WorkoutId",
                table: "WorkoutLogs",
                column: "WorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_RoutineId",
                table: "Workouts",
                column: "RoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseLogs");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "WorkoutLogs");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "MuscleGroups");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
