using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Frontend.Models;

public class GameDetails
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Title { get; set; }

    [Required(ErrorMessage = "The Genre field is required.")]
    public string? GenreId { get; set; }
    public DateOnly ReleaseDate { get; set; }

    [Range(1, 100)]
    public decimal Price { get; set; }
}
