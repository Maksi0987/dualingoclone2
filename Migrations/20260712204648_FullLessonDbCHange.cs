using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Languio.Migrations
{
    /// <inheritdoc />
    public partial class FullLessonDbCHange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_VocabularyWords_VocabularyWordId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "UserWordProgresses");

            migrationBuilder.DropTable(
                name: "WordTranslations");

            migrationBuilder.DropTable(
                name: "VocabularyWords");

            migrationBuilder.DropIndex(
                name: "IX_Groups_VocabularyWordId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "OptinsRaw",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "VocabularyWordId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "Questions",
                newName: "PromptText");

            migrationBuilder.RenameColumn(
                name: "Difficulty",
                table: "Questions",
                newName: "LessonId");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageLessonId",
                table: "Questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    LanguageQuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_LanguageQuestionId",
                        column: x => x.LanguageQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_LanguageQuestionId",
                table: "AnswerOptions",
                column: "LanguageQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions",
                column: "LanguageLessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.RenameColumn(
                name: "PromptText",
                table: "Questions",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "Questions",
                newName: "Difficulty");

            migrationBuilder.AlterColumn<int>(
                name: "LanguageLessonId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OptinsRaw",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VocabularyWordId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VocabularyWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForeignWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocabularyWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserWordProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VocabularyWordId = table.Column<int>(type: "int", nullable: false),
                    CorrectStreak = table.Column<int>(type: "int", nullable: false),
                    LastReviewed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextReviewDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WordLevel = table.Column<int>(type: "int", nullable: false),
                    WrongAnswersCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWordProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWordProgresses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWordProgresses_VocabularyWords_VocabularyWordId",
                        column: x => x.VocabularyWordId,
                        principalTable: "VocabularyWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VocabularyWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordTranslations_VocabularyWords_VocabularyWordId",
                        column: x => x.VocabularyWordId,
                        principalTable: "VocabularyWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_VocabularyWordId",
                table: "Groups",
                column: "VocabularyWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordProgresses_ApplicationUserId",
                table: "UserWordProgresses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWordProgresses_VocabularyWordId",
                table: "UserWordProgresses",
                column: "VocabularyWordId");

            migrationBuilder.CreateIndex(
                name: "IX_WordTranslations_VocabularyWordId",
                table: "WordTranslations",
                column: "VocabularyWordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_VocabularyWords_VocabularyWordId",
                table: "Groups",
                column: "VocabularyWordId",
                principalTable: "VocabularyWords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lessons_LanguageLessonId",
                table: "Questions",
                column: "LanguageLessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
