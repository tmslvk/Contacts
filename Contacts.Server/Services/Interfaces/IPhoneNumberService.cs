namespace Contacts.Server.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task AddPhoneNumber(Guid contactId, string phoneNumber, CancellationToken cancellationToken);
        Task DeletePhoneNumber(Guid contactId, Guid phoneId, CancellationToken cancellationToken);
    }
}
