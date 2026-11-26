export interface DailyHabit {
  habitId: number;
  name: string;
  description: string | null;
  isCompleted: boolean;
}

export interface DailySummary {
  date: string;
  totalHabits: number;
  completedHabits: number;
  remainingHabits: number;
  completionPercentage: number;
  habits: DailyHabit[];
}