using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addOnlineCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineClassInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    SemesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineClassInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineClassInfo_CourseInfo_CourseId",
                        column: x => x.CourseId,
                        principalTable: "CourseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnlineClassInfo_DepartmentInfo_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OnlineClassInfo_SemesterInfo_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "SemesterInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnlineClassDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    OnlineClassInfoId = table.Column<int>(type: "int", nullable: true),
                    ClassTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    BatchInfoId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudentInfoId = table.Column<int>(type: "int", nullable: true),
                    StudentRoll = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineClassDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineClassDetails_CourseInfo_BatchInfoId",
                        column: x => x.BatchInfoId,
                        principalTable: "CourseInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnlineClassDetails_OnlineClassInfo_OnlineClassInfoId",
                        column: x => x.OnlineClassInfoId,
                        principalTable: "OnlineClassInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnlineClassDetails_StudentInfo_StudentInfoId",
                        column: x => x.StudentInfoId,
                        principalTable: "StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassDetails_BatchInfoId",
                table: "OnlineClassDetails",
                column: "BatchInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassDetails_OnlineClassInfoId",
                table: "OnlineClassDetails",
                column: "OnlineClassInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassDetails_StudentInfoId",
                table: "OnlineClassDetails",
                column: "StudentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassInfo_CourseId",
                table: "OnlineClassInfo",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassInfo_DepartmentId",
                table: "OnlineClassInfo",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassInfo_SemesterId",
                table: "OnlineClassInfo",
                column: "SemesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineClassDetails");

            migrationBuilder.DropTable(
                name: "OnlineClassInfo");
        }
    }
}
