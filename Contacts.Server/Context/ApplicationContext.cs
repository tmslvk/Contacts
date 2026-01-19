using Contacts.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Server.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<PhoneNumber> PhoneNumbers { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.JobTitle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.BirthDate)
                    .IsRequired();
            });

            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Number)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(p => p.Contact)
                    .WithMany(c => c.PhoneNumbers)
                    .HasForeignKey(p => p.ContactId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
