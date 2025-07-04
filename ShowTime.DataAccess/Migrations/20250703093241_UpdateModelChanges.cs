using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowTime.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Festivals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Festivals_UserId",
                table: "Festivals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Festivals_Users_UserId",
                table: "Festivals",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Festivals_Users_UserId",
                table: "Festivals");

            migrationBuilder.DropIndex(
                name: "IX_Festivals_UserId",
                table: "Festivals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Festivals");
        }
    }
}
