using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddCascadeDeleteForProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
