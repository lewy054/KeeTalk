using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ThreadId { get; set; }
        public string AuthorId { get; set; }
        [NotMapped]
        public string AuthorName { get; set; }
        [NotMapped]
        public string AuthorImage { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string EditedBy { get; set; }
        public DateTime EditDate { get; set; }
    }
}
