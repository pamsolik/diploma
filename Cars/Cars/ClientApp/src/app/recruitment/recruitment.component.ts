import { Component } from '@angular/core';

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent {
  offers: string[] = ["A", "B", "C", "D"];

  public incrementCounter() {
    //this.currentCount++;
  }
}
