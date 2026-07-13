using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerOptionsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_AspNetUsers_UserId",
                table: "Progresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Progresses_Courses_LanguageCourseId",
                table: "Progresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Progresses",
                table: "Progresses");

            migrationBuilder.RenameTable(
                name: "Progresses",
                newName: "UserProgresses");

            migrationBuilder.RenameIndex(
                name: "IX_Progresses_UserId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Progresses_LanguageCourseId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_LanguageCourseId");

            migrationBuilder.AlterColumn<int>(
                name: "ActiveLangeageCourseId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers",
                column: "ActiveLangeageCourseId",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_AspNetUsers_UserId",
                table: "UserProgresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Courses_LanguageCourseId",
                table: "UserProgresses",
                column: "LanguageCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_AspNetUsers_UserId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Courses_LanguageCourseId",
                table: "UserProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses");

            migrationBuilder.RenameTable(
                name: "UserProgresses",
                newName: "Progresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_UserId",
                table: "Progresses",
                newName: "IX_Progresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_LanguageCourseId",
                table: "Progresses",
                newName: "IX_Progresses_LanguageCourseId");

            migrationBuilder.AlterColumn<int>(
                name: "ActiveLangeageCourseId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Progresses",
                table: "Progresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_ActiveLangeageCourseId",
                table: "AspNetUsers",
                column: "ActiveLangeageCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_AspNetUsers_UserId",
                table: "Progresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Progresses_Courses_LanguageCourseId",
                table: "Progresses",
                column: "LanguageCourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
