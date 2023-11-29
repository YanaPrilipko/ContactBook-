using System.Text.Json.Serialization;

namespace ContactsBook.DTO
{
    public class ContactFormDto
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public DateTime? birthdate { get; set; }
        public string? email { get; set; }

        [JsonIgnore]
        public IFormFile? Photo { get; set; }
        public string? PhotoPath { get; set; }
    }
}
