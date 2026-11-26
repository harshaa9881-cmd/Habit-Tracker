import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { DailySummary } from '../../models/habit-record';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HabitRecordService {

  private apiUrl = `${environment.apiUrl}/habitrecord`;

  constructor(private http: HttpClient) {}

  getDailySummary(date: string): Observable<DailySummary> {
    return this.http.get<DailySummary>(
      `${this.apiUrl}?date=${date}`
    );
  }

  markCompleted(habitId: number, date: string): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/${habitId}/complete?date=${date}`,
      {}
    );
  }

  markIncomplete(habitId: number, date: string): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/${habitId}/complete?date=${date}`
    );
  }
}