using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendZonePlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusFromFollows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Follows");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Follows",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
