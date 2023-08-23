import {Injectable} from '@angular/core';
import {BehaviorSubject} from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    public isHidden: BehaviorSubject<boolean> = new BehaviorSubject(true);

    constructor() {
    }
}
