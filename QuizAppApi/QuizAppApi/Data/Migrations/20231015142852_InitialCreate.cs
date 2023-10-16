using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizAppApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSubmitters = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectOptionName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceOptions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MultipleChoiceQuestionId = table.Column<int>(type: "int", nullable: false),
                    Correct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceOptions", x => new { x.Name, x.MultipleChoiceQuestionId });
                    table.ForeignKey(
                        name: "FK_MultipleChoiceOptions_Questions_MultipleChoiceQuestionId",
                        column: x => x.MultipleChoiceQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleChoiceOptions",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SingleChoiceQuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleChoiceOptions", x => new { x.Name, x.SingleChoiceQuestionId });
                    table.ForeignKey(
                        name: "FK_SingleChoiceOptions_Questions_SingleChoiceQuestionId",
                        column: x => x.SingleChoiceQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceOptions_MultipleChoiceQuestionId",
                table: "MultipleChoiceOptions",
                column: "MultipleChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChoiceOptions_SingleChoiceQuestionId",
                table: "SingleChoiceOptions",
                column: "SingleChoiceQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultipleChoiceOptions");

            migrationBuilder.DropTable(
                name: "SingleChoiceOptions");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
