using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KeeTalk.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ImageName { get; set; }
        public bool Banned { get; set; } = false;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime BanStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime BanEndDate { get; set; }
        public int BanCount { get; set; }
    }
}
