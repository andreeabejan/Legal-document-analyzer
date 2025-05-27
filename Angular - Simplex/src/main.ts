
import { bootstrapApplication } from '@angular/platform-browser';
import { importProvidersFrom }    from '@angular/core';
import { HttpClientModule }       from '@angular/common/http';
import { FormsModule }            from '@angular/forms';
import { AppComponent }           from './app/app.component';

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(HttpClientModule, FormsModule)
  ]
})
.catch(err => console.error(err));
