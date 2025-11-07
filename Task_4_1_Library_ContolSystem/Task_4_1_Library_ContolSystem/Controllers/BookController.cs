using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Services;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetAllBooksAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int bookId)
        {
            var book = await _bookService.GetBookByIdAsync(bookId);
            return Ok(book);
        }

        [HttpGet("Author/{authorId}")]
        public async Task<ActionResult<Book>> GetBookByAuthorIdAsync(int authorId)
        {
            var books = await _bookService.GetBooksByAuthorIdAsync(authorId);
            return Ok(books);
        }

        [HttpGet("Year/{publishingYear}")]
        public async Task<ActionResult<Book>> GetBooksFromPublishYearToNowAsync(int publishingYear)
        {
            var books = await _bookService.GetBooksFromPublishYearToNowAsync(publishingYear);
            return Ok(books);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Book>>> GetBooksByTitleAsync(string bookTitle)
        {
            var books = await _bookService.GetBooksByTitleAsync(bookTitle);
            return Ok(books);
        }


        [HttpPost]
        public async Task<ActionResult<string>> CreateBookAsync(BookDto bookDto)
        {
            await _bookService.CreateBookAsync(bookDto);
            return Ok("Book created successfully.");
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateBookAsync(int id, BookDto bookDto)
        {
            await _bookService.UpdateBookAsync(id, bookDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteBookAsync(int bookId)
        {
            await _bookService.DeleteBookAsync(bookId);
            return NoContent();
        }
    }
}
