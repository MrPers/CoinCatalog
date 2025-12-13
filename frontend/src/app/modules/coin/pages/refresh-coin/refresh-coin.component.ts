import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { UpperCasePipe } from '@angular/common';

/**
 * Компонент для обновления данных криптовалюты
 * Отображает диалог подтверждения перед обновлением данных
 */
@Component({
  selector: 'app-refresh-coin',
  standalone: true,
  imports: [UpperCasePipe],
  templateUrl: './refresh-coin.component.html',
  styleUrls: ['./refresh-coin.component.css']
})
export class RefreshCoinComponent {
  @Input() name!: string;
  @Output() close = new EventEmitter<void>();
  @Output() send = false;

  /**
   * Подтверждение обновления данных монеты
   */
  submit(){
    this.send = true;
    this.close.emit();
  }
}
