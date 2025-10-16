using Microsoft.AspNetCore.Mvc;
using Task_4_1_Library_ControlSystem.Services;
using Task_4_1_Library_ControlSystem.Models;
using Task_4_1_Library_ControlSystem.DtoModels;

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
        public ActionResult<List<Author>> GetAllAuthors()
        {
            try
            {
                return Ok(_authorService.RetriveAll());
            }
            catch (Exception ex)
            {
                return NotFound("Author not Found.");
            }
        }

        [HttpGet("{authorId}")]
        public ActionResult<Author> GetAuthorById(int authorId)
        {
            try
            {
                return Ok(_authorService.Retrive(authorId));
            }
            catch (Exception ex)
            {
                return NotFound("Author not Found.");
            }
        }

        [HttpPost]
        public ActionResult<string> CreateAuthor(AuthorDto authorDto)
        {
            //Console.WriteLine($"Received author: Id={author.Id}, Title={author.Name}");
            try
            {
                _authorService.Add(authorDto);
                return StatusCode(201, "Author added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public ActionResult<string> UpdateAuthor(int id, AuthorDto authorDto)
        {
            try
            {
                _authorService.Modify(id, authorDto);
                return Ok("Author modified.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult<string> DeleteAuthor(int authorId)
        {
            try
            {
                _authorService.Erase(authorId);
                return Ok("Author erased.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
