using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cygnus.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUserFromCartProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_Users_OwnerId",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_OwnerId",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CartProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "CartProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_OwnerId",
                table: "CartProducts",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_Users_OwnerId",
                table: "CartProducts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
