import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Habit,
  CreateHabitRequest,
  UpdateHabitRequest
} from '../../models/habit';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HabitService {

  private apiUrl = `${environment.apiUrl}/habit`;

  constructor(private http: HttpClient) {}

  getAllHabits(): Observable<Habit[]> {
    return this.http.get<Habit[]>(this.apiUrl);
  }

  getHabitById(id: number): Observable<Habit> {
    return this.http.get<Habit>(`${this.apiUrl}/${id}`);
  }

  createHabit(data: CreateHabitRequest): Observable<Habit> {
    return this.http.post<Habit>(this.apiUrl, data);
  }

  updateHabit(
    id: number,
    data: UpdateHabitRequest
  ): Observable<Habit> {
    return this.http.put<Habit>(
      `${this.apiUrl}/${id}`,
      data
    );
  }

  deleteHabit(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}