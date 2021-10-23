import {Component, Directive, EventEmitter, Inject, Input, Output, QueryList, ViewChildren} from '@angular/core';

import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";
import {RecruitmentApplication} from "../../models/RecruitmentApplication"
import {compare, NgbdSortableHeader, SortEvent} from "../../components/NgbdSortableHeader";

@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss'],
})
export class ApplicationsComponent {
  id: string = this.route.snapshot.paramMap.get('id');
  //recruitmentApplicationsBase: RecruitmentApplication[];
  recruitmentApplications: RecruitmentApplication[];

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService,
              private route: ActivatedRoute) {

    http.get<RecruitmentApplication[]>(baseUrl + 'api/recruitments/applications/' + this.id).subscribe(result => {
      this.recruitmentApplications = result;
      //this.recruitmentApplications = this.recruitmentApplicationsBase;

      if (this.recruitmentApplications){
        this.recruitmentApplications[0].codeQualityAssessment = {
          codeSmells: "B",
          completedTime: undefined,
          duplications: 5.2,
          errors: 2,
          id: 0,
          maintainability: "B",
          readability: "C",
          security: "A",
          success: false,
          technicalDebt: "3 dni 2godz"
        };

        this.recruitmentApplications[1].codeQualityAssessment = {
          codeSmells: "A",
          completedTime: undefined,
          duplications: 1.3,
          errors: 2400,
          id: 0,
          maintainability: "A",
          readability: "F",
          security: "D",
          success: false,
          technicalDebt: "69 dni"
        }
      }

    }, error => console.error(error));
  }

  selectCandidates() {

    //this.alertService.showLoading("Dodawanie aplikacji");
    console.log(this.recruitmentApplications);
    // this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments/apply`, this.application).subscribe(result => {
    //   this.alertService.showResultAndRedirect("Gratulacje", "Zapisano aplikację", '/recruitments')
    //   console.log(result);
    // }, error => {
    //   this.alertService.showResult("Błąd", error.message)
    //   console.error(error);
    // })
  }

  // @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  //
  // onSort({column, direction}: SortEvent) {
  //
  //   // resetting other headers
  //   this.headers.forEach(header => {
  //     if (header.sortable !== column) {
  //       header.direction = '';
  //     }
  //   });
  //
  //   // sorting countries
  //   if (direction === '' || column === '') {
  //     this.recruitmentApplications = this.recruitmentApplicationsBase;
  //   } else {
  //     this.recruitmentApplications = [...this.recruitmentApplicationsBase].sort((a, b) => {
  //       const res = compare(a.codeQualityAssessment[column], b.codeQualityAssessment[column]);
  //       return direction === 'asc' ? res : -res;
  //     });
  //   }
  // }

}


