import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { ErrorComponent } from './elements/error/error.component';
import { HeaderComponent } from './/elements/header/header.component';
import { HttpClientModule } from '@angular/common/http';
import { GlobalErrorComponent } from './elements/global-error/global-error.component';
import { FocusDirective } from '../directives/focus.directive';

@NgModule({
    declarations: [
    ],
    imports: [
        HttpClientModule, // need child module
        BrowserModule,
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