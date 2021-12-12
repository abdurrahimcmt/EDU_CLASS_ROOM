using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addStudentRoleInEnrollmentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentRoll",
                table: "EnrollmentInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentRoll",
                table: "EnrollmentInfo");
        }
    }
}
