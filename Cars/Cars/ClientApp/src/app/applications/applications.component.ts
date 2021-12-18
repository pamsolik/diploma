import {Component, Inject, OnInit} from '@angular/core';

import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from "../../services/alert.service";
import {RecruitmentApplication} from "../../models/RecruitmentApplication"
import {ApiAnswer} from "../../models/ApiAnswer";
import {CloseRecruitmentDto, RecruitmentToClose} from "../../models/CloseRecruitmentDto";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";
import {CustomSort} from 'src/util/CustomSort';
import {AuthorizeService} from "../../api-authorization/authorize.service";
import {formatNumber} from "../../util/Formatter";


@Component({
  selector: 'app-applications',
  templateUrl: './applications.component.html',
  styleUrls: ['./applications.component.scss'],
})
export class ApplicationsComponent implements OnInit {
  id: string = this.route.snapshot.paramMap.get('id');
  recruitmentApplications: RecruitmentApplication[];
  recruitment: RecruitmentDetailsView;
  sort: CustomSort = new CustomSort();

  constructor(private modalService: NgbModal,
              @Inject('BASE_URL') private baseUrl: string,
              private http: HttpClient,
              private alertService: AlertService,
              private route: ActivatedRoute,
              public authorizeService: AuthorizeService
  ) {
  }

  ngOnInit() {
    this.http.get<RecruitmentApplication[]>(this.baseUrl + 'api/recruitments/applications/' + this.id).subscribe(result => {
      this.recruitmentApplications = result;
      this.baseSort();
    }, error => console.error(error));

    this.http.get<RecruitmentDetailsView>(this.baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
      this.recruitment = result;
      this.baseSort();
    }, error => console.error(error));
  }

  baseSort() {
    if (!this.recruitmentApplications || !this.recruitment) return;
    this.recruitmentApplications.sort(this.sort.startSort('codeOverallQuality.overallRating', 'asc', null));
  }

  getValue(value: number): string {
    return value === null || value === undefined ? "N/A" : formatNumber(value).toString();
  }

  createDto(): CloseRecruitmentDto {
    return {
      recruitmentsToClose: this.recruitmentApplications
        .map(r => <RecruitmentToClose>{applicationId: r.id, selected: r.selected}),
      recruitmentId: Number(this.id)
    }
  }

  selectCandidates() {
    this.alertService.showYesNo("Czy napewno chcesz zakończyć tą rekrutacje wybierając zaznaczonych kandydatów?").then(result => {
      if (result.isConfirmed) {
        this.alertService.showLoading("Zamykanie rekrutacji");
        this.http.put<ApiAnswer>(`${this.baseUrl}api/recruitments/close`, this.createDto()).subscribe(result => {
          this.alertService.showResultAndRedirect("Gratulacje", "Zamknięto rekrutację", '/recruiter')
          console.log(result);
        }, error => {
          this.alertService.showError(error);
        })
      }
    });
  }
}


