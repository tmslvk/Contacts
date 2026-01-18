using Contacts.Server.DTO;
using Contacts.Server.Exceptions;
using Contacts.Server.Model;
using Contacts.Server.Repositories.Interfaces;
using Contacts.Server.Services.Interfaces;
using Contacts.Server.Validators;

namespace Contacts.Server.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly ContactValidator _validator;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
            _validator = new ContactValidator();
        }

        public async Task<List<Contact>> GetAll(ContactQueryDTO query, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetAllAsync(query, cancellationToken);
        }

        public async Task<Contact> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _contactRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Contact not found");
        }

        public async Task<Contact> Create(ContactDTO dto, CancellationToken cancellationToken)
        {
            var errors = _validator.Validate(dto);
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            string fullname = $"{dto.LastName} {dto.FirstName}";

            var phoneNumbers = dto.PhoneNumbers.Select(pn => new PhoneNumber { Number = pn }).ToList();

            var contact = new Contact
            {
                Name = fullname,
                JobTitle = dto.JobTitle,
                BirthDate = dto.BirthDate,
                PhoneNumbers = phoneNumbers,
            };

            var phoneNumbersErrors = _validator.ValidatePhoneNumbers(contact.PhoneNumbers);
            if (phoneNumbersErrors.Any())
                throw new ValidationException(string.Join("; ", phoneNumbersErrors));

            await _contactRepository.AddAsync(contact, cancellationToken);
            return contact;
        }

        public async Task Update(Guid id, ContactUpdateDTO dto, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Contact not found");

            var errors = _validator.Validate(dto);
            if (errors.Any())
                throw new ValidationException(string.Join("; ", errors));

            string fullname = $"{dto.LastName} {dto.FirstName}";

            contact.Name = fullname;
            contact.JobTitle = dto.JobTitle;
            contact.BirthDate = dto.BirthDate;

            // Сначала очищаем старые номера
            contact.PhoneNumbers.Clear();

            // Добавляем новые номера
            contact.PhoneNumbers.AddRange(dto.PhoneNumbers.Select(pn => new PhoneNumber { Number = pn }));

            var phoneNumbersErrors = _validator.ValidatePhoneNumbers(contact.PhoneNumbers);
            if (phoneNumbersErrors.Any())
                throw new ValidationException(string.Join("; ", phoneNumbersErrors));

            await _contactRepository.UpdateAsync(contact, cancellationToken);
        }

        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException("Contact not found");

            await _contactRepository.DeleteAsync(contact, cancellationToken);
        }
    }
}
