import { Component, OnInit } from '@angular/core';

/**
 * Компонент для отображения ошибок
 */
@Component({
  selector: 'app-error',
  standalone: true,
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent implements OnInit {

  ngOnInit(): void {
  }
}
