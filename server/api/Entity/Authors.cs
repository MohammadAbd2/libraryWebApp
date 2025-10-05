using efscaffold.Entities;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;

namespace api.Entity;

[ApiController]
[Route("[controller]")]
public class Authors : ControllerBase
{
    private readonly MyDbContext _dbContext;
    // ✅ Constructor injection — ASP.NET will provide it automatically
    public Authors (MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // on get request
    [HttpGet]
    public IActionResult Get()
    {
        var authors = _dbContext.Authors.ToList();
        return Ok(authors);
    }
    // on post request 
    [HttpPost]
    public IActionResult Post([FromBody] Author author)
    {
        _dbContext.Authors.Add(author);
        _dbContext.SaveChanges();
        return Ok("data has been recived");
    }
    // on put request
    [HttpPut]
    public IActionResult Put([FromBody] Author author)
    {
        // validation before handling the request
        if (author == null || string.IsNullOrEmpty(author.Id))
            return BadRequest("Invalid author data.");

        // Find existing author in DB
        var existingAuthor = _dbContext.Authors.FirstOrDefault(a => a.Id == author.Id);
        if (existingAuthor == null)
            return NotFound($"Author with ID '{author.Id}' not found.");

        // Update the fields
        existingAuthor.Name = author.Name;

        // Save changes
        _dbContext.SaveChanges();

        // Return updated entity
        return Ok(existingAuthor);
    }
    // on delete request
    [HttpDelete]
    public IActionResult Delete([FromBody] Author author)
    {
        if (author == null || string.IsNullOrEmpty(author.Id))
            return BadRequest("Invalid author data.");

        var existingAuthor = _dbContext.Authors.FirstOrDefault(a => a.Id == author.Id);
        if (existingAuthor == null)
            return NotFound($"Author with ID '{author.Id}' not found.");

        _dbContext.Authors.Remove(existingAuthor);
        _dbContext.SaveChanges();

        return Ok($"Author '{existingAuthor.Name}' was deleted successfully.");
    }
}