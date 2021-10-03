import {Component, Inject} from '@angular/core';
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto"
import {JobLevel} from "../../models/enums/JobLevel";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {HttpClient} from "@angular/common/http";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {RecruitmentList} from "../../models/RecruitmentOffer";
import {ApiAnswer} from "../../models/ApiAnswer";
import Swal from 'sweetalert2';
import {AlertService} from "../../services/alert.service";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();
  editMode: boolean = false;
  settings: RecruitmentDetailsDto = new RecruitmentDetailsDto();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private alertService: AlertService) {
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
    this.settings.city = "Ass";
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
