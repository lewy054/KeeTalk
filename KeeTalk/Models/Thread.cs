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
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Creator { get; set; }
        [NotMapped]
        public string CreatorImage { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [NotMapped]
        public string LastCommentAuthor { get; set; }
        [NotMapped]
        public DateTime LastCommentDate { get; set; }
        [NotMapped]
        public string LastCommentAuthorImage { get; set; }
    }
}
