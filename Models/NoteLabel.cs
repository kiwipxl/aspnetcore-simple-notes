using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace notes.Models
{
    public class NoteLabel
    {
        public int NoteId { get; set; }
        public Note Note { get; set; }

        public int LabelId { get; set; }
        public Label Label { get; set; }
    }
}