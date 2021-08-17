import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { RecruitmentComponent } from './recruitment/recruitment.component';
import { RecruiterComponent } from './recruiter/recruiter.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import {FooterComponent} from "./footer/footer.component";
import {MatCardModule} from "@angular/material/card";
import {RecruitmentDetailsComponent} from "./recruitment-details/recruitment-details.component";
import {RecruitmentSettingsComponent} from "./recruitment-settings/recruitment-settings.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RecruitmentComponent,
    RecruiterComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'recruitments', component: RecruitmentComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitments/:id', component: RecruitmentDetailsComponent, canActivate: [AuthorizeGuard]},

      {path: 'recruiter', component: RecruiterComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruiter/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
    ]),
    MatCardModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
