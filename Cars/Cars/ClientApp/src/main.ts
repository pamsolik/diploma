import {enableProdMode} from '@angular/core';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';

import {AppModule} from './app/app.module';
import {environment} from './environments/environment';

export function getBaseUrl() {
  let x = document.getElementsByTagName('base')[0].href
  if (x.includes('azurewebsites')) return x;
  return "https://localhost:7187/";
}

const providers = [
  {provide: 'BASE_URL', useFactory: getBaseUrl, deps: []}
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
