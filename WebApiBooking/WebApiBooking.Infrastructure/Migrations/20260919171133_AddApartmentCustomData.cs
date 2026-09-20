using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApartmentCustomData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomData",
                table: "Apartments",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomData",
                table: "Apartments");
        }
    }
}
