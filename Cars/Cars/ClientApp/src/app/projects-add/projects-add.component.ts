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

  aTitle: string = "";
  sDesc: string = "";
  sTech: string = "";

  projectsToAdd: string = "";

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
    this.projects.forEach((value, index) =>
       value.technology = getEnumKeyByEnumValue(Technology, this.technology[index]));

    this.updateAllProjects();
    this.alertService.showLoading("Dodawanie projektów");
    this.http.put<ApiAnswer>(`${this.baseUrl}api/projects`, this.projects).subscribe(result => {
      this.close();
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

  addProjects(){
    let urls = this.projectsToAdd.split(/\r?\n/);

    for (let p of urls){
      if (p.trim().length > 0){
        let sp = p.split(';');
        if (sp.length < 2){
          this.projects.push({description: '', title: '', url: p, technology: 0} as ProjectDto);
          this.technology.push('');
        }
        else{
          let n = Number(sp[1]);
          if (n > this.technologies.length || n < 0) n = 0;          
          this.projects.push({description: '', title: '', url: sp[0], technology: Number(sp[1])} as ProjectDto);
          this.technology.push(this.technologies[n]);
        }
      }
    }
    
    this.projectsToAdd = '';
    this.updateAllProjects();
  }

  addProject() {
    this.projects.push({description: '', title: '', url: '', technology: 0} as ProjectDto);
    this.technology.push('');
    this.updateAllProjects();
  }

  updateAllProjects() {
    for (let i = 0; i < this.projects.length; i++) {
      if (this.sameDesc)
        this.projects[i].description = this.sDesc;
      if (this.autoTitle)
        this.projects[i].title = this.getAutoTitle(this.projects[i].url);
      if (this.sameTech){
          this.technology[i] = this.sTech;
      }
    }
  }

  getAutoTitle(url : string) : string {
    return url.replace('https://github.com/', '');;
  }
}
