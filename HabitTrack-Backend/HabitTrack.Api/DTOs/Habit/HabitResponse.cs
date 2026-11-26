namespace HabitTrack.Api.DTOs.Habit;

public class HabitResponse
{
    public int HabitId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
}