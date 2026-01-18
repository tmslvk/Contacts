using Contacts.Server.DTO;
using Contacts.Server.Model;
using System.Text.RegularExpressions;

namespace Contacts.Server.Validators
{
    public class ContactValidator
    {
        public List<string> Validate(ContactDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            {
                errors.Add("Contact name is required.");
            }

            if (dto.FirstName?.Length > 50 || dto.LastName?.Length > 50)
            {
                errors.Add("Contact lastname or firstname cannot exceed 50 characters.");
            }

            if (dto.JobTitle?.Length > 50)
            {
                errors.Add("Job title cannot exceed 50 characters.");
            }

            if (dto.BirthDate > DateTime.Now || dto.BirthDate.Year < 1927)
            {
                errors.Add("Date of birth is incorrect.");
            }

            if (dto.PhoneNumbers != null && !IsValidPhoneNumbers(dto.PhoneNumbers))
            {
                errors.Add("Phone numbers are incorrect.");
            }

            return errors;
        }

        public List<string> Validate(ContactUpdateDTO dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            {
                errors.Add("Contact name is required.");
            }

            if (dto.FirstName?.Length > 50 || dto.LastName?.Length > 50)
            {
                errors.Add("Contact lastname or firstname cannot exceed 50 characters.");
            }

            if (dto.JobTitle?.Length > 50)
            {
                errors.Add("Job title cannot exceed 50 characters.");
            }

            if (dto.BirthDate > DateTime.Now || dto.BirthDate.Year < 1927)
            {
                errors.Add("Date of birth is incorrect.");
            }

            if (dto.PhoneNumbers != null && !IsValidPhoneNumbers(dto.PhoneNumbers))
            {
                errors.Add("Phone numbers are incorrect.");
            }

            return errors;
        }

        public bool IsValidPhoneNumbers(List<string> phoneNumbers)
        {
            if (phoneNumbers.Count > 3)
            {
                return false;
            }

            foreach (var phoneNumber in phoneNumbers)
            {
                if (!IsValid(phoneNumber))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsValid(string phoneNumber) =>
            string.IsNullOrEmpty(phoneNumber) || Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$");

        public List<string> ValidatePhoneNumbers(List<PhoneNumber> phoneNumbers)
        {
            var errors = new List<string>();

            if (phoneNumbers.Count > 3)
            {
                errors.Add("A contact can have no more than 3 phone numbers.");
            }

            foreach (var phoneNumber in phoneNumbers)
            {
                if (!IsValid(phoneNumber.Number))
                {
                    errors.Add($"Phone number {phoneNumber.Number} is incorrect.");
                }
            }

            return errors;
        }
    }
}
