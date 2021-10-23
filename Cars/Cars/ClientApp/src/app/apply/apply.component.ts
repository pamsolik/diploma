import {Component, Inject, Input} from '@angular/core';

import {HttpClient} from '@angular/common/http';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ApplicationDto, ApplicationDtoDefault} from "../../models/ApplicationDto";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";
import {AlertService} from "../../services/alert.service";
import {ApiAnswer} from "../../models/ApiAnswer";
import {ProjectDto} from "../../models/ProjectDto";


@Component({
  selector: 'app-apply',
  templateUrl: './apply.component.html',
  styleUrls: ['./apply.component.scss'],
})
export class ApplyComponent {
  application: ApplicationDto = ApplicationDtoDefault;
  newProj: string;

  @Input()
  details: RecruitmentDetailsView;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) {

  }

  public uploadFinishedCv = (event) => {
    this.application.cvFile = event.dbPath;
  }

  public uploadFinishedCl = (event) => {
    this.application.clFile = event.dbPath;
  }

  apply() {
    this.application.recruitmentId = this.details.id;
    this.alertService.showLoading("Dodawanie aplikacji");
    console.log(this.application);
    this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments/apply`, this.application).subscribe(result => {
      this.alertService.showResultAndRedirect("Gratulacje", "Zapisano aplikację", '/recruitments')
      console.log(result);
    }, error => {
      this.alertService.showResult("Błąd", error.message)
      console.error(error);
    })


  }

  open(content: any) {
    let settings = {
      ariaLabelledBy: 'modal-basic-title',
      centered: true,
      size: 'xl',
    }

    this.modalService.open(content, settings);//.result.then(result => {});
  }

  close() {
    this.modalService.dismissAll();
  }

  deleteProject(i: number) {
    this.application.projects.splice(i, 1);
  }

  addProject(proj: string) {
    if (proj) {
      if (this.application.projects.find(p => p.Url == proj)) {
        this.alertService.showResult("Błąd", "Projekt już istnieje na liście.");
        return;
      }
      this.application.projects.push({Description: 'desc', Title: 'title', Url: proj} as ProjectDto);
      this.newProj = "";
    } else {
      this.alertService.showResult("Błąd", "Nie można dodać pustego projektu.")
    }
  }
}
