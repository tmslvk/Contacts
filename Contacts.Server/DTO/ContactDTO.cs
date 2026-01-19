namespace Contacts.Server.DTO
{
    public class ContactDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = new List<string>();
        public string JobTitle { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}
