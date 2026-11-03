using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class removeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
