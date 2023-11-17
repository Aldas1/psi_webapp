using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizAppApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuizToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Quizzes_QuizId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_QuizId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_QuizId",
                table: "Comments",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Quizzes_QuizId",
                table: "Comments",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
