using Contacts.Server.Exceptions;
using Contacts.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contacts.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {
        private readonly IPhoneNumberService _phoneNumberService;

        public PhoneNumberController(IPhoneNumberService phoneNumberService)
        {
            _phoneNumberService = phoneNumberService;
        }

        [HttpPost("{contactId}")]
        public async Task<IActionResult> AddPhoneNumber(Guid contactId, [FromBody] string phoneNumber, CancellationToken cancellationToken)
        {
            try
            {
                await _phoneNumberService.AddPhoneNumber(contactId, phoneNumber, cancellationToken);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
            {
                return NotFound("Contact not found");
            }
        }

        [HttpDelete("{contactId}/{phoneId}")]
        public async Task<IActionResult> DeletePhoneNumber(Guid contactId, Guid phoneId, CancellationToken cancellationToken)
        {
            try
            {
                await _phoneNumberService.DeletePhoneNumber(contactId, phoneId, cancellationToken);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException)
            {
                return NotFound("Phone number or contact not found");
            }
        }
    }
}
