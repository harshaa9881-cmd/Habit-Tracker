using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHabitRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HabitRecords",
                columns: table => new
                {
                    HabitRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HabitId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitRecords", x => x.HabitRecordId);
                    table.ForeignKey(
                        name: "FK_HabitRecords_Habits_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habits",
                        principalColumn: "HabitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HabitRecords_HabitId_CompletionDate",
                table: "HabitRecords",
                columns: new[] { "HabitId", "CompletionDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HabitRecords");
        }
    }
}
