namespace Contacts.Server.DTO
{
    public class ContactUpdateDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> PhoneNumbers { get; set; } = new List<string>();
        public string JobTitle { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
