using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBDQ_API.Migrations
{
    /// <inheritdoc />
    public partial class v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discout",
                table: "TransactionDetails",
                newName: "Discount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "TransactionDetails",
                newName: "Discout");
        }
    }
}
