using BlazorApp.Server.Data;
using BlazorApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext context;

        public BooksController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> ListAsync()
        {
            var books = await context.Books.ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetAsync(int id)
        {
            var book = await context.Books.SingleOrDefaultAsync(b => b.BookId.Equals(id));

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> CreateAsync(Book book)
        {
            if (book.BookId != 0)
            {
                return BadRequest();
            }

            context.Books.Add(book);
            await context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = book.BookId }, book);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var book = await context.Books.SingleOrDefaultAsync(b => b.BookId.Equals(id));

            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Book book)
        {
            if (await context.Books.AsNoTracking().SingleOrDefaultAsync(b => b.BookId == book.BookId) == null)
            {
                return NotFound();
            }

            context.Entry(book).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
