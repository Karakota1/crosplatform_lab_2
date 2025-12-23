using System;

namespace Books_2.Contracts
{
    public record FilmScreeningResponse(
    Guid Id,
    string Title,
    string Description,
    string Genre,
    int RegisteredCount,
    string CinemaName,
    DateTime StartTime,
    int DurationMinutes
);

}
