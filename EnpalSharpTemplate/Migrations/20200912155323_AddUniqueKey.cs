using Microsoft.EntityFrameworkCore.Migrations;

namespace EnpalSharpTemplate.Migrations
{
    public partial class AddUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryData",
                table: "HistoryData");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "HistoryData",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "HistoryData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryData",
                table: "HistoryData",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoryData",
                table: "HistoryData");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "HistoryData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "HistoryData",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoryData",
                table: "HistoryData",
                column: "Id");
        }
    }
}
