using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendZonePlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add column as nullable first
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Message",
                type: "TEXT",
                nullable: true);

            // Set CreatedAt to current UTC time for all existing rows
            migrationBuilder.Sql(
                "UPDATE Message SET CreatedAt = datetime('now') WHERE CreatedAt IS NULL");

            // Make column non-nullable
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Message",
                type: "TEXT",
                nullable: false,
                defaultValue: DateTime.UtcNow);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Message");
        }
    }
}
