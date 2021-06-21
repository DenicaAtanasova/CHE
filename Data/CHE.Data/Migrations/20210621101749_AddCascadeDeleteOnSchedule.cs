using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddCascadeDeleteOnSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Cooperatives_CooperativeId",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Cooperatives_CooperativeId",
                table: "Schedules",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Cooperatives_CooperativeId",
                table: "Schedules");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Cooperatives_CooperativeId",
                table: "Schedules",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
