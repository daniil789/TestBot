using CoreBot.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class GameStoreDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Определения ограничений и отношений между таблицами
        modelBuilder.Entity<Key>()
            .HasOne(k => k.Game)
            .WithMany(g => g.Keys)
            .HasForeignKey(k => k.GameId);

        modelBuilder.Entity<User>()
                  .HasKey(u => u.Id); // Определение, что Id является ключом

        modelBuilder.Entity<Order>()
            .HasKey(o => o.Id); // Определение, что Id является ключом

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .HasPrincipalKey(u => u.Id); // Указание, что ключ в сущности User является ключом для связи



        // Другие настройки

        base.OnModelCreating(modelBuilder);
    }
}
