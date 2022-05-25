using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SameApp.Models;

namespace SameApp.Data
{
    public class SameAppContext : DbContext
    {
        public SameAppContext(DbContextOptions<SameAppContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasKey(contact => new {contact.Id, contact.UserNameOwner});
        }
        
        public DbSet<SameApp.Models.Contact>? Contact { get; set; }

        public DbSet<SameApp.Models.Message> Message { get; set; }

        public DbSet<SameApp.Models.User> User { get; set; }
    }
}
