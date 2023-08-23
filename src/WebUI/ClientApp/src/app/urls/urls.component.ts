import {Component} from '@angular/core';
import {CreateShortUrl, DeleteUrl, GetUrls, UrlBriefDto, UrlsClient} from "../web-api-client";
import {catchError, Observable} from "rxjs";
import {NotificationService} from "../service/notification.service";
import {AuthorizeService} from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-urls',
  templateUrl: './urls.component.html',
  styleUrls: ['./urls.component.css']
})
export class UrlsComponent {
  createShortUrl: CreateShortUrl = new CreateShortUrl();
  urls: UrlBriefDto[];
  isAuthenticated: Observable<boolean>;

  constructor(private readonly urlsClient: UrlsClient, private readonly notificationService: NotificationService, private readonly authorizeService: AuthorizeService) {
    this.getUrls();
    this.isAuthenticated = authorizeService.isAuthenticated();
  }

  getUrls() {
    this.urlsClient.get(new GetUrls()).pipe(
      catchError((error, res) => {
        if (error.status == 401) {
          console.log('unauthorized');
        }
        return res;
      })
    ).subscribe(res => {
      this.urls = res.urls;
    })
  }

  createUrl() {
    this.urlsClient.create(this.createShortUrl)
      .pipe(
        catchError((error, res) => {
          console.log(error)
          if (error.status == 409) {
            this.notificationService.add({title: error.title});
          }
          throw new Error();
        }))
      .subscribe(res => {
        console.log(res)
        this.getUrls();
      })
  }

  delete(url: UrlBriefDto) {
    this.urlsClient.delete(url).subscribe(res => {
      this.getUrls();
    })
  }
}
