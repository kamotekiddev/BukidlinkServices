using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintInReservationForOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations",
                column: "OrderId",
                unique: true);
        }
    }
}
