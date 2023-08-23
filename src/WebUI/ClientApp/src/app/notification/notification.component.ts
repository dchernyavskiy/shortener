import {Component} from '@angular/core';
import {BehaviorSubject} from "rxjs";
import {Notification, NotificationService} from "../service/notification.service";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent {
  notifications: Notification[];

  constructor(private readonly notificationService: NotificationService) {
    this.notifications = notificationService.notifications;
  }

  close(notification: Notification) {
    this.notificationService.close(notification);
  }
}
