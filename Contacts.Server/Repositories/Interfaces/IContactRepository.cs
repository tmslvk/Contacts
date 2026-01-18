using Contacts.Server.DTO;
using Contacts.Server.Model;

namespace Contacts.Server.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllAsync(ContactQueryDTO query, CancellationToken cancellationToken);
        Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Contact contact, CancellationToken cancellationToken);
        Task UpdateAsync(Contact contact, CancellationToken cancellationToken);
        Task DeleteAsync(Contact contact, CancellationToken cancellationToken);
    }
}
