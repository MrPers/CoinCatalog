import { Component, inject } from '@angular/core';
import { ErrorService } from '../../../services/error';
import { CommonModule } from '@angular/common';

/**
 * Компонент для отображения глобальных ошибок приложения
 */
@Component({
  selector: 'app-global-error',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './global-error.component.html',
  styleUrls: ['./global-error.component.css']
})
export class GlobalErrorComponent  {
  // Инъекция сервиса ошибок
  public errorService = inject(ErrorService);
}
