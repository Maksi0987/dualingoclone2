using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class FixAnswerOptionForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Questions_LanguageQuestionId",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageQuestionId",
                table: "AnswerOptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Questions_LanguageQuestionId",
                table: "AnswerOptions",
                column: "LanguageQuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Questions_LanguageQuestionId",
                table: "AnswerOptions");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageQuestionId",
                table: "AnswerOptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "AnswerOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Questions_LanguageQuestionId",
                table: "AnswerOptions",
                column: "LanguageQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
