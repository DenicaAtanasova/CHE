using Microsoft.EntityFrameworkCore.Migrations;

namespace CHE.Data.Migrations
{
    public partial class AddedEducationLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EducationLevel",
                table: "Portfolios",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationLevel",
                table: "Portfolios");
        }
    }
}
