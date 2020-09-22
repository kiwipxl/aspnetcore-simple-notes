using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace notes.Models
{
    public class Label
    {
        public Label()
        {

        }

        public Label(DTO.LabelDTO label)
        {
            LabelId = label.Id;
            Name = label.Name;
        }

        public int LabelId { get; set; }

        // public int UserId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
    }
}