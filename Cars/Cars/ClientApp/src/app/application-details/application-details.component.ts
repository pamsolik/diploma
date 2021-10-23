import {Component, Inject, Input} from '@angular/core';

import {HttpClient} from '@angular/common/http';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ApplicationDto, ApplicationDtoDefault} from "../../models/ApplicationDto";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";
import {AlertService} from "../../services/alert.service";
import {ApiAnswer} from "../../models/ApiAnswer";
import {RecruitmentApplication} from "../../models/RecruitmentApplication";


@Component({
  selector: 'app-application-details',
  templateUrl: './application-details.component.html',
  styleUrls: ['./application-details.component.scss'],
})
export class ApplicationDetailsComponent {

  newProj: string;

  @Input()
  details: RecruitmentApplication;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) {


  }

  apply() {
    // this.application.recruitmentId = this.details.id;
    // this.alertService.showLoading("Dodawanie aplikacji");
    // console.log(this.application);
    // this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments/apply`, this.application).subscribe(result => {
    //   this.alertService.showResultAndRedirect("Gratulacje", "Zapisano aplikację", '/recruitments')
    //   console.log(result);
    // }, error => {
    //   this.alertService.showResult("Błąd", error.message)
    //   console.error(error);
    // })
  }

  open(content: any) {
    let settings = {
      ariaLabelledBy: 'modal-basic-title',
      centered: true,
      size: 'xl',
    }
    this.modalService.open(content, settings);//.result.then(result => {});
    console.log(this.details);
  }

  close() {
    this.modalService.dismissAll();
  }
}
