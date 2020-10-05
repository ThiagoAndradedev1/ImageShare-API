using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUploadAPI.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(400)")]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
