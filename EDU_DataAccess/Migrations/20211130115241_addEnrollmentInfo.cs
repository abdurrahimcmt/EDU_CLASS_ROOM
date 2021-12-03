using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addEnrollmentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrollmentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentInfo_DepartmentInfo_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EnrollmentInfo_SemesterInfo_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "SemesterInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrollmentInfo_StudentInfo_StudentId",
                        column: x => x.StudentId,
                        principalTable: "StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentInfo_DepartmentId",
                table: "EnrollmentInfo",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentInfo_SemesterId",
                table: "EnrollmentInfo",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentInfo_StudentId",
                table: "EnrollmentInfo",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentInfo");
        }
    }
}
