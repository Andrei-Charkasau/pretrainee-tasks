using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;
using Task_4_1_Library_ControlSystem.Services;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthorsAsync()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<Author>> GetAuthorByIdAsync(int authorId)
        {
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            return Ok(author);
        }

        [HttpGet("With_books_count")]
        public async Task<ActionResult<List<AuthorWithBooksCountDto>>> GetAllAuthorsWithBooksAmountAsync()
        {
            var authors = await _authorService.GetAllAuthorsWithBooksAmountAsync();
            return Ok(authors);
        }

        [HttpGet("Search")]
        public async Task<ActionResult<Author>> GetAuthorByNameAsync(string authorName)
        {
            var author = await _authorService.GetAuthorByNameAsync(authorName);
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthorAsync(AuthorDto authorDto)
        {
            await _authorService.CreateAuthorAsync(authorDto);
            return Ok("Author created successfully.");
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateAuthorAsync(int id, AuthorDto authorDto)
        {
            await _authorService.UpdateAuthorAsync(id, authorDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteAuthorAsync(int authorId)
        {
            await _authorService.DeleteAuthorAsync(authorId);
            return NoContent();
        }
    }
}
