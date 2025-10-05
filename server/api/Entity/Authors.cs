using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")] // => /api/authors
public class AuthorsController : ControllerBase
{
    private readonly MyDbContext _dbContext;

    public AuthorsController(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET /api/authors
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var authors = await _dbContext.Authors.ToListAsync();
        return Ok(authors);
    }

    // GET /api/authors/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var author = await _dbContext.Authors.FindAsync(id);
        if (author == null)
            return NotFound(new { message = $"Author with ID '{id}' not found." });

        return Ok(author);
    }

    // POST /api/authors
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Author author)
    {
        if (author == null || string.IsNullOrWhiteSpace(author.Name))
            return BadRequest(new { message = "Invalid author data." });

        if (string.IsNullOrEmpty(author.Id))
            author.Id = Guid.NewGuid().ToString();

        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    // PUT /api/authors/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Author updatedAuthor)
    {
        if (updatedAuthor == null || string.IsNullOrWhiteSpace(updatedAuthor.Name))
            return BadRequest(new { message = "Invalid author data." });

        var existingAuthor = await _dbContext.Authors.FindAsync(id);
        if (existingAuthor == null)
            return NotFound(new { message = $"Author with ID '{id}' not found." });

        existingAuthor.Name = updatedAuthor.Name;
        await _dbContext.SaveChangesAsync();

        return Ok(existingAuthor);
    }

    // DELETE /api/authors/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var author = await _dbContext.Authors.FindAsync(id);
        if (author == null)
            return NotFound(new { message = $"Author with ID '{id}' not found." });

        _dbContext.Authors.Remove(author);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
