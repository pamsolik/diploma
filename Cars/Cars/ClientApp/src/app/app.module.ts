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
import {MatPaginatorModule, MatPaginatorIntl} from "@angular/material/paginator";
import {AdminComponent} from "./admin/admin.component";
import {CommonModule} from '@angular/common';
import {MatPaginatorIntlCustom} from "../components/MatPaginatorIntlCustom";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatSelectModule} from "@angular/material/select";
import {MatInputModule} from "@angular/material/input";
import {FileUploadComponent} from "./file-upload/file-upload.component";


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RecruitmentComponent,
    RecruitmentDetailsComponent,
    RecruitmentSettingsComponent,
    RecruiterComponent,
    AdminComponent,
    FooterComponent,
    FileUploadComponent
  ],
  imports: [
    BrowserModule,//.withServerTransition({appId: 'ng-cli-universal'}),
    CommonModule,
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'recruitments', component: RecruitmentComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-details/:id', component: RecruitmentDetailsComponent, canActivate: [AuthorizeGuard]},

      {path: 'recruiter', component: RecruiterComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruiter/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-settings/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-settings', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},

      {path: 'admin', component: AdminComponent, canActivate: [AuthorizeGuard]},
    ]),
    MatCardModule,
    MatTabsModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatSelectModule,
    MatInputModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true},
    {provide: MatPaginatorIntl, useClass: MatPaginatorIntlCustom}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
