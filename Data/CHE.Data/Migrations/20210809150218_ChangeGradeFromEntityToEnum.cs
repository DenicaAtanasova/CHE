using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class ChangeGradeFromEntityToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cooperatives_Grades_GradeId",
                table: "Cooperatives");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Cooperatives_GradeId",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Cooperatives");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Cooperatives",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Cooperatives");

            migrationBuilder.AddColumn<string>(
                name: "GradeId",
                table: "Cooperatives",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumValue = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cooperatives_GradeId",
                table: "Cooperatives",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cooperatives_Grades_GradeId",
                table: "Cooperatives",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
