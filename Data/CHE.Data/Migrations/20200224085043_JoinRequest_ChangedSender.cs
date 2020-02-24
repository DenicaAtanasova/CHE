using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class JoinRequest_ChangedSender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Cooperatives_CoopReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Cooperatives_CoopSenderId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ParentSenderId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_TeacherReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_CoopReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_CoopSenderId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_ParentSenderId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_TeacherReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "CoopReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "CoopSenderId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "ParentSenderId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "TeacherReceiverId",
                table: "JoinRequests");

            migrationBuilder.AddColumn<string>(
                name: "CooperativeId",
                table: "JoinRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "JoinRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "JoinRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_CooperativeId",
                table: "JoinRequests",
                column: "CooperativeId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_SenderId",
                table: "JoinRequests",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests",
                column: "CooperativeId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_SenderId",
                table: "JoinRequests",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Cooperatives_CooperativeId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_SenderId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_CooperativeId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_SenderId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "CooperativeId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "JoinRequests");

            migrationBuilder.AddColumn<string>(
                name: "CoopReceiverId",
                table: "JoinRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoopSenderId",
                table: "JoinRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentSenderId",
                table: "JoinRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherReceiverId",
                table: "JoinRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_CoopReceiverId",
                table: "JoinRequests",
                column: "CoopReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_CoopSenderId",
                table: "JoinRequests",
                column: "CoopSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_ParentSenderId",
                table: "JoinRequests",
                column: "ParentSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_TeacherReceiverId",
                table: "JoinRequests",
                column: "TeacherReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Cooperatives_CoopReceiverId",
                table: "JoinRequests",
                column: "CoopReceiverId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Cooperatives_CoopSenderId",
                table: "JoinRequests",
                column: "CoopSenderId",
                principalTable: "Cooperatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ParentSenderId",
                table: "JoinRequests",
                column: "ParentSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_TeacherReceiverId",
                table: "JoinRequests",
                column: "TeacherReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
