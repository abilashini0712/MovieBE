using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class bid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bookings",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bookings",
                newName: "id");
        }
    }
}
