using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhulkariThreadz.DataAccess.Migrations
{
    public partial class UpdateTableBannerImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainImage",
                table: "BannerImages");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "BannerImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "BannerImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainImage",
                table: "BannerImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
