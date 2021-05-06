using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightSE.Data.Migrations
{
    public partial class SearchQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchQuery",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(nullable: true),
                    Way = table.Column<int>(nullable: false),
                    Adults = table.Column<int>(nullable: false),
                    Children = table.Column<int>(nullable: false),
                    Infant = table.Column<int>(nullable: false),
                    Class = table.Column<string>(nullable: false),
                    Stopovers = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    From = table.Column<string>(nullable: false),
                    To = table.Column<string>(nullable: false),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    QueryDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchQuery", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchQuery");
        }
    }
}
