using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace notes.Models.DTO
{
    public class LabelDTO
    {
        public LabelDTO()
        {

        }

        public LabelDTO(Label label)
        {
            Id = label.LabelId;
            Name = label.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}