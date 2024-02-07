using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhulkariThreadz.DataAccess.Migrations
{
    public partial class AddBinaryInagesToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BannerImages",
                columns: table => new
                {
                    BannerImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageURl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerSubText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerImages", x => x.BannerImageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannerImages");
        }
    }
}
