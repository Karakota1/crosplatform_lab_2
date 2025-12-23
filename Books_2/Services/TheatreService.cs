using Books_2.Contracts;
using Books_2.Data;
using Books_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_2.Services
{
    public class CinemaService
    {
        private readonly AppDbContext _db;

        public CinemaService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CinemaResponse>> GetAllAsync()

        {
            var Cinemas = await _db.Cinemas
                .Include(t => t.FilmScreenings)
                .ToListAsync();

            return Cinemas.Select(t => new CinemaResponse(
                 t.Id,
                 t.Name,
                 t.Address,
                 t.FilmScreenings.Select(p => p.Title).ToList(),
                 t.FilmScreenings.Sum(p => p.RegisteredCount)
             )).ToList();
        }

        public async Task<Cinema?> GetByIdAsync(Guid id)
        {
            return await _db.Cinemas
                .Include(t => t.FilmScreenings) // важно включить FilmScreenings
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<Guid> CreateCinema(string name, string address)
        {
            var Cinema = new Cinema
            {
                Name = name,
                Address = address
            };

            _db.Cinemas.Add(Cinema);
            await _db.SaveChangesAsync();
            return Cinema.Id;
        }

        public async Task<Guid> UpdateCinema(Guid id, string name, string address)
        {
            var Cinema = await _db.Cinemas.FindAsync(id);
            if (Cinema == null)
                throw new KeyNotFoundException("Cinema not found");

            Cinema.Name = name;
            Cinema.Address = address;

            await _db.SaveChangesAsync();
            return Cinema.Id;
        }

        public async Task DeleteCinema(Guid id)
        {
            var Cinema = await _db.Cinemas.FindAsync(id);
            if (Cinema == null)
                throw new KeyNotFoundException("Cinema not found");

            _db.Cinemas.Remove(Cinema);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Cinema>> GetCinemasByFilmScreening(string title)
        {
            return await _db.Cinemas
                .Include(t => t.FilmScreenings)
                .Where(t => t.FilmScreenings.Any(p => p.Title.Contains(title)))
                .ToListAsync();
        }
    }
}