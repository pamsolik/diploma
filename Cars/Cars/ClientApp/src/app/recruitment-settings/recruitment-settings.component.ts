import {Component, Inject} from '@angular/core';
import {RecruitmentDetailsDto} from "../../models/RecruitmentDetailsDto"
import {JobLevel} from "../../models/enums/JobLevel";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";
import {HttpClient} from "@angular/common/http";
import {RecruitmentEnums} from "../../models/enums/RecruitmentEnums";
import {RecruitmentList} from "../../models/RecruitmentOffer";
import {ApiAnswer} from "../../models/ApiAnswer";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  enums: RecruitmentEnums = new RecruitmentEnums();
  editMode: boolean = false;
  settings: RecruitmentDetailsDto = new RecruitmentDetailsDto();

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
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
    this.http.post<ApiAnswer>(`${this.baseUrl}api/recruitments`, this.settings).subscribe(result => {
      console.log(result);
    }, error => console.error(error));
  }

}
