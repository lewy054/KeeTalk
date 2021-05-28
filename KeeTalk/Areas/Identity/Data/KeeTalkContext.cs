using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeeTalk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeeTalk.Data
{
    public class KeeTalkContext : IdentityDbContext<IdentityUser>
    {
        public KeeTalkContext(DbContextOptions<KeeTalkContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<Message> Messages { get; set; }
    }
}
