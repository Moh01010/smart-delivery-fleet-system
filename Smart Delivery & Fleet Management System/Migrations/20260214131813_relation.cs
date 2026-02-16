using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Delivery___Fleet_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Drivers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Users_UserId",
                table: "Drivers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Users_UserId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_UserId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Drivers");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
