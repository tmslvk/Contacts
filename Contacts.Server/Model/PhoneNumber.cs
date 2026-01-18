namespace Contacts.Server.Model
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
