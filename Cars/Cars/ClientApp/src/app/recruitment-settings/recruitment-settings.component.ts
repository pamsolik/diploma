import { Component } from '@angular/core';

@Component({
  selector: 'app-recruitment-settings-component',
  templateUrl: './recruitment-settings.component.html',
  styleUrls: ['./recruitment-settings.component.css']
})
export class RecruitmentSettingsComponent {
  offers: string[] = ["A", "B", "C", "D"];

  public incrementCounter() {
    //this.currentCount++;
  }
}
