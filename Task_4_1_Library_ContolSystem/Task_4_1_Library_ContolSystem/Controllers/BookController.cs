using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;

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
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            try
            {
                return Ok(await _bookService.GetAllBooksAsync());
            }
            catch (Exception ex)
            {
                return NotFound("Book not Found.");
            }
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int bookId)
        {
            try
            {
                return Ok(await _bookService.GetBookByIdAsync(bookId));
            }
            catch (Exception ex)
            {
                return NotFound("Book not Found.");
            }
        }

        [HttpGet("Author/{authorId}")]
        public async Task<ActionResult<Book>> GetBookByAuthorId(int authorId)
        {
            try
            {
                return Ok(await _bookService.GetBooksByAuthorIdAsync(authorId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Year/{publishingYear}")]
        public async Task<ActionResult<Book>> GetBooksFromPublishYearToNowAsync(int publishingYear)
        {
            try
            {
                return Ok(await _bookService.GetBooksFromPublishYearToNowAsync(publishingYear));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<string>> CreateBookAsync(BookDto bookDto)
        {
            try
            {
                _bookService.CreateBookAsync(bookDto);
                return StatusCode(201, "Book added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateBookAsync(int id, BookDto bookDto)
        {
            try
            {
                await _bookService.UpdateBookAsync(id, bookDto);
                return Ok("Book modified.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteBookAsync(int bookId)
        {
            try
            {
                await _bookService.DeleteBookAsync(bookId);
                return Ok("Book erased.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
