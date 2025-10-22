using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlotWise.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddImgServiceToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgService",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgService",
                table: "Services");
        }
    }
}
