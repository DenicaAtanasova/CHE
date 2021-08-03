using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddParentAndTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperatives_AspNetUsers_AdminId",
                table: "Cooperatives");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_AspNetUsers_SenderId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_SenderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_AspNetUsers_TeacherId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "UserCooperatives");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TeacherId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_ReceiverId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "JoinRequests");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Schedules",
                newName: "OwnerId");

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParentsCooperatives",
                columns: table => new
                {
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CooperativeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentsCooperatives", x => new { x.ParentId, x.CooperativeId });
                    table.ForeignKey(
                        name: "FK_ParentsCooperatives_Cooperatives_CooperativeId",
                        column: x => x.CooperativeId,
                        principalTable: "Cooperatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsCooperatives_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_OwnerId",
                table: "Schedules",
                column: "OwnerId",
                unique: true,
                filter: "[OwnerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_UserId",
                table: "Parents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsCooperatives_CooperativeId",
                table: "ParentsCooperatives",
                column: "CooperativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperatives_Parents_AdminId",
                table: "Cooperatives",
                column: "AdminId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Parents_SenderId",
                table: "JoinRequests",
                column: "SenderId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Teachers_OwnerId",
                table: "Profiles",
                column: "OwnerId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Parents_SenderId",
                table: "Reviews",
                column: "SenderId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Teachers_ReceiverId",
                table: "Reviews",
                column: "ReceiverId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Teachers_OwnerId",
                table: "Schedules",
                column: "OwnerId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperatives_Parents_AdminId",
                table: "Cooperatives");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Parents_SenderId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Teachers_OwnerId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Parents_SenderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Teachers_ReceiverId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Teachers_OwnerId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "ParentsCooperatives");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_OwnerId",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Schedules",
                newName: "TeacherId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "JoinRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCooperatives",
                columns: table => new
                {
                    CheUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CooperativeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCooperatives", x => new { x.CheUserId, x.CooperativeId });
                    table.ForeignKey(
                        name: "FK_UserCooperatives_AspNetUsers_CheUserId",
                        column: x => x.CheUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCooperatives_Cooperatives_CooperativeId",
                        column: x => x.CooperativeId,
                        principalTable: "Cooperatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                unique: true,
                filter: "[TeacherId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCooperatives_CooperativeId",
                table: "UserCooperatives",
                column: "CooperativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperatives_AspNetUsers_AdminId",
                table: "Cooperatives",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_ReceiverId",
                table: "JoinRequests",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_AspNetUsers_SenderId",
                table: "JoinRequests",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_AspNetUsers_OwnerId",
                table: "Profiles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ReceiverId",
                table: "Reviews",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_SenderId",
                table: "Reviews",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_AspNetUsers_TeacherId",
                table: "Schedules",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
