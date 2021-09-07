import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {RouterModule} from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {RecruitmentComponent} from './recruitment/recruitment.component';
import {RecruiterComponent} from './recruiter/recruiter.component';
import {ApiAuthorizationModule} from 'src/api-authorization/api-authorization.module';
import {AuthorizeGuard} from 'src/api-authorization/authorize.guard';
import {AuthorizeInterceptor} from 'src/api-authorization/authorize.interceptor';
import {FooterComponent} from "./footer/footer.component";
import {MatCardModule} from "@angular/material/card";
import {RecruitmentDetailsComponent} from "./recruitment-details/recruitment-details.component";
import {RecruitmentSettingsComponent} from "./recruitment-settings/recruitment-settings.component";
import {MatTabsModule} from "@angular/material/tabs";
import {MatPaginatorModule} from "@angular/material/paginator";
import {AdminComponent} from "./admin/admin.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RecruitmentComponent,
    RecruiterComponent,
    AdminComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'recruitments', component: RecruitmentComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitments/:id', component: RecruitmentDetailsComponent, canActivate: [AuthorizeGuard]},

      {path: 'recruiter', component: RecruiterComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruiter/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},

      {path: 'admin', component: AdminComponent, canActivate: [AuthorizeGuard]},
    ]),
    MatCardModule,
    MatTabsModule,
    MatPaginatorModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
