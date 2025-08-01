using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlazorApp.Client.Dtos;
using MyBlazorApp.Data;

namespace MyBlazorApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MyEntityController : ControllerBase
{
  private readonly MyBlazorAppContext _context;

  public MyEntityController(MyBlazorAppContext context)
  {
    _context = context;
  }

  // GET: api/MyEntity
  [HttpGet]
  public async Task<ActionResult<IEnumerable<MyEntityDto>>> GetMyEntity()
  {
    return await _context.MyEntityDto.ToListAsync();
  }

  // GET: api/MyEntity/5
  [HttpGet("{id}")]
  public async Task<ActionResult<MyEntityDto>> GetMyEntity(Guid id)
  {
    var myEntityDto = await _context.MyEntityDto.FindAsync(id);

    if (myEntityDto == null)
    {
      return NotFound();
    }

    return myEntityDto;
  }

  // PUT: api/MyEntity/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutMyEntity(Guid id, MyEntityDto myEntityDto)
  {
    if (id != myEntityDto.Id)
    {
      return BadRequest();
    }

    _context.Entry(myEntityDto).State = EntityState.Modified;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!MyEntityExists(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  // POST: api/MyEntity
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<MyEntityDto>> PostMyEntity(MyEntityDto myEntityDto)
  {
    _context.MyEntityDto.Add(myEntityDto);
    await _context.SaveChangesAsync();

    return CreatedAtAction("GetMyEntity", new { id = myEntityDto.Id }, myEntityDto);
  }

  // DELETE: api/MyEntity/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteMyEntity(Guid id)
  {
    var myEntityDto = await _context.MyEntityDto.FindAsync(id);
    if (myEntityDto == null)
    {
      return NotFound();
    }

    _context.MyEntityDto.Remove(myEntityDto);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool MyEntityExists(Guid id)
  {
    return _context.MyEntityDto.Any(e => e.Id == id);
  }
}
