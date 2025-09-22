using efscaffold.Entities;
using Infrastructure.Postgres.Scaffolding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql("Host=ep-rough-butterfly-ag29xb3y-pooler.c-2.eu-central-1.aws.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_4iTInVwJ5Zly;SslMode=Require;ChannelBinding=Require;");
    
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
