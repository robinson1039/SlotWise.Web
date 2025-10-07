using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlotWise.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Services");
        }
    }
}
