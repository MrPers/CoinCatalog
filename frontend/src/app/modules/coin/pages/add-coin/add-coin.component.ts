import { Component, EventEmitter, Input, OnInit, Output, signal, computed } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FocusDirective } from '../../../../../directives/focus.directive';

/**
 * Компонент для добавления новой криптовалюты
 * Предоставляет форму для ввода названия монеты и даты
 */
@Component({
  selector: 'app-add-coin',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, FocusDirective],
  templateUrl: './add-coin.component.html',
  styleUrls: ['./add-coin.component.css']
})
export class AddCoinComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  @Output() dateTime = "2020-01-01";
  @Output() coinName = "";

  // Сигналы для реактивного состояния
  myForm = signal<FormGroup>(new FormGroup({}));
  private maxData = new Date().toLocaleString().split(',')[0].split('/');
  public maxDataString = signal<string>(this.maxData[2] + "-" + this.maxData[1] + "-" + this.maxData[0]);

  /**
   * Инициализация формы с валидацией
   */
  ngOnInit(): void {
    this.myForm.set(new FormGroup({
      "coinName": new FormControl("", [
        Validators.required,
        Validators.maxLength(10)])
    }));
  }

  /**
   * Отправка формы добавления монеты
   */
  submit(){
    this.coinName = this.myForm().controls['coinName'].value;
    this.close.emit();
  }
}
