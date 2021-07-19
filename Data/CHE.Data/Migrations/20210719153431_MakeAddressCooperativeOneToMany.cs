using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class MakeAddressCooperativeOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cooperatives_AddressId",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Cooperatives_AddressId",
                table: "Cooperatives",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cooperatives_AddressId",
                table: "Cooperatives");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cooperatives_AddressId",
                table: "Cooperatives",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");
        }
    }
}
