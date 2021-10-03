import {AfterViewInit, Component, Inject, OnInit, ViewChild} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {RecruitmentList, RecruitmentOffer} from "../../models/RecruitmentOffer";
import {Filters, newFilters} from "../../models/Filters";
import {MatPaginator, MatPaginatorIntl, PageEvent} from "@angular/material/paginator";
import {SortOrder} from "../../models/enums/SortOrder";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {Options} from "@angular-slider/ngx-slider";

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent{
  enums: RecruitmentEnums = new RecruitmentEnums();
  sortOrders = Object.values(SortOrder);
  sortOrder: string = SortOrder.NameAsc;
  offers: RecruitmentList;
  filters: Filters;


  options: Options = {
    floor: 0,
    ceil: 100,
    step: 5,
    showTicks: false
    // ,
    // translate: (value: number): string => {
    //   return value + 'km';
    // }
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filters = newFilters();
    this.clearFilters();
    this.loadData();
  }

  applyFilter(pageEvent: PageEvent) {
    this.filters.pageSize = pageEvent.pageSize;
    this.filters.pageIndex = pageEvent.pageIndex;
  }

  loadData() {
    this.filters.sortOrder = getEnumKeyByEnumValue(SortOrder, this.sortOrder);
    this.http.post<RecruitmentList>(this.baseUrl + 'api/recruitments/filtered', this.filters).subscribe(result => {
      this.offers = result;
    }, error => console.error(error));
  }

  clearFilters() {
    console.log(this.filters);
    //this.filters = newFilters();
  }

  public createImgPath = (path: string) => {
    return `${this.baseUrl}${path}`;
  }
}
