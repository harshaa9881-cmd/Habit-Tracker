using System.ComponentModel.DataAnnotations;

namespace HabitTrack.Api.DTOs.Habit;

public class UpdateHabitRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Description { get; set; }
}