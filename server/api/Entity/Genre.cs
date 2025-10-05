using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")] // => api wokks on http://localhost/genres
public class GenresController : ControllerBase
{
    private readonly MyDbContext _dbContext;

    public GenresController(MyDbContext context)
    {
        _dbContext = context;
    }

    // GET /genres
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var genres = await _dbContext.Genres
            .Include(g => g.Books)
            .ToListAsync();

        return Ok(genres);
    }

    // GET /genres/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var genre = await _dbContext.Genres
            .Include(g => g.Books)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null)
            return NotFound(new { message = $"Genre with ID '{id}' not found." });

        return Ok(genre);
    }

    // POST /genres
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Genre genre)
    {
        if (genre == null || string.IsNullOrWhiteSpace(genre.Name))
            return BadRequest(new { message = "Invalid genre data." });

        genre.Id = Guid.NewGuid().ToString();
        genre.Createdat = DateTime.UtcNow;

        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

    // PUT /genres/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Genre updatedGenre)
    {
        var existingGenre = await _dbContext.Genres.FindAsync(id);
        if (existingGenre == null)
            return NotFound(new { message = $"Genre with ID '{id}' not found." });

        existingGenre.Name = updatedGenre.Name;
        existingGenre.Createdat = updatedGenre.Createdat ?? existingGenre.Createdat;

        await _dbContext.SaveChangesAsync();
        return Ok(existingGenre);
    }

    // DELETE /genres/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null)
            return NotFound(new { message = $"Genre with ID '{id}' not found." });

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
