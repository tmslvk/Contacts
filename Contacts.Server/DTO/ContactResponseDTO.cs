namespace Contacts.Server.DTO
{
    public class ContactResponseDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public List<string> PhoneNumbers { get; set; } = new();
    }
}
