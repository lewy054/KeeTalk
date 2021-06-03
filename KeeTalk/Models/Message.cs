﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RoomId { get; set; }

        [Required]
        public string Author { get; set; }
        [NotMapped]
        public string AuthorProfileImage { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
