import { Injectable, signal } from '@angular/core';
import { Subject } from 'rxjs';

/**
 * Сервис для управления ошибками приложения
 * Предоставляет централизованный механизм обработки и отображения ошибок
 */
@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  /** Observable поток для ошибок */
  error$ = new Subject<string>();

  /**
   * Обработка и отображение сообщения об ошибке
   * @param message - текст сообщения об ошибке
   */
  handle(message: string){
    this.error$.next(message);
  }

  /**
   * Очистка текущего сообщения об ошибке
   */
  clear(){
    this.error$.next('');
  }
}
