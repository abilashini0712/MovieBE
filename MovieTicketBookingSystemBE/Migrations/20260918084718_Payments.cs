uusing Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class Payments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Payments_BookingId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegisterId",
                table: "Logins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "RegisterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Payments_BookingId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RegisterId",
                table: "Logins",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Movies_MovieId",
                table: "Bookings",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                principalTable: "Payments",
                principalColumn: "PaymentId");
        }
    }
}
