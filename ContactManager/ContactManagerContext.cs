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
            modelBuilder.Entity<Contact>().HasData(
                    new Contact { Id = 1, Name = "First Test", Email = "first.test@example.com", Phone = "9874563218", Address = "123 Main St, City, Country" });
            modelBuilder.Entity<Contact>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        }

        public virtual DbSet<Contact> Contacts { get; set; }

    }
}
