using Microsoft.EntityFrameworkCore.Migrations;

namespace EDU_DataAccess.Migrations
{
    public partial class ChangeIntoInquiryDetailTypeIntToStringFieldOfApplicationUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryHeader_AspNetUsers_ApplicationUserId1",
                table: "InquiryHeader");

            migrationBuilder.DropIndex(
                name: "IX_InquiryHeader_ApplicationUserId1",
                table: "InquiryHeader");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "InquiryHeader");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "InquiryHeader",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_InquiryHeader_ApplicationUserId",
                table: "InquiryHeader",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryHeader_AspNetUsers_ApplicationUserId",
                table: "InquiryHeader",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InquiryHeader_AspNetUsers_ApplicationUserId",
                table: "InquiryHeader");

            migrationBuilder.DropIndex(
                name: "IX_InquiryHeader_ApplicationUserId",
                table: "InquiryHeader");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "InquiryHeader",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "InquiryHeader",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InquiryHeader_ApplicationUserId1",
                table: "InquiryHeader",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_InquiryHeader_AspNetUsers_ApplicationUserId1",
                table: "InquiryHeader",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
