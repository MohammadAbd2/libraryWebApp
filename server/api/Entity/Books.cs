using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")] // works on /api/books
public class BooksController : ControllerBase
{
    private readonly MyDbContext _dbContext;

    public BooksController(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET /api/books
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var books = await _dbContext.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .ToListAsync();

        return Ok(books);
    }

    // GET /api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var book = await _dbContext.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return NotFound(new { message = $"Book with ID '{id}' not found." });

        return Ok(book);
    }

    // POST /api/books
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Book book)
    {
        if (book == null || string.IsNullOrWhiteSpace(book.Title))
            return BadRequest(new { message = "Invalid book data." });

        if (string.IsNullOrEmpty(book.Id))
            book.Id = Guid.NewGuid().ToString();

        book.Createdat = DateTime.UtcNow;

        if (book.Genre != null)
            _dbContext.Entry(book.Genre).State = EntityState.Unchanged;

        if (book.Authors != null)
        {
            foreach (var author in book.Authors)
                _dbContext.Entry(author).State = EntityState.Unchanged;
        }

        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT /api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Book updatedBook)
    {
        if (updatedBook == null || string.IsNullOrWhiteSpace(updatedBook.Title))
            return BadRequest(new { message = "Invalid book data." });

        var existingBook = await _dbContext.Books
            .Include(b => b.Genre)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (existingBook == null)
            return NotFound(new { message = $"Book with ID '{id}' not found." });

        // Update fields
        existingBook.Title = updatedBook.Title;
        existingBook.Pages = updatedBook.Pages;
        existingBook.Createdat = updatedBook.Createdat ?? existingBook.Createdat;

        // Update Genre
        if (updatedBook.Genre != null)
            existingBook.Genre = updatedBook.Genre;

        // Update Authors
        if (updatedBook.Authors != null)
        {
            existingBook.Authors.Clear();
            foreach (var author in updatedBook.Authors)
                existingBook.Authors.Add(author);
        }

        await _dbContext.SaveChangesAsync();
        return Ok(existingBook);
    }

    // DELETE /api/books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
            return NotFound(new { message = $"Book with ID '{id}' not found." });

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}
