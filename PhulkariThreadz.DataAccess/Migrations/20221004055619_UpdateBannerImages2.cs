using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhulkariThreadz.DataAccess.Migrations
{
    public partial class UpdateBannerImages2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerSubTextFontColor",
                table: "BannerImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BannerTextFontColor",
                table: "BannerImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerSubTextFontColor",
                table: "BannerImages");

            migrationBuilder.DropColumn(
                name: "BannerTextFontColor",
                table: "BannerImages");
        }
    }
}
