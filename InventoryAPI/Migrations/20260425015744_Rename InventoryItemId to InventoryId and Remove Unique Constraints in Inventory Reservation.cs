using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameInventoryItemIdtoInventoryIdandRemoveUniqueConstraintsinInventoryReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Inventories_InventoryItemId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_InventoryItemId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "Reservations",
                newName: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_InventoryId",
                table: "Reservations",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Inventories_InventoryId",
                table: "Reservations",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Inventories_InventoryId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_InventoryId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Reservations",
                newName: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_InventoryItemId",
                table: "Reservations",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Inventories_InventoryItemId",
                table: "Reservations",
                column: "InventoryItemId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
