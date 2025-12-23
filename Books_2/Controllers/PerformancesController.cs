using Books_2.Contracts;
using Books_2.Models;
using Books_2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Books_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmScreeningsController : ControllerBase
    {
        private readonly FilmScreeningService _performanceService;

        public FilmScreeningsController(FilmScreeningService performanceService)
        {
            _performanceService = performanceService;
        }

        // ---------------------------------------------------------
        // GET ALL
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<FilmScreeningResponse>>> GetAll()
        {
            var performances = await _performanceService.GetAllFilmScreenings();

            var response = performances
                .Select(p => new FilmScreeningResponse(
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Genre,
                    p.RegisteredCount,    // ← теперь RegisteredCount выводится
                    p.Cinema.Name,
                    p.StartTime,
                    p.DurationMinutes
                ))
                .ToList();

            return Ok(response);
        }

        // ---------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FilmScreeningResponse>> GetById(Guid id)
        {
            var perf = await _performanceService.GetFilmScreeningById(id);
            if (perf == null)
                return NotFound($"FilmScreening with ID {id} not found.");

            var response = new FilmScreeningResponse(
                perf.Id,
                perf.Title,
                perf.Description,
                perf.Genre,
                perf.RegisteredCount,  // ← RegisteredCount
                perf.Cinema.Name,
                perf.StartTime,
                perf.DurationMinutes
            );

            return Ok(response);
        }

        // ---------------------------------------------------------
        // CREATE
        // ---------------------------------------------------------
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] FilmScreeningRequest request)
        {
            var perfId = await _performanceService.CreateFilmScreening(
                request.Title,
                request.Description,
                request.Genre,
                request.CinemaId,
                request.StartTime,
                request.DurationMinutes
            );

            return Ok(perfId);
        }

        // ---------------------------------------------------------
        // UPDATE (для редактирования основных данных)
        // ---------------------------------------------------------
        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> Update(Guid id, [FromBody] DateTime startTime)
        {
            try
            {
                var perfId = await _performanceService.UpdateFilmScreening(
                    id,
                    startTime
                );

                return Ok(perfId);
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

        // ---------------------------------------------------------
        // UPDATE RegisteredCount (только для авторизованных пользователей)
        // ---------------------------------------------------------
        [Authorize]
        [HttpPut("{id:guid}/register")]
        public async Task<ActionResult> UpdateRegisteredCount(Guid id, [FromBody] UpdateRegisteredCountRequest request)
        {
            if (request.RegisteredCount < 0)
                return BadRequest("Количество зарегистрировавшихся не может быть отрицательным.");

            var success = await _performanceService.UpdateRegisteredCountAsync(id, request.RegisteredCount);
            if (!success) return NotFound("Выступление не найдено.");

            return Ok("Количество зарегистрировавшихся обновлено.");
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------
        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _performanceService.DeleteFilmScreening(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ---------------------------------------------------------
        // FIND BY THEATRE NAME
        // ---------------------------------------------------------
        [HttpGet("Cinema/{CinemaName}")]
        public async Task<ActionResult<List<FilmScreeningResponse>>> GetByCinema(string CinemaName)
        {
            var performances = await _performanceService.GetFilmScreeningsByCinema(CinemaName);

            var response = performances.Select(p => new FilmScreeningResponse(
                p.Id,
                p.Title,
                p.Description,
                p.Genre,
                p.RegisteredCount,   // ← RegisteredCount
                p.Cinema.Name,
                p.StartTime,
                p.DurationMinutes
            )).ToList();

            return Ok(response);
        }
    }
}
