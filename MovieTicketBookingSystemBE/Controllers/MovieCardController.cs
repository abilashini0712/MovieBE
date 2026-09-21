using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystemBE.Data;
using MovieTicketBookingSystemBE.Models;


namespace MovieCardControllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CardsController(ApplicationDbContext context)
    {
        _context = context;
    }

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetCards()
    {
        return await _context.Movies.ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetCard(int id)
    {
        var card = await _context.Movies.FindAsync(id);
        if (card == null) return NotFound();
        return card;
    }


    [HttpPost]
    public async Task<ActionResult<Movie>> CreateCard(Movie card)
    {
        _context.Movies.Add(card);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCard), new { id = card.id }, card);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCard(int id, Movie card)
    {
        if (id != card.id) return BadRequest();

        _context.Entry(card).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Movies.Any(e => e.id == id)) return NotFound();
            throw;
        }

        return NoContent();
    }

  
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var card = await _context.Movies.FindAsync(id);
        if (card == null) return NotFound();

        _context.Movies.Remove(card);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
