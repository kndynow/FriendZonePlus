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
  public DbSet<Follow> Follows { get; set; }
  public DbSet<Message> Messages { get; set; }

  // Configurate relations between models
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure table names to singular
    modelBuilder.Entity<User>().ToTable("User");
    modelBuilder.Entity<WallPost>().ToTable("WallPost");
    modelBuilder.Entity<Follow>().ToTable("Follow");
    modelBuilder.Entity<Message>().ToTable("Message");

    // Set composite key for Follow
    modelBuilder.Entity<Follow>(entity =>
    {
      entity.HasKey(f => new { f.FollowerId, f.FollowedUserId });

      entity.HasOne(f => f.Follower)
      .WithMany(u => u.Followers)
      .HasForeignKey(f => f.FollowerId)
      .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(f => f.Follower)
      .WithMany(u => u.Following)
      .HasForeignKey(f => f.FollowedUserId)
      .OnDelete(DeleteBehavior.Restrict);

    });

    // Configure WallPost -> Author relation
    modelBuilder.Entity<WallPost>(entity =>
    {
      // Author -> AuthoredPosts relation
      entity.HasOne(wp => wp.Author)
      .WithMany(u => u.AuthoredPosts)
      .HasForeignKey(wp => wp.AuthorId)
      .OnDelete(DeleteBehavior.Restrict);

      // TargetUser -> WallPosts relation
      entity.HasOne(wp => wp.TargetUser)
      .WithMany(u => u.WallPosts)
      .HasForeignKey(wp => wp.TargetUserId)
      .OnDelete(DeleteBehavior.Restrict);
    });

    // // Configure Post -> Author relation
    // modelBuilder.Entity<WallPost>()
    // .HasOne(p => p.Author)
    // .WithMany()
    // .HasForeignKey(p => p.AuthorId)
    // .OnDelete(DeleteBehavior.Restrict);

    // // Configure Post -> TargetUser relation
    // modelBuilder.Entity<WallPost>()
    // .HasOne(p => p.TargetUser)
    // .WithMany()
    // .HasForeignKey(p => p.TargetUserId)
    // .OnDelete(DeleteBehavior.Restrict);

    // // Configure Follow -> FollowedUser relation
    // modelBuilder.Entity<Follow>()
    // .HasOne(f => f.FollowedUser)
    // .WithMany()
    // .HasForeignKey(f => f.FollowedUserId)
    // .OnDelete(DeleteBehavior.Restrict);

    // // Configure Follow -> Follower relation
    // modelBuilder.Entity<Follow>()
    // .HasOne(f => f.Follower)
    // .WithMany()
    // .HasForeignKey(f => f.FollowerId)
    // .OnDelete(DeleteBehavior.Restrict);
  }

}
