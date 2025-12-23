using System;
using System.Collections.Generic;

namespace Books_2.Contracts
{
    public record CinemaResponse(
        Guid Id,
        string Name,
        string Address,
        List<string> FilmScreenings,
        int Popularity
    );
}
