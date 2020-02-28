using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class RemovedTeacherAndParentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_VCards_VCardId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VCardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VCardId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "VCards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VCards_OwnerId",
                table: "VCards",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VCards_AspNetUsers_OwnerId",
                table: "VCards",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VCards_AspNetUsers_OwnerId",
                table: "VCards");

            migrationBuilder.DropIndex(
                name: "IX_VCards_OwnerId",
                table: "VCards");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "VCards");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VCardId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VCardId",
                table: "AspNetUsers",
                column: "VCardId",
                unique: true,
                filter: "[VCardId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_VCards_VCardId",
                table: "AspNetUsers",
                column: "VCardId",
                principalTable: "VCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
