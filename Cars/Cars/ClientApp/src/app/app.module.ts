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
import {FooterComponent} from './footer/footer.component';
import {MatCardModule} from '@angular/material/card';
import {RecruitmentDetailsComponent} from './recruitment-details/recruitment-details.component';
import {RecruitmentSettingsComponent} from './recruitment-settings/recruitment-settings.component';
import {MatTabsModule} from '@angular/material/tabs';
import {MatPaginatorIntl, MatPaginatorModule} from '@angular/material/paginator';
import {AdminComponent} from './admin/admin.component';
import {CommonModule} from '@angular/common';
import {MatPaginatorIntlCustom} from '../components/MatPaginatorIntlCustom';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatSelectModule} from '@angular/material/select';
import {MatInputModule} from '@angular/material/input';
import {FileUploadComponent} from './file-upload/file-upload.component';
import {MatSliderModule} from '@angular/material/slider';
import {NgxSliderModule} from '@angular-slider/ngx-slider';
import {ApplyComponent} from './apply/apply.component';
import {RecruitmentListComponent} from './recruitment-list/recruitment-list.component';
import {ApplicationsComponent} from './applications/applications.component';
import {NgbdSortableHeader} from "../components/NgbdSortableHeader";
import {ApplicationDetailsComponent} from "./application-details/application-details.component";
import {MatTableModule} from "@angular/material/table";
import {MatSortModule} from "@angular/material/sort";
import {SortDirective} from "../directives/sort-directive";
import {UserListComponent} from "./user-list/user-list.component";
import {UserDetailsComponent} from "./user-details/user-details.component";
import {ProjectsDetailsComponent} from "./projects-details/projects-details.component";
import {NgCircleProgressModule} from 'ng-circle-progress';
import {GooglePlaceModule} from "ngx-google-places-autocomplete";
import { GeoapifyGeocoderAutocompleteModule } from '@geoapify/angular-geocoder-autocomplete';
import { ProjectsListComponent } from './projects-list/projects-list.component';
import { ProjectsAddComponent } from './projects-add/projects-add.component';
import { MatNativeDateModule } from '@angular/material/core';

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
    RecruitmentListComponent,
    FileUploadComponent,
    ApplicationsComponent,
    ApplyComponent,
    NgbdSortableHeader,
    ApplicationDetailsComponent,
    SortDirective,
    UserListComponent,
    ProjectsListComponent,
    ProjectsAddComponent,
    UserDetailsComponent,
    ProjectsDetailsComponent
  ],
  imports: [
    BrowserModule, // .withServerTransition({appId: 'ng-cli-universal'}),
    CommonModule,
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    BrowserAnimationsModule,
    NgCircleProgressModule,
    GeoapifyGeocoderAutocompleteModule.withConfig('7f8147a4c77d4c519e057c1cda94409f'),
    NgCircleProgressModule.forRoot({
      radius: 100,
      outerStrokeWidth: 16,
      innerStrokeWidth: 8,
      outerStrokeColor: "#78C000",
      innerStrokeColor: "#C7E596",
      animationDuration: 300,
    }),

    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'recruitments', component: RecruitmentComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-details/:id', component: RecruitmentDetailsComponent, canActivate: [AuthorizeGuard]},

      {path: 'projects', component: ProjectsListComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruiter', component: RecruiterComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruiter/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-settings/:id', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-settings', component: RecruitmentSettingsComponent, canActivate: [AuthorizeGuard]},
      {path: 'recruitment-applications/:id', component: ApplicationsComponent, canActivate: [AuthorizeGuard]},
      {path: 'admin', component: AdminComponent, canActivate: [AuthorizeGuard]},
    ]),
    MatCardModule,
    MatTabsModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatInputModule,
    MatSliderModule,
    NgxSliderModule,
    MatTableModule,
    MatSortModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true},
    {provide: MatPaginatorIntl, useClass: MatPaginatorIntlCustom}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
