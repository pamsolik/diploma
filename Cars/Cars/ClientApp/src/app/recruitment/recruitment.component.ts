import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent {
  offers: RecruitmentOffer[] = [];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<RecruitmentOffer[]>(baseUrl + 'api/recruitments').subscribe(result => {
      this.offers = result;
      console.log(this.offers);
    }, error => console.error(error));
  }

}

interface RecruitmentOffer {
  id :number,
  title: string,
  description: string,
  startDate: Date,
  status: number,
  type: number,
  jobType: string,
  recruiter: any
}
