namespace Contacts.Server.Model
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
        public string JobTitle { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
