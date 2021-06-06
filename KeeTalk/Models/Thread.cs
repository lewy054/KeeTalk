using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Section { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
    }
}
