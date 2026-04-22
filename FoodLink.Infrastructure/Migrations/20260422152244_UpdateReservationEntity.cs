using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodLink.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DonationId",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Reservations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "ReservationItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ReservationItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NoShowCount",
                table: "CharityProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ReservationItem");

            migrationBuilder.DropColumn(
                name: "NoShowCount",
                table: "CharityProfiles");
        }
    }
}
