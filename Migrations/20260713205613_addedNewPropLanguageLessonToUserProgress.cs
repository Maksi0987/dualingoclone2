using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class addedNewPropLanguageLessonToUserProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentLessonId",
                table: "UserProgresses",
                newName: "LanguageLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProgresses_LanguageLessonId",
                table: "UserProgresses",
                column: "LanguageLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Lessons_LanguageLessonId",
                table: "UserProgresses",
                column: "LanguageLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Lessons_LanguageLessonId",
                table: "UserProgresses");

            migrationBuilder.DropIndex(
                name: "IX_UserProgresses_LanguageLessonId",
                table: "UserProgresses");

            migrationBuilder.RenameColumn(
                name: "LanguageLessonId",
                table: "UserProgresses",
                newName: "CurrentLessonId");
        }
    }
}
