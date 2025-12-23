namespace Books_2.Contracts
{
    public record FilmScreeningRequest(
    string Title,
    string Description,
    string Genre,
    Guid CinemaId,
    DateTime StartTime,
    int DurationMinutes
    );
    public class UpdateFilmScreeningDateRequest
    {
        public DateTime StartTime { get; set; }
    }


}
