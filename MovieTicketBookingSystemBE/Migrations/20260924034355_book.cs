using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class book : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RegisterId",
                table: "Bookings",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Registers_RegisterId",
                table: "Bookings",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Registers_RegisterId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RegisterId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "Bookings");
        }
    }
}
