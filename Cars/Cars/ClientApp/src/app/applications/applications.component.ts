import {Component, Inject} from '@angular/core';

import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";
import {RecruitmentApplication} from "../../models/RecruitmentApplication"

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss'],
})
export class ApplicationsComponent {
  id: string = this.route.snapshot.paramMap.get('id');
  recruitmentApplications: RecruitmentApplication[];

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService,
              private route: ActivatedRoute) {

    http.get<RecruitmentApplication[]>(baseUrl + 'api/recruitments/applications/' + this.id).subscribe(result => {
      this.recruitmentApplications = result;
    }, error => console.error(error));
  }

  apply() {
    this.alertService.showLoading("Dodawanie aplikacji");
    //console.log(this.application);
    // this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments/apply`, this.application).subscribe(result => {
    //   this.alertService.showResultAndRedirect("Gratulacje", "Zapisano aplikację", '/recruitments')
    //   console.log(result);
    // }, error => {
    //   this.alertService.showResult("Błąd", error.message)
    //   console.error(error);
    // })
  }
}
