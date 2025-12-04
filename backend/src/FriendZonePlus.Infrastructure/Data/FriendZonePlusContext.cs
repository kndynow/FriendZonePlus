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

    // Configure Follow -> FollowedUser relation
    modelBuilder.Entity<Follow>()
    .HasOne(f => f.FollowedUser)
    .WithMany()
    .HasForeignKey(f => f.FollowedUserId)
    .OnDelete(DeleteBehavior.Restrict);

    // Configure Follow -> Follower relation
    modelBuilder.Entity<Follow>()
    .HasOne(f => f.Follower)
    .WithMany()
    .HasForeignKey(f => f.FollowerId)
    .OnDelete(DeleteBehavior.Restrict);
  }

}
