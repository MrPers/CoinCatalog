import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { UpperCasePipe } from '@angular/common';

/**
 * Компонент для подтверждения удаления криптовалюты
 * Отображает диалог подтверждения перед удалением
 */
@Component({
  selector: 'app-delete-coin',
  standalone: true,
  imports: [UpperCasePipe],
  templateUrl: './delete-coin.component.html',
  styleUrls: ['./delete-coin.component.css']
})
export class DeleteCoinComponent {
  @Input() name!: string;
  @Output() close = new EventEmitter<void>();
  @Output() send = false;

  /**
   * Подтверждение удаления монеты
   */
  submit(){
    this.send = true;
    this.close.emit();
  }
}