using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightSE.Data.Migrations
{
    public partial class AddedFlightID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlightID",
                table: "UserFlight",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightID",
                table: "UserFlight");
        }
    }
}
