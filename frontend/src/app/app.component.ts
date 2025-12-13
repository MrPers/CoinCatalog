import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './elements/header/header.component';
import { GlobalErrorComponent } from './elements/global-error/global-error.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, GlobalErrorComponent],
  template:'<app-global-error></app-global-error> <body class="text-gray-700"><div class="container mx-auto px-4"><app-header></app-header><router-outlet></router-outlet></div></body>',
})
export class AppComponent {

}
