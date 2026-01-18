using Contacts.Server.Exceptions;
using Contacts.Server.Model;
using Contacts.Server.Repositories.Interfaces;
using Contacts.Server.Services.Interfaces;

namespace Contacts.Server.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IPhoneNumberRepository _phoneNumberRepository;
        private readonly IContactRepository _contactRepository;

        public PhoneNumberService(IPhoneNumberRepository phoneNumberRepository, IContactRepository contactRepository)
        {
            _phoneNumberRepository = phoneNumberRepository;
            _contactRepository = contactRepository;
        }

        public async Task AddPhoneNumber(Guid contactId, string phoneNumber, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId, cancellationToken);
            if (contact == null)
            {
                throw new NotFoundException("Contact not found");
            }

            if (contact.PhoneNumbers.Count >= 3)
            {
                throw new ValidationException("A contact can have no more than 3 phone numbers.");
            }

            var newPhoneNumber = new PhoneNumber
            {
                Number = phoneNumber,
                ContactId = contactId
            };

            await _phoneNumberRepository.AddAsync(newPhoneNumber, cancellationToken);
        }

        public async Task DeletePhoneNumber(Guid contactId, Guid phoneId, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId, cancellationToken);
            if (contact == null)
            {
                throw new NotFoundException("Contact not found");
            }

            var phoneNumber = contact.PhoneNumbers.FirstOrDefault(p => p.Id == phoneId);
            if (phoneNumber == null)
            {
                throw new NotFoundException("Phone number not found");
            }

            await _phoneNumberRepository.DeleteAsync(phoneNumber, cancellationToken);
        }
    }
}
