using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class ChangeCoopCreatorToAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperatives_AspNetUsers_CreatorId",
                table: "Cooperatives");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Cooperatives",
                newName: "AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Cooperatives_CreatorId",
                table: "Cooperatives",
                newName: "IX_Cooperatives_AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperatives_AspNetUsers_AdminId",
                table: "Cooperatives",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperatives_AspNetUsers_AdminId",
                table: "Cooperatives");

            migrationBuilder.RenameColumn(
                name: "AdminId",
                table: "Cooperatives",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Cooperatives_AdminId",
                table: "Cooperatives",
                newName: "IX_Cooperatives_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperatives_AspNetUsers_CreatorId",
                table: "Cooperatives",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
