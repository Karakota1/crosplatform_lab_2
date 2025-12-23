using System;
using System.ComponentModel.DataAnnotations;

namespace Books_2.Models
{
    public class FilmScreening
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Genre { get; set; } = string.Empty;

        // Количество зарегистрировавшихся людей
        public int RegisteredCount { get; set; } = 0;

        [Required]
        public Guid CinemaId { get; set; }

        [Required]
        public Cinema Cinema { get; set; } = null!;

        // Дата и время выступления
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }

    }
}
