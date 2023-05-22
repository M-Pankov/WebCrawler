using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCrawler.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrawledSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CrawlDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawledSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrawledSiteUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponseTimeMs = table.Column<long>(type: "bigint", nullable: true),
                    UrlFoundLocation = table.Column<int>(type: "int", nullable: false),
                    CrawledSiteId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawledSiteUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrawledSiteUrls_CrawledSites_CrawledSiteId",
                        column: x => x.CrawledSiteId,
                        principalTable: "CrawledSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CrawledSiteUrls_CrawledSiteId",
                table: "CrawledSiteUrls",
                column: "CrawledSiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrawledSiteUrls");

            migrationBuilder.DropTable(
                name: "CrawledSites");
        }
    }
}
