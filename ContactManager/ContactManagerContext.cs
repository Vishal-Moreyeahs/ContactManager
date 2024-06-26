using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager
{
    public class ContactManagerContext : DbContext
    {
        public ContactManagerContext(DbContextOptions<ContactManagerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactManagerContext).Assembly);
        }

        public virtual DbSet<Contact> Customers { get; set; }

    }
}
