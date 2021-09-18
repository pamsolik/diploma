import {Component} from '@angular/core';
import {newRecruitmentDetails, RecruitmentDetails} from "../../models/RecruitmentDetails"

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  editMode: boolean = false;
  settings: RecruitmentDetails = newRecruitmentDetails();

}
