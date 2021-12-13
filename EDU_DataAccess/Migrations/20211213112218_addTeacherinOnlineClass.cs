using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addTeacherinOnlineClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "OnlineClassInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "OnlineClassInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnlineClassInfo_TeacherId",
                table: "OnlineClassInfo",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_OnlineClassInfo_TeacherInfo_TeacherId",
                table: "OnlineClassInfo",
                column: "TeacherId",
                principalTable: "TeacherInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnlineClassInfo_TeacherInfo_TeacherId",
                table: "OnlineClassInfo");

            migrationBuilder.DropIndex(
                name: "IX_OnlineClassInfo_TeacherId",
                table: "OnlineClassInfo");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "OnlineClassInfo");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "OnlineClassInfo");
        }
    }
}
