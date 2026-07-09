using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class someChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_LanguageGroupId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_LanguageLessonGroupId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_LanguageGroupId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LanguageGroupId",
                table: "Lessons");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_LanguageLessonGroupId",
                table: "Lessons",
                column: "LanguageLessonGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_LanguageLessonGroupId",
                table: "Lessons");

            migrationBuilder.AddColumn<int>(
                name: "LanguageGroupId",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LanguageGroupId",
                table: "Lessons",
                column: "LanguageGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_LanguageGroupId",
                table: "Lessons",
                column: "LanguageGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_LanguageLessonGroupId",
                table: "Lessons",
                column: "LanguageLessonGroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
