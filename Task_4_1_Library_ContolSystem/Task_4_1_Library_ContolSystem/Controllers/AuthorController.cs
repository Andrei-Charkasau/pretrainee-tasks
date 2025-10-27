using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;

namespace Task_4_1_Library_ControlSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AurhorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AurhorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            try
            {
                return Ok(await _authorService.GetAllAuthorsAsync());
            }
            catch (Exception ex)
            {
                return NotFound("Author not Found.");
            }
        }

        [HttpGet("{authorId}")]
        public async Task<ActionResult<Author>> GetAuthorById(int authorId)
        {
            try
            {
                return Ok(await _authorService.GetAuthorByIdAsync(authorId));
            }
            catch (Exception ex)
            {
                return NotFound("Author not Found.");
            }
        }

        [HttpGet("Count")]
        public async Task<ActionResult<List<AuthorWithBooksCountDto>>> GetAllAuthorsWithBooksAmount()
        {
            try
            {
                return Ok(await _authorService.GetAllAuthorsWithBooksAmountAsync());
            }
            catch (Exception ex)
            {
                return NotFound("Authors not Found.");
            }
        }

        [HttpGet("ByName")]
        public async Task<ActionResult<List<AuthorWithBooksCountDto>>> GetAuthorByName(string authorName)
        {
            try
            {
                return Ok(await _authorService.GetAuthorByNameAsync(authorName));
            }
            catch (Exception ex)
            {
                return NotFound("Author not Found.");
            }
        }


        [HttpPost]
        public async Task<ActionResult<string>> CreateAuthorAsync(AuthorDto authorDto)
        {
            try
            {
                await _authorService.CreateAuthorAsync(authorDto);
                return StatusCode(201, "Author added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateAuthorAsync(int id, AuthorDto authorDto)
        {
            try
            {
                await _authorService.UpdateAuthorAsync(id, authorDto);
                return Ok("Author modified.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteAuthorAsync(int authorId)
        {
            try
            {
                await _authorService.DeleteAuthorAsync(authorId);
                return Ok("Author erased.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
