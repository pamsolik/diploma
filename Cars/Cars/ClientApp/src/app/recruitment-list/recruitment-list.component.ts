import {Component, Inject, Input, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";

import {Filters, FiltersDefault} from "../../models/Filters";
import {PageEvent} from "@angular/material/paginator";
import {SortOrder} from "../../models/enums/SortOrder";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {Options} from "@angular-slider/ngx-slider";
import {RecruitmentList} from "../../models/RecruitmentList";

@Component({
  selector: 'app-recruitment-list-component',
  templateUrl: './recruitment-list.component.html',
  styleUrls: ['./recruitment-list.component.css']
})
export class RecruitmentListComponent implements OnInit {
  enums: RecruitmentEnums = new RecruitmentEnums();
  sortOrders = Object.values(SortOrder);
  sortOrder: string = SortOrder.NameAsc;
  offers: RecruitmentList;
  filters: Filters;

  @Input()
  apiUrl: string;

  @Input()
  redirectUrl: string;

  options: Options = {
    floor: 0,
    ceil: 100,
    step: 5,
    showTicks: false
  };

  // @ViewChild('address-input') qElementRef: ElementRef;
  // private places: any;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filters = FiltersDefault();
  }

  ngOnInit() {
    this.clearFilters();
    this.loadData();
    // let placesAutocomplete = places({
    //   appId: 'WIIZS8MMO7',
    //   apiKey: '72678c53476a1c91292a5c2ca7ee2139',
    //   container: this.qElementRef.nativeElement
    // });
    // this.places = places({
    //   appId: 'WIIZS8MMO7',
    //   apiKey: '72678c53476a1c91292a5c2ca7ee2139',
    //   container: this.qElementRef.nativeElement,
    //
    // });
    //
    // if (this.q) {
    //   this.places.setVal(this.q);
    // }
    //
    // this.places.on('change', function resultSelected(e) {
    //   //...
    // });
  }

  applyFilter(pageEvent: PageEvent) {
    this.filters.pageSize = pageEvent.pageSize;
    this.filters.pageIndex = pageEvent.pageIndex;
  }

  loadData() {
    this.filters.sortOrder = getEnumKeyByEnumValue(SortOrder, this.sortOrder);

    this.http.post<RecruitmentList>(this.baseUrl + this.apiUrl, this.filters).subscribe(result => {
      this.offers = result;
    }, error => console.error(error));
  }

  clearFilters() {
    this.filters = FiltersDefault();
  }

  createImgPath = (path: string) => {
    return `${this.baseUrl}${path}`;
  }
}
