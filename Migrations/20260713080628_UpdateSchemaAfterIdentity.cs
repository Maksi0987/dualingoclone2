using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaAfterIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ActiveLangeageCourseId",
                table: "AspNetUsers",
                newName: "ActiveLanguageCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ActiveLangeageCourseId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ActiveLanguageCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLanguageCourseId",
                table: "AspNetUsers",
                column: "ActiveLanguageCourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLanguageCourseId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ActiveLanguageCourseId",
                table: "AspNetUsers",
                newName: "ActiveLangeageCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ActiveLanguageCourseId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ActiveLangeageCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers",
                column: "ActiveLangeageCourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
