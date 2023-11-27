using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizAppApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserStoreQuizzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Quizzes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Username",
                table: "Quizzes",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_Username",
                table: "Quizzes",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_Username",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_Username",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Quizzes");
        }
    }
}
