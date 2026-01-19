using Contacts.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Server.Context
{
    public class DbSeeder
    {
        private readonly ApplicationContext _context;

        public DbSeeder(ApplicationContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Contacts.Any())
                return;

            var random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    FirstName = $"FirstName{i}",
                    LastName = $"LastName{i}",
                    JobTitle = $"JobTitle{i}",
                    BirthDate = DateTime.Now.AddYears(-random.Next(20, 60))
                };

                // Добавляем 1-3 номера телефонов
                int phoneCount = random.Next(1, 4);
                for (int j = 1; j <= phoneCount; j++)
                {
                    var phone = new PhoneNumber
                    {
                        Id = Guid.NewGuid(),
                        Number = $"+7{random.Next(900000000, 999999999)}",
                        Contact = contact
                    };
                    contact.PhoneNumbers.Add(phone);
                }

                _context.Contacts.Add(contact);
            }

            _context.SaveChanges();
        }
    }
}
