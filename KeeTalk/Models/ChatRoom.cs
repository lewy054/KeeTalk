using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; }
        [NotMapped]
        public string AuthorName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Must be between 2 and 30 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

    }
}
