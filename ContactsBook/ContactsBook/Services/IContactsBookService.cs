namespace ContactsBook.Services
{
    public interface IContactsBookService
    {
        bool AddContact(DTO.ContactFormDto contact);
        IEnumerable<DTO.ContactFormDto> GetAllContacts();
    }
}
