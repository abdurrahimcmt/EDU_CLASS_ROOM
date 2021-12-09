using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addCourseNameInEnrollmentInfoDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "EnrollmentDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "EnrollmentDetails");
        }
    }
}
