using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addNameInEnrollmentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmnetName",
                table: "EnrollmentInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SemesterName",
                table: "EnrollmentInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "EnrollmentInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmnetName",
                table: "EnrollmentInfo");

            migrationBuilder.DropColumn(
                name: "SemesterName",
                table: "EnrollmentInfo");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "EnrollmentInfo");
        }
    }
}
