import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { ErrorComponent } from './elements/error/error.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HeaderComponent } from './/elements/header/header.component';
import { HttpClientModule } from '@angular/common/http';
import { DynamicDirective} from '../services/types';
import { GlobalErrorComponent } from './elements/global-error/global-error.component';
import { FocusDirective } from '../directives/focus.directive';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [
    ],
    imports: [
        HttpClientModule, // need child module
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutes,
        AppComponent,
        ErrorComponent,
        HeaderComponent,
        GlobalErrorComponent,
        FocusDirective
    ],
    providers: [
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }