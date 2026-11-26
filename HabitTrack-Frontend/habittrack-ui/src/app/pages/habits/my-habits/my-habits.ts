
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { finalize } from 'rxjs';

import { HabitService } from '../../../core/services/habit.service';

import {
  Habit,
  CreateHabitRequest,
  UpdateHabitRequest
} from '../../../models/habit';

@Component({
  selector: 'app-my-habits',
  standalone: true,
  imports: [FormsModule, RouterLink, DatePipe],
  templateUrl: './my-habits.html',
  styleUrl: './my-habits.css'
})
export class MyHabits implements OnInit {

  // Signals for UI state
  habits = signal<Habit[]>([]);

  isLoading = signal(false);
  isSaving = signal(false);

  errorMessage = signal('');
  successMessage = signal('');

  // Normal properties for form values
  habitId: number | null = null;

  name = '';
  description = '';

  isEditing = false;

  constructor(private habitService: HabitService) {}

  ngOnInit(): void {
    this.loadHabits();
  }

  loadHabits(): void {

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.habitService.getAllHabits()
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        })
      )
      .subscribe({

        next: response => {
          this.habits.set(response);
          console.log('Habits loaded:', this.habits());
        },

        error: error => {
          console.error('Error loading habits:', error);
          this.errorMessage.set('Unable to load your habits.');
        }

      });
  }

  saveHabit(): void {

    this.errorMessage.set('');
    this.successMessage.set('');

    const habitData = {
      name: this.name.trim(),
      description: this.description.trim()
    };

    if (!habitData.name) {
      this.errorMessage.set('Please enter a habit name.');
      return;
    }

    this.isSaving.set(true);

    if (this.isEditing && this.habitId !== null) {
      this.updateHabit(this.habitId, habitData);
    } else {
      this.createHabit(habitData);
    }
  }

  createHabit(data: CreateHabitRequest): void {

    this.habitService.createHabit(data).subscribe({

      next: () => {

        this.isSaving.set(false);
        this.successMessage.set('Habit created successfully!');

        this.resetForm();
        this.loadHabits();

      },

      error: error => {

        this.isSaving.set(false);

        this.errorMessage.set(
          error.error?.message || 'Unable to create habit.'
        );

      }

    });
  }

  updateHabit(id: number, data: UpdateHabitRequest): void {

    this.habitService.updateHabit(id, data).subscribe({

      next: () => {

        this.isSaving.set(false);
        this.successMessage.set('Habit updated successfully!');

        this.resetForm();
        this.loadHabits();

      },

      error: error => {

        this.isSaving.set(false);

        this.errorMessage.set(
          error.error?.message || 'Unable to update habit.'
        );

      }

    });
  }

  editHabit(habit: Habit): void {

    this.habitId = habit.habitId;

    this.name = habit.name;
    this.description = habit.description || '';

    this.isEditing = true;

    this.errorMessage.set('');
    this.successMessage.set('');

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  deleteHabit(id: number): void {

    const confirmed = window.confirm(
      'Are you sure you want to delete this habit?'
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.habitService.deleteHabit(id).subscribe({

      next: () => {

        this.successMessage.set('Habit deleted successfully!');

        if (this.habitId === id) {
          this.resetForm();
        }

        this.loadHabits();

      },

      error: () => {
        this.errorMessage.set('Unable to delete habit.');
      }

    });
  }

  resetForm(): void {

    this.habitId = null;

    this.name = '';
    this.description = '';

    this.isEditing = false;

  }

}
