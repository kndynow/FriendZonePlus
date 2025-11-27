using FriendZonePlus.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FriendZonePlus.Infrastructure.Data;

public class FriendZonePlusContext : DbContext
{

  public FriendZonePlusContext(DbContextOptions<FriendZonePlusContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }
  public DbSet<WallPost> WallPosts { get; set; }

  // Configurate relations between models
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure Post -> Author relation
    modelBuilder.Entity<WallPost>()
    .HasOne(p => p.Author)
    .WithMany()
    .HasForeignKey(p => p.AuthorId)
    .OnDelete(DeleteBehavior.Restrict);

    // Configure Post -> TargetUser relation
    modelBuilder.Entity<WallPost>()
    .HasOne(p => p.TargetUser)
    .WithMany()
    .HasForeignKey(p => p.TargetUserId)
    .OnDelete(DeleteBehavior.Restrict);
  }

}
