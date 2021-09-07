import {Component} from '@angular/core';

@Component({
  selector: 'app-recruitment-component',
  templateUrl: './recruitment.component.html',
  styleUrls: ['./recruitment.component.css']
})
export class RecruitmentComponent {
  offers: string[] = ["1", "2", "3", "4"];

  public incrementCounter() {
    //this.currentCount++;
  }
}
