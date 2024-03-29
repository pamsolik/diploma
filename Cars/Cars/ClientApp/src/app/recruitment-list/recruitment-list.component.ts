import {Component, Inject, Input, OnInit, ViewChild} from '@angular/core';
import {HttpClient} from "@angular/common/http";

import {Filters, FiltersDefault} from "../../models/Filters";
import {PageEvent} from "@angular/material/paginator";
import {SortOrder} from "../../models/enums/SortOrder";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {Options} from "@angular-slider/ngx-slider";
import {RecruitmentList} from "../../models/RecruitmentList";
import {GooglePlaceDirective} from "ngx-google-places-autocomplete";
import {Observable} from "rxjs";

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
  selectedCity: any;
  lat: number;
  lon: number;


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

  @ViewChild("ngx-places") placesRef: GooglePlaceDirective;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filters = FiltersDefault();
  }

  ngOnInit() {
    this.clearFilters();
    this.loadData();

    this.getPosition().subscribe(pos => {
      console.log(pos);
      this.lat = pos.coords.latitude;
      this.lon = pos.coords.longitude;
      this.filters.city.longitude = this.lon;
      this.filters.city.latitude = this.lat;
    });
  }

  public placeSelect(place: any){
    this.selectedCity = place;
    console.log(place);
    if (place){
      this.filters.city.name = place.properties.address_line1;
      this.filters.city.latitude = place.properties.lat;
      this.filters.city.longitude = place.properties.lon;
    }
    else {
      this.filters.city.name = "";
      this.filters.city.latitude = this.lat;
      this.filters.city.longitude = this.lon;
    }
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

  getPosition(): Observable<any> {
    return new Observable(observer => {
      window.navigator.geolocation.getCurrentPosition(position => {
          observer.next(position);
          observer.complete();
        },
        error => observer.error(error));
    });
  }
}
