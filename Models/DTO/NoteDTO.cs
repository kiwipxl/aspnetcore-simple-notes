using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace notes.Models.DTO
{
    public class NoteDTO
    {
        public NoteDTO()
        {
            Images = new List<ImageDTO>();
            Labels = new List<LabelDTO>();
        }

        public NoteDTO(Note note)
        {
            Id = note.NoteId;
            Title = note.Title;
            Body = note.Body;
            Category = note.Category;
            Created = note.Created;
            Edited = note.Edited;

            Images = new List<ImageDTO>();
            foreach (Image image in note.NoteImages)
            {
                Images.Add(new ImageDTO(image));
            }

            Labels = new List<LabelDTO>();
            foreach (NoteLabel label in note.NoteLabels)
            {
                Labels.Add(new LabelDTO(label.Label));
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public List<ImageDTO> Images { get; set; }

        public List<LabelDTO> Labels { get; set; }
        
        public NoteCategory Category { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime Edited { get; set; }
    }
}