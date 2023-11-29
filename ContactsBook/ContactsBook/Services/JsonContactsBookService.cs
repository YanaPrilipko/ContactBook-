namespace ContactsBook.Services
{
    public class JsonContactsBookService : IContactsBookService

    {
        public const string ContactsFile = "contacts.json";
        public const string PhotosFolder = "wwwroot/photos";

        public bool AddContact(DTO.ContactFormDto contact)
        {
            try
            {
                var allContacts = new List<DTO.ContactFormDto>(GetAllContacts());
                allContacts.Add(contact);

                if (!Directory.Exists(PhotosFolder))
                {
                    Directory.CreateDirectory(PhotosFolder);
                }
                if (contact.Photo != null && contact.Photo.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(contact.Photo.FileName);
                    string filePath = Path.Combine(PhotosFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        contact.Photo.CopyTo(fileStream);
                    }

                    contact.PhotoPath = $"/photos/{fileName}";
                }
                string json = System.Text.Json.JsonSerializer.Serialize(allContacts);
                File.WriteAllText(ContactsFile, json);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<DTO.ContactFormDto> GetAllContacts()
        {
            if (!File.Exists(ContactsFile))
            {
                return Enumerable.Empty<DTO.ContactFormDto>();
            }
            using (StreamReader r = new StreamReader(ContactsFile))
            {
                string json = r.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    var allContacts = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<DTO.ContactFormDto>>(json);
                    return allContacts;
                }
                return new List<DTO.ContactFormDto>(); ;
            }
        }
    }
}
