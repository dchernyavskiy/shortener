import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  get notifications(): Notification[] {
    return this._notifications;
  }

  private _notifications: Notification[] = [];

  add(notification: Notification) {
    this._notifications.push(notification)
  }

  close(notification: Notification) {
    this._notifications.splice(this._notifications.indexOf(notification), 1);
  }

  constructor() {
  }
}

export interface Notification {
  title: string
}
