import {Component, Inject} from '@angular/core';
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto"
import {HttpClient} from "@angular/common/http";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {ApiAnswer} from "../../models/ApiAnswer";
import {AlertService} from "../../services/alert.service";
import {ActivatedRoute} from "@angular/router";
import {RecruitmentDetailsView} from "../../models/RecruitmentDetailsView";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();
  settings: RecruitmentDetailsDto;
  editMode: boolean = false;
  clauseOpt1: boolean = false;
  clauseOpt2: boolean = false;

  id: string = this.route.snapshot.paramMap.get('id');

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private alertService: AlertService, private route: ActivatedRoute) {
    if (this.id){
      this.editMode = true;

      http.get<RecruitmentDetailsDto>(baseUrl + 'api/recruitments/' + this.id).subscribe(result => {
        this.settings = result;
        console.log(this.settings);
      }, error => console.error(error));

    }
    else {
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
    this.settings.city.name = "Ass"; //TODO: change
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
