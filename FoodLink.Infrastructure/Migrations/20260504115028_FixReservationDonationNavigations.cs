using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixReservationDonationNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_DonationId",
                table: "Reservations",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_BusinessProfiles_BusinessProfileId",
                table: "Donations",
                column: "BusinessProfileId",
                principalTable: "BusinessProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Donations_DonationId",
                table: "Reservations",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_BusinessProfiles_BusinessProfileId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Donations_DonationId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_DonationId",
                table: "Reservations");
        }
    }
}
