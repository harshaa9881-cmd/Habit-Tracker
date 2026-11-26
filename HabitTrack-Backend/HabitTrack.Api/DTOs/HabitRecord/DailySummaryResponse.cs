namespace HabitTrack.Api.DTOs.HabitRecord;

public class DailySummaryResponse
{
    public DateOnly Date { get; set; }

    public int TotalHabits { get; set; }

    public int CompletedHabits { get; set; }

    public int RemainingHabits { get; set; }

    public int CompletionPercentage { get; set; }

    public List<DailyHabitResponse> Habits { get; set; } = new();
}