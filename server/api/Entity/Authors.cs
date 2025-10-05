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
}