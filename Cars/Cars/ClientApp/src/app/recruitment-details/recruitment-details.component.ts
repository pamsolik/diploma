import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView"

@Component({
  selector: 'app-recruitment-details-component',
  templateUrl: './recruitment-details.component.html',
  styleUrls: ['./recruitment-details.component.css']
})
export class RecruitmentDetailsComponent {
  details: RecruitmentDetailsView;
  id: string = this.route.snapshot.paramMap.get('id');

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    http.get<RecruitmentDetailsView>(baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
      this.details = result;
      console.log(this.details);
    }, error => console.error(error));
  }
}
