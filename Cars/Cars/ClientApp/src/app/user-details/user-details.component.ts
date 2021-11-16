import {Component, Inject, Input} from '@angular/core';

import {HttpClient} from '@angular/common/http';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";

import {BaseValues} from "../../models/enums/BaseValues";
import {UserAdminView} from "../../models/UserList";


@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
})
export class UserDetailsComponent {

  @Input()
  userAdminView: UserAdminView;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) { }

  open(content: any) {
    let settings = {
      ariaLabelledBy: 'modal-basic-title',
      centered: true,
      size: 'xl',
    }
    this.modalService.open(content, settings);//.result.then(result => {});
    console.log(this.userAdminView);
  }

  close() {
    this.modalService.dismissAll();
  }

  createImgPath() {
    let pp = this.userAdminView.profilePicture
    if (!pp) pp = BaseValues.BaseProfileUrl
    return `${this.baseUrl}${pp}`;
  }
}
