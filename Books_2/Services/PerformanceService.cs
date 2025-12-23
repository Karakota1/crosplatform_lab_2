using Books_2.Data;
using Books_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Books_2.Services
{
    public class FilmScreeningService
    {
        private readonly AppDbContext _db;

        public FilmScreeningService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<FilmScreening>> GetAllFilmScreenings()
        {
            return await _db.FilmScreenings
                .Include(p => p.Cinema)
                .ToListAsync();
        }

        public async Task<FilmScreening> GetFilmScreeningById(Guid id)
        {
            return await _db.FilmScreenings
                .Include(p => p.Cinema)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Guid> CreateFilmScreening(
            string title,
            string description,
            string genre,
            Guid CinemaId,
            DateTime startTime,
            int durationMinutes)
        {
            // Проверка пересечения
            if (!await IsTimeSlotAvailable(CinemaId, startTime, durationMinutes))
                throw new InvalidOperationException("В это время у театра уже есть другое представление.");

            var perf = new FilmScreening
            {
                Title = title,
                Description = description,
                Genre = genre,
                CinemaId = CinemaId,
                StartTime = startTime,
                DurationMinutes = durationMinutes
            };

            _db.FilmScreenings.Add(perf);
            await _db.SaveChangesAsync();

            return perf.Id;
        }

        public async Task<Guid> UpdateFilmScreening(Guid id, DateTime newStartTime)
        {
            var perf = await _db.FilmScreenings.FindAsync(id);
            if (perf == null)
                throw new KeyNotFoundException("FilmScreening not found");

            // Проверка пересечения
            if (!await IsTimeSlotAvailable(perf.CinemaId, newStartTime, perf.DurationMinutes, perf.Id))
                throw new InvalidOperationException("В это время у театра уже есть другое представление.");

            perf.StartTime = newStartTime;

            await _db.SaveChangesAsync();
            return perf.Id;
        }


        public async Task DeleteFilmScreening(Guid id)
        {
            var perf = await _db.FilmScreenings.FindAsync(id);
            if (perf == null)
                throw new KeyNotFoundException("FilmScreening not found");

            _db.FilmScreenings.Remove(perf);
            await _db.SaveChangesAsync();
        }

        public async Task<List<FilmScreening>> GetFilmScreeningsByCinema(string CinemaName)
        {
            return await _db.FilmScreenings
                .Include(p => p.Cinema)
                .Where(p => p.Cinema.Name.Contains(CinemaName))
                .ToListAsync();
        }


        public async Task<bool> UpdateRegisteredCountAsync(Guid id, int registeredCount)
        {
            var perf = await _db.FilmScreenings.FindAsync(id);
            if (perf == null)
                return false;

            perf.RegisteredCount = registeredCount;
            await _db.SaveChangesAsync();
            return true;
        }

        private async Task<bool> IsTimeSlotAvailable(Guid CinemaId, DateTime startTime, Guid? excludeFilmScreeningId = null)
        {
            return !await _db.FilmScreenings.AnyAsync(p =>
                p.CinemaId == CinemaId &&
                p.StartTime == startTime &&
                (excludeFilmScreeningId == null || p.Id != excludeFilmScreeningId)
            );
        }
        private async Task<bool> IsTimeSlotAvailable(
        Guid CinemaId,
        DateTime newStart,
        int newDuration,
        Guid? excludeId = null)
        {
            var newEnd = newStart.AddMinutes(newDuration);

            return !await _db.FilmScreenings.AnyAsync(p =>
                p.CinemaId == CinemaId &&
                (excludeId == null || p.Id != excludeId) &&

                // Проверка пересечения интервалов
                newStart < p.StartTime.AddMinutes(p.DurationMinutes) &&
                newEnd > p.StartTime
            );
        }

    }
}
