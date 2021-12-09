using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addDepartmentNameInCourseInof : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "CourseInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "CourseInfo");
        }
    }
}
