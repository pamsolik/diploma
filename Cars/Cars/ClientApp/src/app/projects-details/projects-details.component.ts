import {Component, Inject, Input} from '@angular/core';

import {HttpClient} from '@angular/common/http';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";
import {RecruitmentApplication} from "../../models/RecruitmentApplication";
import {BaseValues} from "../../models/enums/BaseValues";
import {Technology} from "../../models/enums/Technology";
import {ProjectDto} from "../../models/ProjectDto";
import {CodeOverallQuality} from "../../models/CodeOverallQuality";


@Component({
  selector: 'app-projects-details-details',
  templateUrl: './projects-details.component.html',
  styleUrls: ['./projects-details.component.scss'],
})
export class ProjectsDetailsComponent {

  @Input()
  details: RecruitmentApplication;
  technologies: string[] = Object.values(Technology);
  isCoq: boolean = false;
  codeQuality: CodeOverallQuality;
  project: ProjectDto;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string) {
  }

  openProject(content: any, project: number) {
    this.openModal(content);
    this.isCoq = false;
    this.project = this.details.projects[project];
    this.codeQuality = this.project.codeQualityAssessment;
  }

  openCoq(content: any) {
    this.openModal(content);
    this.isCoq = true;
    this.codeQuality = this.details.codeOverallQuality;
  }

  openModal(content: any){
    let settings = {
      ariaLabelledBy: 'modal-basic-title',
      centered: true,
      size: 'xl',
    }
    this.modalService.open(content, settings);
    console.log(this.details);
  }

  close() {
    this.modalService.dismissAll();
  }
}
