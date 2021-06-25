using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddCascadeDeleteOnReceiverJoinRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
