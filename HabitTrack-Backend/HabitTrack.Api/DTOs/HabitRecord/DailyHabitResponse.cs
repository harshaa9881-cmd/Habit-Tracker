namespace HabitTrack.Api.DTOs.HabitRecord;

public class DailyHabitResponse
{
    public int HabitId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}