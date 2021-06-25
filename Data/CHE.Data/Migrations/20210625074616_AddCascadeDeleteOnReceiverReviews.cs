using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddCascadeDeleteOnReceiverReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
