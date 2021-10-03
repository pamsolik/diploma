import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ActivatedRoute} from "@angular/router";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView"
import {JobLevel} from "../../models/enums/JobLevel";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";


@Component({
  selector: 'app-recruitment-details-component',
  templateUrl: './recruitment-details.component.html',
  styleUrls: ['./recruitment-details.component.css']
})
export class RecruitmentDetailsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();
  details: RecruitmentDetailsView;
  id: string = this.route.snapshot.paramMap.get('id');

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute) {
    http.get<RecruitmentDetailsView>(baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
      this.details = result;
    }, error => console.error(error));
  }

  public createImgPath = (path: string) => {
    return `${this.baseUrl}${path}`;
  }
}
