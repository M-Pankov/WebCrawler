using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCrawler.Model.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteCrawlResults",
                columns: table => new
                {
                    SiteCrawlResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    CrawlDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteCrawlResults", x => x.SiteCrawlResultId);
                });

            migrationBuilder.CreateTable(
                name: "UrlCrawlResults",
                columns: table => new
                {
                    CrawledUrlId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseTimeMs = table.Column<long>(type: "INTEGER", nullable: true),
                    UrlFoundLocation = table.Column<int>(type: "INTEGER", nullable: false),
                    SiteCrawlResultId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlCrawlResults", x => x.CrawledUrlId);
                    table.ForeignKey(
                        name: "FK_UrlCrawlResults_SiteCrawlResults_SiteCrawlResultId",
                        column: x => x.SiteCrawlResultId,
                        principalTable: "SiteCrawlResults",
                        principalColumn: "SiteCrawlResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlCrawlResults_SiteCrawlResultId",
                table: "UrlCrawlResults",
                column: "SiteCrawlResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlCrawlResults");

            migrationBuilder.DropTable(
                name: "SiteCrawlResults");
        }
    }
}
