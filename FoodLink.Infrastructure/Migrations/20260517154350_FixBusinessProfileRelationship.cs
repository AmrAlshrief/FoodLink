using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBusinessProfileRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_CharityId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_BusinessProfileId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_ExpiryDate",
                table: "Donations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CharityId_Status",
                table: "Reservations",
                columns: new[] { "CharityId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Status",
                table: "Reservations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_BusinessProfileId_Status",
                table: "Donations",
                columns: new[] { "BusinessProfileId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ExpiryDate_Status",
                table: "Donations",
                columns: new[] { "ExpiryDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CharityProfiles_UserId",
                table: "CharityProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CharityProfiles_Users_UserId",
                table: "CharityProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharityProfiles_Users_UserId",
                table: "CharityProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CharityId_Status",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_Status",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_BusinessProfileId_Status",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_ExpiryDate_Status",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_CharityProfiles_UserId",
                table: "CharityProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CharityId",
                table: "Reservations",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_BusinessProfileId",
                table: "Donations",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ExpiryDate",
                table: "Donations",
                column: "ExpiryDate");
        }
    }
}
