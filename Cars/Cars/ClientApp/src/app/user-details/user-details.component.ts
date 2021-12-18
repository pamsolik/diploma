import {Component, Inject, Input, OnInit} from '@angular/core';

import {HttpClient} from '@angular/common/http';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";

import {BaseValues} from "../../models/enums/BaseValues";
import {UserAdminView} from "../../models/UserList";
import {EditRolesDto} from "../../models/EditRoleDto";
import {UserRoles} from "../../models/enums/UserRoles";


@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss'],
})
export class UserDetailsComponent implements OnInit {

  @Input()
  userAdminView: UserAdminView;
  userRoles: string[];

  userRolesEnum = Object.values(UserRoles);
  newRole: string;

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService) {
  }

  ngOnInit() {
    this.loadUserRoles();
  }

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

  removeUserRole(role: string) {
    this.alertService.showYesNo("Usunąć role użytkownika?").then(
      result => {
        if (result.isConfirmed) {
          const arg: EditRolesDto = {userId: this.userAdminView.id, role: role}
          this.http.put<boolean>(this.baseUrl + 'api/admin/roles/remove', arg).subscribe(result => {
            console.log('Result: ' + result);
            this.loadUserRoles();
          }, error => this.alertService.showError(error));
        }
      }
    )
  }

  addUserRole(role: string) {
    this.alertService.showYesNo("Dodać rolę: " + role + " dla użytkownika?").then(
      result => {
        if (result.isConfirmed) {
          const arg: EditRolesDto = {userId: this.userAdminView.id, role: role}
          this.http.put<boolean>(this.baseUrl + 'api/admin/roles/add', arg).subscribe(result => {
            console.log('Result: ' + result);
            this.loadUserRoles();
          }, error => this.alertService.showError(error));
        }
      }
    )
  }

  private loadUserRoles() {
    this.http.get<string[]>(this.baseUrl + 'api/admin/roles/' + this.userAdminView.id).subscribe(result => {
      this.userRoles = result;
    }, error => console.error(error));
  }

}
