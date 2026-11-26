using System.ComponentModel.DataAnnotations;

namespace HabitTrack.Api.Models;

public class HabitRecord
{
    public int HabitRecordId { get; set; }

    public int HabitId { get; set; }

    public int UserId { get; set; }

    public DateOnly CompletionDate { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}