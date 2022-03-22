using Library.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryDataBaseContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// APIs
app.MapGet("/books", async (LibraryDataBaseContext context) =>
    await context.Books.ToListAsync());

app.MapGet("/book/{id}", async (LibraryDataBaseContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    return book != null ? Results.Ok(book) : Results.NotFound();
});

app.MapPost("/book", async (LibraryDataBaseContext context, Book book) =>
{
    await context.Books.AddAsync(book);
    var result = await context.SaveChangesAsync();
    return result > 0 ? Results.Ok() : Results.BadRequest();
});

app.MapPut("/book", async (LibraryDataBaseContext context, Book book) =>
{
    var bookFound = await context.Books.FindAsync(book.Id);
    if (bookFound != null)
    {
        bookFound.Author = book.Author;
        bookFound.AuthorId = book.AuthorId;
        bookFound.Editorial = book.Editorial;
        bookFound.GenreId = book.GenreId;
        context.Update(bookFound);
        var result = await context.SaveChangesAsync();
        return result > 0 ? Results.Ok() : Results.BadRequest("Something went wrong when updating...");
    }
    return Results.NotFound("Book not found!");
});

app.MapDelete("/book/{id}", async (LibraryDataBaseContext context, int id) =>
{
    var bookFound = await context.Books.FindAsync(id);
    if (bookFound != null)
    {
        context.Books.Remove(bookFound);
        var result = context.SaveChanges();
        return result > 0 ? Results.Ok() : Results.BadRequest("Something went wrong when removing...");
    }
    return Results.NotFound("Book not found!");
});

app.Run();

