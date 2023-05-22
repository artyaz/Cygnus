using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cygnus.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaLatest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_OwnerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OwnerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OwnerId",
                table: "Orders",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_OwnerId",
                table: "Orders",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
