using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddCascadeDeleteForImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Profiles_ProfileId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Profiles_ProfileId",
                table: "Images",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Profiles_ProfileId",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Profiles_ProfileId",
                table: "Images",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
