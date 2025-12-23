using Books_2.Contracts;
using Books_2.Models;
using Books_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CinemasController : ControllerBase
    {
        private readonly CinemaService _CinemaService;

        public CinemasController(CinemaService CinemaService)
        {
            _CinemaService = CinemaService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Cinemas = await _CinemaService.GetAllAsync();
            return Ok(Cinemas);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CinemaResponse>> GetById(Guid id)
        {
            var Cinema = await _CinemaService.GetByIdAsync(id); // возвращает Cinema с FilmScreenings
            if (Cinema == null)
                return NotFound($"Cinema with ID {id} not found");

            var response = new CinemaResponse(
                Cinema.Id,
                Cinema.Name,
                Cinema.Address,
                Cinema.FilmScreenings.Select(p => p.Title).ToList(), // список названий
                Cinema.FilmScreenings.Sum(p => p.RegisteredCount)     // сумма зарегистрировавшихся
            );

            return Ok(response);
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CinemaRequest request)
        {
            var CinemaId = await _CinemaService.CreateCinema(
                request.Name,
                request.Address
            );

            return Ok(CinemaId);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] CinemaRequest request)
        {
            try
            {
                var CinemaId = await _CinemaService.UpdateCinema(
                    id,
                    request.Name,
                    request.Address
                );

                return Ok(CinemaId);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _CinemaService.DeleteCinema(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Поиск театров по постановке
        [HttpGet("performance/{title}")]
        public async Task<ActionResult<List<CinemaResponse>>> GetByFilmScreening(string title)
        {
            var Cinemas = await _CinemaService.GetCinemasByFilmScreening(title);

            var response = Cinemas.Select(t => new CinemaResponse(
                t.Id,
                t.Name,
                t.Address,
                t.FilmScreenings.Select(p => p.Title).ToList(),
                t.Popularity
            )).ToList();

            return Ok(response);
        }
    }
}
