import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AuthorizeGuard} from '../api-authorization/authorize.guard';
import {UrlsComponent} from "./urls/urls.component";
import {UrlDetailsComponent} from "./url-details/url-details.component";

export const routes: Routes = [
  {path: '', redirectTo: 'urls', pathMatch: 'full'},
  {path: 'urls', component: UrlsComponent},
  {path: 'urls/:id', component: UrlDetailsComponent, canActivate: [AuthorizeGuard]},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
