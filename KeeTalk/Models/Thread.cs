using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class Thread
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [StringLength(150, MinimumLength = 8, ErrorMessage = "Must be between 8 and 150 characters")]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string AuthorId { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public bool Closed { get; set; } = false;

        [NotMapped]
        public string AuthorName { get; set; }
        [NotMapped]
        public string AuthorImage { get; set; }
        [NotMapped]
        public string LastCommentAuthor { get; set; }
        [NotMapped]
        public DateTime LastCommentDate { get; set; }
        [NotMapped]
        public string LastCommentAuthorImage { get; set; }
    }
}
