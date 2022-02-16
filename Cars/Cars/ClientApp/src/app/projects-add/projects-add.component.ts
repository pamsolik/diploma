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
  selector: 'app-projects-add',
  templateUrl: './projects-add.component.html',
  styleUrls: ['./projects-add.component.scss'],
})
export class ProjectsAddComponent implements OnInit {
  projects: ProjectDto[] = [];

  autoTitle: boolean = false;
  sameDesc: boolean = false;
  sameTech: boolean = false;

  technologies: string[] = Object.values(Technology);
  technology: string[] = [];

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) {
  }

  ngOnInit() {
  }

  apply() {
    // this.projects.recruitmentId = this.details.id;
    // this.projects.projects.forEach((value, index) =>
    //   value.technology = getEnumKeyByEnumValue(Technology, this.technology[index]));

    this.alertService.showLoading("Dodawanie projektów");
    console.log(this.projects);
    this.http.put<ApiAnswer>(`${this.baseUrl}api/projects`, this.projects).subscribe(result => {
      this.close();
      console.log(result);
      this.alertService.showResultAndRedirect("Gratulacje", "Dodano projekty", '/projects');
    }, error => {
      this.alertService.showError(error);
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
    this.projects.splice(i, 1);
    this.technology.splice(i, 1);
  }

  addProject() {
      this.projects.push({description: '', title: '', url: '', technology: 0} as ProjectDto);
      this.technology.push('');
  }
}
