using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBDQ_API.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetail_Products_ProductId",
                table: "TransactionDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetail_Transactions_TransactionId",
                table: "TransactionDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionDetail",
                table: "TransactionDetail");

            migrationBuilder.RenameTable(
                name: "TransactionDetail",
                newName: "TransactionDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetail_TransactionId",
                table: "TransactionDetails",
                newName: "IX_TransactionDetails_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetail_ProductId",
                table: "TransactionDetails",
                newName: "IX_TransactionDetails_ProductId");

            migrationBuilder.AddColumn<double>(
                name: "Discout",
                table: "TransactionDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionDetails",
                table: "TransactionDetails",
                column: "TransactionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Products_ProductId",
                table: "TransactionDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Products_ProductId",
                table: "TransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionDetails_Transactions_TransactionId",
                table: "TransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionDetails",
                table: "TransactionDetails");

            migrationBuilder.DropColumn(
                name: "Discout",
                table: "TransactionDetails");

            migrationBuilder.RenameTable(
                name: "TransactionDetails",
                newName: "TransactionDetail");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetails_TransactionId",
                table: "TransactionDetail",
                newName: "IX_TransactionDetail_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionDetails_ProductId",
                table: "TransactionDetail",
                newName: "IX_TransactionDetail_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionDetail",
                table: "TransactionDetail",
                column: "TransactionDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetail_Products_ProductId",
                table: "TransactionDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionDetail_Transactions_TransactionId",
                table: "TransactionDetail",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
