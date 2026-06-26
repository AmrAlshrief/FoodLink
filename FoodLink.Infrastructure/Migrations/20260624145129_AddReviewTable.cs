using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReservationId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Reviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReservationId_ReviewerId_Type",
                table: "Reviews",
                columns: new[] { "ReservationId", "ReviewerId", "Type" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ReservationId_ReviewerId_Type",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReservationId",
                table: "Reviews",
                column: "ReservationId",
                unique: true);
        }
    }
}
