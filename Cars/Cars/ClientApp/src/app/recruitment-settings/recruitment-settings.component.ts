import {Component} from '@angular/core';
import {newRecruitmentDetails, RecruitmentDetails} from "../../models/RecruitmentDetails"
import {JobLevel} from "../../models/enums/JobLevel";
import {getEnumKeyByEnumValue} from "../../components/EnumTool";

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  jobLevels = Object.values(JobLevel);
  jobLevel: string;
  editMode: boolean = false;
  settings: RecruitmentDetails = newRecruitmentDetails();

  save(){
    this.settings.jobLevel = getEnumKeyByEnumValue(JobLevel, this.jobLevel);

    console.log(this.settings);
  }

}
