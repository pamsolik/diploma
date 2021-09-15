import {Component, Inject, ViewEncapsulation} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-recruiter',
  templateUrl: './recruiter.component.html',
  styleUrls: ['./recruiter.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class RecruiterComponent {
  offers: string[] = ["1", "2", "3", "4"];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    // http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
    //   this.forecasts = result;
    // }, error => console.error(error));
  }
}


