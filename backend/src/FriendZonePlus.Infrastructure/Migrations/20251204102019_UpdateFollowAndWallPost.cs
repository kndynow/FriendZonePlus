using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendZonePlus.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFollowAndWallPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Users -> User
            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            // WallPosts -> WallPost
            migrationBuilder.RenameTable(
                name: "WallPosts",
                newName: "WallPost");

            // Messages -> Message
            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Message -> Messages
            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            // WallPost -> WallPosts
            migrationBuilder.RenameTable(
                name: "WallPost",
                newName: "WallPosts");

            // User -> Users
            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");
        }
    }
}
