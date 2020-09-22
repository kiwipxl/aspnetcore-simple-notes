using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace notes.Models
{
    public enum NoteCategory
    {
        Default, 
        Pinned, 
        Archived
    }

    public class Note
    {
        public Note()
        {
            Category = NoteCategory.Default;
            NoteImages = new List<Image>();
            NoteLabels = new List<NoteLabel>();
        }

        public Note(DTO.NoteDTO note, DatabaseContext dbContext)
        {
            Title = note.Title;
            Body = note.Body;
            Category = note.Category;
            Created = note.Created;
            Edited = note.Edited;

            NoteImages = new List<Image>();
            foreach (DTO.ImageDTO image in note.Images)
            {
                NoteImages.Add(new Image(image));
            }

            NoteLabels = new List<NoteLabel>();
            foreach (DTO.LabelDTO label in note.Labels)
            {
                AddLabel(label, dbContext);
            }
        }

        public int NoteId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public string Body { get; set; }

        public virtual List<Image> NoteImages { get; set; }

        public virtual List<NoteLabel> NoteLabels { get; set; }

        public NoteCategory Category { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Edited { get; set; }

        public void SetFrom(DTO.NoteDTO note, DatabaseContext dbContext)
        {
            Edited = DateTime.Now;
            
            Title = note.Title;
            Body = note.Body;
            Category = note.Category;

            NoteImages.Clear();
            foreach (DTO.ImageDTO image in note.Images)
            {
                NoteImages.Add(new Image(image));
            }

            // Repopulate note labels
            NoteLabels.Clear();
            foreach (DTO.LabelDTO label in note.Labels)
            {
                AddLabel(label, dbContext);
            }
        }

        private void AddLabel(DTO.LabelDTO label, DatabaseContext dbContext)
        {
            var dbLabel =
                dbContext.Labels
                .AsTracking()
                .AsEnumerable()
                .FirstOrDefault(l =>
                (l.LabelId == label.Id || string.Equals(l.Name, label.Name, StringComparison.CurrentCultureIgnoreCase)));

            if (dbLabel == null)
            {
                throw new Exception($"Failed to find label with id {label.Id} or name '{label.Name}'.");
            }

            if (NoteLabels.Any(nl => nl.LabelId == dbLabel.LabelId))
            {
                throw new Exception($"Duplicate labels found with name '{dbLabel.Name}'");
            }

            NoteLabels.Add(new NoteLabel()
            {
                Note = this,
                Label = dbLabel
            });
        }
    }
}