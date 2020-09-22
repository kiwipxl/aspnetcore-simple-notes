using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace notes.Models
{
    public class Image
    {
        public Image()
        {

        }

        public Image(DTO.ImageDTO image)
        {
            Data = image.Data;
        }

        [Required]
        public byte[] Data { get; set; }
    }
}