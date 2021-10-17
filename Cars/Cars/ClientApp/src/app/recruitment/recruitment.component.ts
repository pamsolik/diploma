import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

}
