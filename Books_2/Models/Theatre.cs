using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Books_2.Models
{
    public class Cinema
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string? Address { get; set; }

        // Популярность = сумма проданных билетов всех постановок
        public int Popularity => FilmScreenings.Sum(p => p.RegisteredCount);

        public List<FilmScreening> FilmScreenings { get; set; } = new();
    }
}
