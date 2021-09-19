import {AfterViewInit, Component, Inject, OnInit, ViewChild} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RecruitmentList, RecruitmentOffer} from "../../models/RecruitmentOffer";
import {Filters, newFilters} from "../../models/Filters";
import {MatPaginator, MatPaginatorIntl, PageEvent} from "@angular/material/paginator";
import {tap} from "rxjs/operators";

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent{
  offers: RecruitmentList;
  filters: Filters;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.clearFilters();
    this.loadData();
  }

  applyFilter(pageEvent: PageEvent) {
    this.filters.pageSize = pageEvent.pageSize;
    this.filters.pageIndex = pageEvent.pageIndex;
  }

  loadData() {
    console.log(this.filters.pageIndex);
    this.http.post<RecruitmentList>(this.baseUrl + 'api/recruitments/filtered', this.filters).subscribe(result => {
      this.offers = result;
      console.log(this.offers.items);
    }, error => console.error(error));
  }

  clearFilters() {
    this.filters = newFilters();
  }
}
