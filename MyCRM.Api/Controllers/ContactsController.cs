using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCRM.Application.Contacts;

namespace MyCRM.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/contacts")]
public sealed class ContactsController : ControllerBase
{
    private readonly ContactService _contactService;

    public ContactsController(ContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContactRequest request)
    {
        await _contactService.CreateAsync(request);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateContactRequest request)
    {
        await _contactService.UpdateAsync(request);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ContactDto>>> GetAll()
    {
        var result = await _contactService.GetAllAsync();
        return Ok(result);
    }

    

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContactDto>> GetById(int id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact == null)
            return NotFound();

        return Ok(contact);
    }

}
