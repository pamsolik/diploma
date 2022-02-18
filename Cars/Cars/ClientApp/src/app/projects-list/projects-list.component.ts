import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ProjectsFilters, FiltersDefault} from "../../models/ProjectsFilters";
import {PageEvent} from "@angular/material/paginator";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {Observable} from "rxjs";
import { ProjectList } from 'src/models/ProjectList';
import { formatNumber } from 'src/util/Formatter';
import { CustomSort } from 'src/util/CustomSort';
import { Technology } from 'src/models/enums/Technology';
import { ApiAnswer } from 'src/models/ApiAnswer';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-projects-list-component',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.css']
})
export class ProjectsListComponent implements OnInit {

  sort: CustomSort = new CustomSort();
  projects: ProjectList;
  filters: ProjectsFilters;
  technologies: string[] = Object.values(Technology);
  technology: string = "";
  

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filters = FiltersDefault();
  }

  ngOnInit() {
    this.clearFilters();
    this.loadData();
  }

  applyFilter(pageEvent: PageEvent) {
    this.filters.pageSize = pageEvent.pageSize;
    this.filters.pageIndex = pageEvent.pageIndex;
  }
  applyTech(){
    this.filters.technology = getEnumKeyByEnumValue(Technology, this.technology);
  }

  loadData() {
    this.applyTech();
    if (this.filters.technology == -1) this.filters.technology = null;
    console.log(this.filters);
    this.http.post<ProjectList>(this.baseUrl + 'api/projects', this.filters).subscribe(result => {
      this.projects = result;
      this.baseSort();
    }, error => console.error(error));
  }

  loadCsv() {
    this.applyTech();
    this.http.post<ApiAnswer>(this.baseUrl + 'api/projects/csv', this.filters).subscribe(result => {
      const blob = new Blob([result.message], {type: "text/plain;charset=utf-8"});
      saveAs(blob, "projects.csv");
    }, error => console.error(error));
  }

  clearFilters() {
    this.technology = "";
    this.filters = FiltersDefault();
  }

  baseSort() {
    if (!this.projects) return;
    this.projects.items.sort(this.sort.startSort('id', 'asc', null));
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
  
  createImgPath = (path: string) => {
    return `${this.baseUrl}${path}`;
  }

  getValue(value: number): string {
    return value === null || value === undefined ? "N/A" : formatNumber(value).toString();
  }
}
