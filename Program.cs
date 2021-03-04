using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace notes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = new DbContextOptionsBuilder<Models.DatabaseContext>();
            //builder.UseNpgsql("Host=localhost;Username=postgres;Password=lol;Database=notes;");

            //using (var db = new Models.DatabaseContext(builder.Options))
            //{
            //    AddNotesUnitTest(db);
            //}

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void AddNotesUnitTest(Models.DatabaseContext db)
        {
            // Simple and quick unit test to verify API functionality
            db.Notes.RemoveRange(db.Notes);
            db.Labels.RemoveRange(db.Labels);

            db.SaveChanges();

            var socialLabel = new Models.Label { Name = "Social" };
            db.Labels.Add(socialLabel);

            var meditationLabel = new Models.Label { Name = "Meditation" };
            db.Labels.Add(meditationLabel);

            db.Notes.Add(new Models.Note
            {
                Title = "Gratitude 30/8",
                Body = "Grateful.",
                Created = DateTime.Now,
                NoteLabels = new List<Models.NoteLabel>()
                {
                    new Models.NoteLabel() { Label = meditationLabel }
                }
            });

            db.Notes.Add(new Models.Note
            {
                Title = "Gratitude 31/8",
                Body = "Grateful again!",
                Created = DateTime.Now,
                NoteImages = new List<Models.Image>()
                {
                    new Models.Image() { Data = new byte[8] }
                },
                NoteLabels = new List<Models.NoteLabel>()
                {
                    new Models.NoteLabel() { Label = meditationLabel }
                }
            });

            db.Notes.Add(new Models.Note
            {
                Title = "Social Meditation",
                Body = "Meetup event!",
                Created = DateTime.Now,
                NoteImages = new List<Models.Image>()
                {
                    new Models.Image() { Data = new byte[12] },
                    new Models.Image() { Data = new byte[18] }
                },
                NoteLabels = new List<Models.NoteLabel>()
                {
                    new Models.NoteLabel() { Label = meditationLabel },
                    new Models.NoteLabel() { Label = socialLabel }
                }
            });

            db.SaveChanges();

            foreach (var note in
                db.Notes
                .Where(n => n.NoteLabels.Any(nl => nl.Label == meditationLabel))
                .OrderBy(n => n.NoteId))
            {
                Console.WriteLine($"meditation note: {note.NoteId}, {note.Title}, {note.Body}, num images: {note.NoteImages.Count}, num labels: {note.NoteLabels.Count}");
            }

            foreach (var note in
                db.Notes
                .Where(n => n.NoteLabels.Any(nl => nl.Label == socialLabel))
                .OrderBy(n => n.NoteId))
            {
                Console.WriteLine($"social note: {note.NoteId}, {note.Title}, {note.Body}, num images: {note.NoteImages.Count}, num labels: {note.NoteLabels.Count}");
            }

            Console.WriteLine($"remove note {db.Notes.First().NoteId}");
            db.Notes.Remove(db.Notes.First());

            Console.WriteLine("remove social label");
            db.Labels.Remove(socialLabel);

            db.SaveChanges();

            foreach (var note in
                db.Notes
                .Where(n => n.NoteLabels.Any(nl => nl.Label == meditationLabel))
                .OrderBy(n => n.NoteId))
            {
                Console.WriteLine($"meditation note: {note.NoteId}, {note.Title}, {note.Body}, num images: {note.NoteImages.Count}, num labels: {note.NoteLabels.Count}");
            }

            foreach (var note in
                db.Notes
                .Where(n => n.NoteLabels.Any(nl => nl.Label == socialLabel))
                .OrderBy(n => n.NoteId))
            {
                Console.WriteLine($"social note: {note.NoteId}, {note.Title}, {note.Body}, num images: {note.NoteImages.Count}, num labels: {note.NoteLabels.Count}");
            }
        }
    }
}
