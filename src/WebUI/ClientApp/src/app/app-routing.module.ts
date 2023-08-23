import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AuthorizeGuard} from '../api-authorization/authorize.guard';
import {UrlsComponent} from "./urls/urls.component";

export const routes: Routes = [
  {path: '', redirectTo: 'urls', pathMatch: 'full'},
  {path: 'urls', component: UrlsComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
