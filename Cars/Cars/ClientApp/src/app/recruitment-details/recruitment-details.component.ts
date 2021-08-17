import { Component } from '@angular/core';

@Component({
  selector: 'app-recruitment-details-component',
  templateUrl: './recruitment-details.component.html',
  styleUrls: ['./recruitment-details.component.css']
})
export class RecruitmentDetailsComponent {
  offers: string[] = ["A", "B", "C", "D"];

  public incrementCounter() {
    //this.currentCount++;
  }
}
