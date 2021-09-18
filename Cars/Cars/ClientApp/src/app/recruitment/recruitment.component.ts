import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RecruitmentOffer} from "../../models/RecruitmentOffer";
import {Filters, newFilters} from "../../models/Filters";

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent {
  offers: RecruitmentOffer[] = [];
  filters: Filters;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.clearFilters();
    this.applyFilter();
  }

  applyFilter() {
    this.http.post<RecruitmentOffer[]>(this.baseUrl + 'api/recruitments/filtered', this.filters).subscribe(result => {
      this.offers = result;
      console.log(this.offers);
    }, error => console.error(error));
  }

  clearFilters() {
    this.filters = newFilters();
  }

}
