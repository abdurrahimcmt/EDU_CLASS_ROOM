using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addEnrollmentDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrollmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentInfoId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CourseInfoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentDetails_CourseInfo_CourseInfoId",
                        column: x => x.CourseInfoId,
                        principalTable: "CourseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnrollmentDetails_EnrollmentInfo_EnrollmentInfoId",
                        column: x => x.EnrollmentInfoId,
                        principalTable: "EnrollmentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentDetails_CourseInfoId",
                table: "EnrollmentDetails",
                column: "CourseInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentDetails_EnrollmentInfoId",
                table: "EnrollmentDetails",
                column: "EnrollmentInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentDetails");
        }
    }
}
