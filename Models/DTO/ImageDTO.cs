using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace notes.Models.DTO
{
    public class ImageDTO
    {
        public ImageDTO()
        {

        }

        public ImageDTO(Image image)
        {
            Data = image.Data;
        }

        public byte[] Data { get; set; }
    }
}