import {Component, Inject, Input, OnInit} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {PageEvent} from "@angular/material/paginator";
import {SortOrder} from "../../models/enums/SortOrder";

import {Options} from "@angular-slider/ngx-slider";
import {UserFilters, UserFiltersDefault, UserList} from "../../models/UserList";

@Component({
  selector: 'app-user-list-component',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  sortOrders = Object.values(SortOrder);
  sortOrder: string = SortOrder.NameAsc;

  users: UserList;
  filters: UserFilters;


  @Input()
  apiUrl: string;

  options: Options = {
    floor: 0,
    ceil: 100,
    step: 5,
    showTicks: false
  };


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filters = UserFiltersDefault();
  }

  ngOnInit() {
    this.clearFilters();
    this.loadData();
  }

  applyFilter(pageEvent: PageEvent) {
    this.filters.pageSize = pageEvent.pageSize;
    this.filters.pageIndex = pageEvent.pageIndex;
  }

  loadData() {
    let params = new HttpParams();
    params = params.append('searchTerm', this.filters.searchTerm);
    params = params.append('pageIndex', String(this.filters.pageIndex));
    params = params.append('pageSize', String(this.filters.pageSize));

    this.http.get<UserList>(this.baseUrl + this.apiUrl, {params: params}).subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }

  clearFilters() {
    this.filters = UserFiltersDefault();
  }

  createImgPath = (path: string) => {
    return `${this.baseUrl}${path}`;
  }
}
