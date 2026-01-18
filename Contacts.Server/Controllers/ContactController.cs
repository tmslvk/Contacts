using Contacts.Server.DTO;
using Contacts.Server.Exceptions;
using Contacts.Server.Model;
using Contacts.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ContactQueryDTO query, CancellationToken cancellationToken)
        {
            var contacts = await _contactService.GetAll(query, cancellationToken);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var contact = await _contactService.GetById(id, cancellationToken);
            if (contact == null)
            {
                return NotFound("Contact not found");
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _contactService.Create(dto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContactUpdateDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _contactService.Update(id, dto, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Contact not found");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _contactService.Delete(id, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound("Contact not found");
            }
        }
    }
}
