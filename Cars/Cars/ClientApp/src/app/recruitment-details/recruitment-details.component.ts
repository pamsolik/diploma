import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {RecruitmentDetails} from "../../models/RecruitmentDetails"

@Component({
  selector: 'app-recruitment-details-component',
  templateUrl: './recruitment-details.component.html',
  styleUrls: ['./recruitment-details.component.css']
})
export class RecruitmentDetailsComponent {
  details: RecruitmentDetails;
  id: string = this.route.snapshot.paramMap.get('id');

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    console.log(this.id);
    http.get<RecruitmentDetails>(baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
      this.details = result;
      console.log(this.details);
    }, error => console.error(error));
  }
}
