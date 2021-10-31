import {Component, Inject, OnInit} from '@angular/core';
import {AuthorizeService} from '../authorize.service';
import {Observable} from 'rxjs';
import {map} from 'rxjs/operators';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public displayProfile: boolean = false;

  constructor(private authorizeService: AuthorizeService, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
  }

  mouseEnter() {
    this.displayProfile = true;
  }

  mouseLeave() {
    this.displayProfile = false;
  }

  createImgPath() {
    return `${this.baseUrl}${this.authorizeService.ProfileUrl}`;
  }
}
