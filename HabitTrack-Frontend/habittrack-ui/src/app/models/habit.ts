export interface Habit {
  habitId: number;
  name: string;
  description: string | null;
  createdAt: string;
}

export interface CreateHabitRequest {
  name: string;
  description: string;
}

export interface UpdateHabitRequest {
  name: string;
  description: string;
}