using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightSE.Data.Migrations
{
    public partial class LocationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Location");
        }
    }
}
