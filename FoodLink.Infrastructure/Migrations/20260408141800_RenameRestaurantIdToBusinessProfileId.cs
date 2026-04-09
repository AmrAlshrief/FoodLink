using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameRestaurantIdToBusinessProfileId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestaurantId",
                table: "Donations",
                newName: "BusinessProfileId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "DonationItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "DonationItem");

            migrationBuilder.RenameColumn(
                name: "BusinessProfileId",
                table: "Donations",
                newName: "RestaurantId");
        }
    }
}
