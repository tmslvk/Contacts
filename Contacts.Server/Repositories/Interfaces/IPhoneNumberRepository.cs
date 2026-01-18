using Contacts.Server.Model;

namespace Contacts.Server.Repositories.Interfaces
{
    public interface IPhoneNumberRepository
    {
        Task<PhoneNumber?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
        Task DeleteAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    }
}
