using Contacts.Server.Context;
using Contacts.Server.DTO;
using Contacts.Server.Exceptions;
using Contacts.Server.Model;
using Contacts.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Server.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationContext _db;

        public ContactRepository(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<List<Contact>> GetAllAsync(ContactQueryDTO query, CancellationToken cancellationToken)
        {
            IQueryable<Contact> contactsQuery = _db.Contacts
                .AsNoTracking()
                .Include(c => c.PhoneNumbers);

            // фильтры
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                contactsQuery = contactsQuery.Where(c =>
                    c.FirstName.Contains(query.Search) ||
                    c.LastName.Contains(query.Search) ||
                    c.JobTitle.Contains(query.Search));
            }

            // сортировка и пагинация
            contactsQuery = contactsQuery
                .OrderByDescending(c => c.BirthDate)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            return await contactsQuery.ToListAsync(cancellationToken);
        }




        public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _db.Contacts
                .AsNoTracking()
                .Include(c => c.PhoneNumbers)
                .FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
        }

        public async Task AddAsync(Contact contact, CancellationToken cancellationToken)
        {
            if (contact.PhoneNumbers.Count > 3)
                throw new ValidationException("A contact can have no more than 3 phone numbers.");

            await _db.Contacts.AddAsync(contact, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken)
        {
            if (contact.PhoneNumbers.Count > 3)
                throw new ValidationException("A contact can have no more than 3 phone numbers.");

            _db.Contacts.Update(contact);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Contact contact, CancellationToken cancellationToken)
        {
            _db.Contacts.Remove(contact);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
