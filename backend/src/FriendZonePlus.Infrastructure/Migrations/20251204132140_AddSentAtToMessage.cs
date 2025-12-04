using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendZonePlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSentAtToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Message",
                newName: "SentAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "Message",
                newName: "CreatedAt");
        }
    }
}
