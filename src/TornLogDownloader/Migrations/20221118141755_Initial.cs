using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TornLogDownloader.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetimeoffset(3)", nullable: false),
                    Ended = table.Column<DateTimeOffset>(type: "datetimeoffset(3)", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunApiCalls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Started = table.Column<DateTimeOffset>(type: "datetimeoffset(3)", nullable: false),
                    Ended = table.Column<DateTimeOffset>(type: "datetimeoffset(3)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunApiCalls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunApiCalls_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunApiCallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LogTypeId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "smalldatetime", nullable: false, computedColumnSql: "DATEADD(S, [Timestamp], '1970-01-01')"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Data = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Params = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawLogs_RunApiCalls_RunApiCallId",
                        column: x => x.RunApiCallId,
                        principalTable: "RunApiCalls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawLogs_RunApiCallId",
                table: "RawLogs",
                column: "RunApiCallId");

            migrationBuilder.CreateIndex(
                name: "IX_RunApiCalls_RunId",
                table: "RunApiCalls",
                column: "RunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RawLogs");

            migrationBuilder.DropTable(
                name: "RunApiCalls");

            migrationBuilder.DropTable(
                name: "Runs");
        }
    }
}
