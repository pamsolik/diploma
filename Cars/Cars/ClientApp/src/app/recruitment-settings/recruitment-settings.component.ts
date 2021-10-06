import {Component, Inject} from '@angular/core';
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto"
import {HttpClient} from "@angular/common/http";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {ApiAnswer} from "../../models/ApiAnswer";
import {AlertService} from "../../services/alert.service";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();
  settings: RecruitmentDetailsDto = new RecruitmentDetailsDto();
  editMode: boolean = false;
  clauseOpt1: boolean = false;
  clauseOpt2: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private alertService: AlertService) {
    console.log(this.settings);
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
    this.settings.city.name = "Ass";
    this.alertService.showLoading("Dodawanie");
    this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments`, this.settings).subscribe(result => {
        this.alertService.showResultAndRedirect("Gratulacje", "Dodano rekrutacje", '/recruiter')
        console.log(result);
    }, error => {
        this.alertService.showResult("Błąd", error.message)
        console.error(error);
    })
  }
}
