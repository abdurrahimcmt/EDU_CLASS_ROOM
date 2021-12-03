using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class addmigrationupdteStudentInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfo_InfoBatch_InfoBatchId",
                table: "StudentInfo");

            migrationBuilder.DropIndex(
                name: "IX_StudentInfo_InfoBatchId",
                table: "StudentInfo");

            migrationBuilder.DropColumn(
                name: "InfoBatchId",
                table: "StudentInfo");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfo_BatchId",
                table: "StudentInfo",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfo_InfoBatch_BatchId",
                table: "StudentInfo",
                column: "BatchId",
                principalTable: "InfoBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentInfo_InfoBatch_BatchId",
                table: "StudentInfo");

            migrationBuilder.DropIndex(
                name: "IX_StudentInfo_BatchId",
                table: "StudentInfo");

            migrationBuilder.AddColumn<int>(
                name: "InfoBatchId",
                table: "StudentInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfo_InfoBatchId",
                table: "StudentInfo",
                column: "InfoBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentInfo_InfoBatch_InfoBatchId",
                table: "StudentInfo",
                column: "InfoBatchId",
                principalTable: "InfoBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
