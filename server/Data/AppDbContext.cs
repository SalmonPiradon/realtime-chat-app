using Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomMember> RoomMembers { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Room>().HasIndex(r => r.Name).IsUnique();

        modelBuilder.Entity<RoomMember>().HasKey(rm => new { rm.RoomId, rm.UserId });
        // ต่อให้ไม่เขียน EF ก็เดาเองได้ว่าตัวไหนเป็น FK แต่เราเขียนให้ clear
        modelBuilder.Entity<RoomMember>().HasOne(rm => rm.Room).WithMany(r => r.Members)
            .HasForeignKey(rm => rm.RoomId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RoomMember>().HasOne(rm => rm.User).WithMany()
            .HasForeignKey(rm => rm.UserId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>().HasOne(r => r.CreatedBy).WithMany()
            .HasForeignKey(r => r.CreatedById).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Message>().HasOne(m => m.User).WithMany()
            .HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}