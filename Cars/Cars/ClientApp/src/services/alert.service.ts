﻿import { Injectable, OnInit, Inject } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Injectable({
  providedIn: "root",
})
export class AlertService {

  constructor( private router: Router) {
//private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,
  }

  showLoading(message: string) {

    let timerInterval;
    Swal.fire({
      title: message,
      timer: 1000,
      timerProgressBar: true,
      didOpen: () => {
        Swal.showLoading()
        timerInterval = setInterval(() => { }, 100)
      },
      willClose: () => {
        clearInterval(timerInterval)
      }
    }).then((result) => {
      if (result.dismiss === Swal.DismissReason.timer) {
      }
    })
  }

  showResultAndRedirect(title: string, result: string, route: string, type: any = "info"){
    Swal.close();
    Swal.fire(title, result, type).then(
      () => { this.router.navigate([route])}
    );
  }

  showResult(title: string, result: string, type: any = "error"){
    Swal.close();
    Swal.fire(title, result, type).then(
      () => {}
    );
  }
}