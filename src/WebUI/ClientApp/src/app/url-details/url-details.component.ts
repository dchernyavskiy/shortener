import {Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UrlDto, UrlsClient} from "../web-api-client";

@Component({
  selector: 'app-url-details',
  templateUrl: './url-details.component.html',
  styleUrls: ['./url-details.component.css']
})
export class UrlDetailsComponent {
  url: UrlDto = new UrlDto();

  constructor(private readonly activatedRoute: ActivatedRoute, private readonly urlsClient: UrlsClient) {
    activatedRoute.params.subscribe(res => {
      const id = res["id"]
      console.log(id)
      urlsClient.getDetails(id).subscribe(res => {
        this.url = res.url
      })
    })
  }
}
