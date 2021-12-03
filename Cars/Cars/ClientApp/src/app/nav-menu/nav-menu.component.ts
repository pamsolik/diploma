import {Component} from '@angular/core';
import {AuthorizeService} from "../../api-authorization/authorize.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isAuthenticated: Observable<boolean>;


  constructor(public authorizeService: AuthorizeService) {
    this.isAuthenticated = authorizeService.isAuthenticated();
    if (this.isAuthenticated){
      this.authorizeService.checkRoles();
      this.authorizeService.getUserInfo();
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
