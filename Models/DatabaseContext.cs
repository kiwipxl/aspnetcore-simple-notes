using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace notes.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            // Tracking allows us to return a value from a database query, change it's value, and
            // save changes to the database. This is the default behaviour, however it's slower, so
            // let's when we query lets explicitly call .AsTracking() instead
            // See https://docs.microsoft.com/en-us/ef/core/querying/tracking

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteLabel>()
                .HasKey(nl => new { nl.NoteId, nl.LabelId });

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NoteId);

            modelBuilder.Entity<Note>()
                .OwnsMany(n => n.NoteImages);

            modelBuilder.Entity<Label>()
                .HasIndex(l => l.Name)
                .IsUnique();
        }
    }
}