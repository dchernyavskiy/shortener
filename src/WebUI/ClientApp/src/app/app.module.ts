import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';

import {ModalModule} from 'ngx-bootstrap/modal';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';

import {ApiAuthorizationModule} from 'src/api-authorization/api-authorization.module';
import {AuthorizeInterceptor} from 'src/api-authorization/authorize.interceptor';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NotificationComponent} from './notification/notification.component';
import {UrlsComponent} from './urls/urls.component';
import { UrlDetailsComponent } from './url-details/url-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    NotificationComponent,
    UrlsComponent,
    UrlDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ModalModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
