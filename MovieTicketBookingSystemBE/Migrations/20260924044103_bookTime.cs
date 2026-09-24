using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class bookTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Bookings");
        }
    }
}
