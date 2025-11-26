using FriendZonePlus.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FriendZonePlus.Infrastructure.Data;

public class FriendZonePlusContext : DbContext
{

  public FriendZonePlusContext(DbContextOptions<FriendZonePlusContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }

}
