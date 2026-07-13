using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class changedLessonPropName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageLessonId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions",
                column: "LanguageLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageLessonId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions",
                column: "LanguageLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }
    }
}
