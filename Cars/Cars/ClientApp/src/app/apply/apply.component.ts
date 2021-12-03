import {Component, Inject, Input, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ApplicationDto, ApplicationDtoDefault} from "../../models/ApplicationDto";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";
import {AlertService} from "../../services/alert.service";
import {ApiAnswer} from "../../models/ApiAnswer";
import {ProjectDto} from "../../models/ProjectDto";
import {Technology} from "../../models/enums/Technology";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";

@Component({
  selector: 'app-apply',
  templateUrl: './apply.component.html',
  styleUrls: ['./apply.component.scss'],
})
export class ApplyComponent implements OnInit {
  application: ApplicationDto;

  technologies: string[] = Object.values(Technology);
  technology: string[] = [];

  @Input()
  details: RecruitmentDetailsView;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) {
  }

  ngOnInit() {
    this.application = ApplicationDtoDefault();
    this.addProject()
  }

  public uploadFinishedCv = (event) => {
    this.application.cvFile = event.dbPath;
  }

  public uploadFinishedCl = (event) => {
    this.application.clFile = event.dbPath;
  }

  apply() {
    this.application.recruitmentId = this.details.id;
    this.application.projects.forEach((value, index) =>
      value.technology = getEnumKeyByEnumValue(Technology, this.technology[index]));

    this.alertService.showLoading("Dodawanie aplikacji");
    console.log(this.application);
    this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments/apply`, this.application).subscribe(result => {
      this.close();
      console.log(result);
      this.alertService.showResultAndRedirect("Gratulacje", "Zapisano aplikację", '/recruitments');
    }, error => {
      this.alertService.showResult("Błąd", error.message)
      console.error(error);
    });
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
    this.technology.splice(i, 1);
  }

  addProject() {
    if (this.application.projects.length < 5) {
      this.application.projects.push({description: '', title: '', url: '', technology: 0} as ProjectDto);
      this.technology.push('');
    } else {
      this.alertService.showResult("Błąd", "Nie można dodać więcej niż 5 projektów.")
    }
  }
}
