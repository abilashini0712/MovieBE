using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookingSystemBE.Migrations
{
    /// <inheritdoc />
    public partial class newloging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisterId",
                table: "Logins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Logins_RegisterId",
                table: "Logins",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Registers_RegisterId",
                table: "Logins");

            migrationBuilder.DropIndex(
                name: "IX_Logins_RegisterId",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "RegisterId",
                table: "Logins");
        }
    }
}
