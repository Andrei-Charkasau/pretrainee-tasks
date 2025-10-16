using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Services;
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
        public ActionResult<List<Book>> GetAllBooks()
        {
            try
            {
                return Ok(_bookService.RetriveAll());
            }
            catch (Exception ex)
            {
                return NotFound("Book not Found.");
            }
        }

        [HttpGet("{bookId}")]
        public ActionResult<Book> GetBookById(int bookId)
        {
            try
            {
                return Ok(_bookService.Retrive(bookId));
            }
            catch(Exception ex)
            {
                return NotFound("Book not Found.");
            }
        }

        [HttpPost]
        public ActionResult<string> CreateBook(BookDto bookDto)
        {
            try
            {
                _bookService.Add(bookDto);
                return StatusCode(201, "Book added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public ActionResult<string> UpdateBook(int id, BookDto bookDto)
        {
            try
            {
                _bookService.Modify(id, bookDto);
                return Ok("Book modified.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult<string> DeleteBook(int bookId)
        {
            try
            {
                _bookService.Erase(bookId);
                return Ok("Book erased.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
