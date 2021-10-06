import {Component, OnInit, OnDestroy, Input, Inject, EventEmitter, Output} from '@angular/core';

import {HttpClient} from '@angular/common/http';
import {Router, ActivatedRoute} from '@angular/router';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ApplicationDto, ApplicationDtoDefault} from "../../models/ApplicationDto";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto";
import {AlertService} from "../../services/alert.service";
import {END} from "@angular/cdk/keycodes";


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

  constructor(private modalService: NgbModal, private alertService: AlertService) {

  }

  public uploadFinishedCv = (event) => {
    this.application.cvFile = event.dbPath;
  }

  public uploadFinishedCl = (event) => {
    this.application.clFile = event.dbPath;
  }

  apply(){
    this.application.recruitmentId = this.details.id;
    console.log(this.application);
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
      if(this.application.projects.find(p => p == proj)){
        this.alertService.showResult("Błąd", "Projekt już istnieje na liście.");
        return;
      }
      this.application.projects.push(proj);
      this.newProj = "";
    } else {
      this.alertService.showResult("Błąd", "Nie można dodać pustego projektu.")
    }
  }
}
