import {Component, Inject} from '@angular/core';
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto"
import {HttpClient} from "@angular/common/http";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {ApiAnswer} from "../../models/ApiAnswer";
import {AlertService} from "../../services/alert.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html'
})
export class RecruitmentSettingsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();

  clauseOpt1: boolean = false;
  clauseOpt2: boolean = false;
  settings: RecruitmentDetailsDto;

  editMode: boolean = false;

  id: string = this.route.snapshot.paramMap.get('id');

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private alertService: AlertService, private route: ActivatedRoute) {
    if (this.id) {
      this.editMode = true;

      http.get<RecruitmentDetailsDto>(baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
        this.settings = result;
        this.updateEnums();
      }, error => console.error(error));
    } else {
      this.settings = new RecruitmentDetailsDto();
    }
  }

  public createImgPath = (serverPath: string) => {
    if (serverPath)
      return `${this.baseUrl}${serverPath}`;
    return `${this.baseUrl}Resources/Images/Defaults/placeholder.png`;
  }

  public uploadFinished = (event) => {
    this.settings.imgUrl = event.dbPath;
  }

  save() {
    this.enums.updateRecruitmentSettings(this.settings);
    console.log(this.settings);
    this.settings.city.name = "Warszawa"; //TODO: add places autocomplete
    if (this.editMode) this.saveChanges();
    else this.saveNew();
  }

  private updateEnums() {
    this.enums.jobLevel = this.enums.jobLevels[this.settings.jobLevel];
    this.enums.jobType = this.enums.jobTypes[this.settings.jobType];
    this.enums.recruitmentType = this.enums.recruitmentTypes[this.settings.type];
    this.enums.teamSize = this.enums.teamSizes[this.settings.teamSize];

    this.clauseOpt1 = this.settings.clauseOpt1.length > 0;
    this.clauseOpt2 = this.settings.clauseOpt2.length > 0;
  }

  private saveNew() {
    this.alertService.showLoading("Dodawanie");
    this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments`, this.settings).subscribe(result => {
      this.alertService.showResultAndRedirect("Gratulacje", "Dodano rekrutacje", '/recruiter')
      console.log(result);
    }, error => {
      this.alertService.showError(error);
    });
  }

  private saveChanges() {
    this.alertService.showLoading("Zapisywanie zmian");
    this.http.put<ApiAnswer>(`${this.baseUrl}api/recruitments/${this.id}`, this.settings).subscribe(result => {
      this.alertService.showResultAndRedirect("Gratulacje", "Zaktualizowano ofertę", '/recruiter')
      console.log(result);
    }, error => {
      this.alertService.showError(error);
    });
  }
}
