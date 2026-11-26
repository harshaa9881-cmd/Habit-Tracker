
import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthService } from '../../core/services/auth.service';
import { HabitRecordService } from '../../core/services/habit-record.service';

import {
  DailyHabit,
  DailySummary
} from '../../models/habit-record';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {

  userName = localStorage.getItem('habitTrackUser') || 'User';

  // Signals
  summary = signal<DailySummary | null>(null);

  selectedDate = signal(this.getLocalDate());

  isLoading = signal(false);

  updatingHabitIds = signal<number[]>([]);

  errorMessage = signal('');

  successMessage = signal('');

  constructor(
    private authService: AuthService,
    private router: Router,
    private habitRecordService: HabitRecordService
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  getLocalDate(): string {

    const today = new Date();

    const year = today.getFullYear();

    const month = String(today.getMonth() + 1).padStart(2, '0');

    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  loadDashboard(): void {

    this.isLoading.set(true);

    this.errorMessage.set('');

    this.habitRecordService
      .getDailySummary(this.selectedDate())
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        })
      )
      .subscribe({

        next: response => {

          this.summary.set(response);

        },

        error: error => {

          console.error('Error loading dashboard:', error);

          this.errorMessage.set(
            'Unable to load your daily progress.'
          );

        }

      });
  }

  toggleHabit(habit: DailyHabit): void {

    if (this.updatingHabitIds().includes(habit.habitId)) {
      return;
    }

    this.errorMessage.set('');

    this.successMessage.set('');

    this.updatingHabitIds.update(ids => [
      ...ids,
      habit.habitId
    ]);

    const request = habit.isCompleted
      ? this.habitRecordService.markIncomplete(
          habit.habitId,
          this.selectedDate()
        )
      : this.habitRecordService.markCompleted(
          habit.habitId,
          this.selectedDate()
        );

    request
      .pipe(
        finalize(() => {

          this.updatingHabitIds.update(ids =>
            ids.filter(id => id !== habit.habitId)
          );

        })
      )
      .subscribe({

        next: () => {

          this.loadDashboard();

        },

        error: error => {

          console.error('Error updating habit:', error);

          this.errorMessage.set(
            'Unable to update habit completion.'
          );

        }

      });
  }

  changeDate(date: string): void {

    if (!date || date > this.getLocalDate()) {
      return;
    }

    this.selectedDate.set(date);

    this.loadDashboard();

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}
