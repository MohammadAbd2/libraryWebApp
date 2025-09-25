using System.Text.Json;
using api;
using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.AddAppOptions(builder.Configuration);
Console.WriteLine(JsonSerializer.Serialize(appOptions));

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(appOptions.DbConnectionString);
    
});


var app = builder.Build();

app.MapGet("/", ([FromServices] MyDbContext dbContext) =>
{
    

    var author = new Author()
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Neon"
    };
    dbContext.Authors.Add(author);
    dbContext.SaveChanges();
    var authors = dbContext.Authors.ToList();
    return authors;
});

app.Run();
