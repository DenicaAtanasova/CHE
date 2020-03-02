using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class FixedSpellingMistake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_RecieverId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_RecieverId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "RecieverId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Reviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReceiverId",
                table: "Reviews",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReceiverId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "RecieverId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RecieverId",
                table: "Reviews",
                column: "RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_RecieverId",
                table: "Reviews",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
