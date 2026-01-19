using Contacts.Server.DTO;
using Contacts.Server.Model;

namespace Contacts.Server.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactResponseDTO>> GetAll(ContactQueryDTO query, CancellationToken cancellationToken);
        Task<Contact> GetById(Guid id, CancellationToken cancellationToken);
        Task<Contact> Create(ContactDTO dto, CancellationToken cancellationToken);
        Task Update(Guid id, ContactUpdateDTO dto, CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
