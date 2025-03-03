using CinemaHD.Models.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CinemaHD.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Cinemas> Cinemas { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<MovieShowtimes> MovieShowtimes { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<OTPs> OTPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa ràng buộc khoá ngoại
            modelBuilder.Entity<Cinemas>()
                .HasOne(c => c.Location)
            .WithMany()
                .HasForeignKey(c => c.LocationID);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .IsRequired(false);

            modelBuilder.Entity<Seats>()
                .HasOne(s => s.Cinema)
            .WithMany()
                .HasForeignKey(s => s.CinemaID);

            modelBuilder.Entity<MovieShowtimes>()
                .HasOne(ms => ms.Movie)
            .WithMany()
                .HasForeignKey(ms => ms.MovieID);

            modelBuilder.Entity<MovieShowtimes>()
                .HasOne(ms => ms.Cinema)
            .WithMany()
                .HasForeignKey(ms => ms.CinemaID);

            modelBuilder.Entity<Shifts>()
                .HasOne(s => s.User)
            .WithMany()
                .HasForeignKey(s => s.UserID);

            modelBuilder.Entity<Shifts>()
                .HasOne(s => s.Location)
            .WithMany()
                .HasForeignKey(s => s.LocationID);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.User)
            .WithMany()
                .HasForeignKey(r => r.UserID);

            modelBuilder.Entity<Reservations>()
                .HasOne(r => r.Seat)
                .WithMany()
                .HasForeignKey(r => r.SeatID);

            modelBuilder.Entity<OTPs>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
