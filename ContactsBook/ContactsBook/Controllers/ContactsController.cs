using ContactsBook.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;


namespace ContactsBook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly Services.IContactsBookService _contactsBookService;

        public ContactsController(
            Services.IContactsBookService contactsBookService, ILogger<ContactsController> logger)
        {
            _contactsBookService = contactsBookService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] ContactFormDto contact)
        {
            try
            {
                if (contact.Photo != null && contact.Photo.Length > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(contact.Photo.FileName);
                    string filePath = Path.Combine("wwwroot/photos", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await contact.Photo.CopyToAsync(fileStream);
                    }

                    _logger.LogInformation($"File saved to: {filePath}");
                    contact.PhotoPath = filePath;
                }

                bool success = _contactsBookService.AddContact(contact);

                return success
                    ? Redirect("/")
                    : StatusCode(500, new { status = "Something went wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, new { status = "Internal Server Error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_contactsBookService.GetAllContacts());
        }
    }
}

