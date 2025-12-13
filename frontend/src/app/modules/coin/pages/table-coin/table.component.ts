import { Component, EventEmitter, Input, OnInit, Output, ViewChild, ViewContainerRef, signal, computed, inject } from '@angular/core';
import { RefreshCoinComponent } from '../refresh-coin/refresh-coin.component';
import { AddCoinComponent } from '../add-coin/add-coin.component';
import { catchError, mergeMap, of, tap, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DeleteCoinComponent } from '../delete-coin/delete-coin.component';
import { DynamicDirective, ICoin } from '../../../../../services/types';
import { ErrorService } from '../../../../../services/error';
import { URLService } from '../../../../../services/url';
import { CommonModule, DecimalPipe, UpperCasePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

/**
 * Компонент таблицы криптовалют
 * Отображает список монет с возможностью добавления, обновления и удаления
 */
@Component({
  selector: 'app-table',
  standalone: true,
  imports: [CommonModule, RouterLink, DecimalPipe, UpperCasePipe, DynamicDirective],
  templateUrl: './table.component.html',
})
export class TableCoinComponent implements OnInit {
  @ViewChild(DynamicDirective) dynamic!: DynamicDirective;

  // Инъекция зависимостей
  private urlService = inject(URLService);
  private errorService = inject(ErrorService);

  // Сигналы для реактивного состояния
  private resultCoins = signal<ICoin[]>([]);
  private increaseDecrease = signal<number>(1);
  private valueCriteria = signal<string>("");
  private valuesOnKey = signal<string>("");
  public coins = signal<ICoin[]>([]);

  /**
   * Инициализация компонента
   * Загружает список всех монет
   */
  ngOnInit() {
    this.outputCoins();
  }

  /**
   * Открытие модального окна для добавления новой монеты
   */
  cleekAdd(): void {
    const component = this.dynamic.viewContainerRef.createComponent(AddCoinComponent);
    component.instance.close.subscribe(async ()=>{
      this.dynamic.viewContainerRef.clear();
      if(component.instance.coinName){
        await this.addCoins(component.instance.coinName, new Date(component.instance.dateTime).getTime());
      }
    });
  }

  /**
   * Открытие модального окна для обновления данных монеты
   * @param coin - монета для обновления
   */
  cleekRefresh(coin: ICoin): void {
    const component = this.dynamic.viewContainerRef.createComponent(RefreshCoinComponent);
    component.instance.name = coin.name;
    component.instance.close.subscribe(()=>{
    this.dynamic.viewContainerRef.clear();
      if(component.instance.send){
        this.refreshCoins(coin.id);
      }
    });
  }

  /**
   * Открытие модального окна для удаления монеты
   * @param coin - монета для удаления
   */
  cleekDelete(coin: ICoin): void {
    const component = this.dynamic.viewContainerRef.createComponent(DeleteCoinComponent);
    component.instance.name = coin.name;
    component.instance.close.subscribe(()=>{
    this.dynamic.viewContainerRef.clear();
      if(component.instance.send){
        this.deleteCoins(coin.id);
      }
    });
  }

  /**
   * Фильтрация монет по введенному тексту
   * @param event - событие ввода текста
   */
  onKey(event: any) {
    const searchValue = event.target.value.toLowerCase();
    this.valuesOnKey.set(searchValue);

    const filtered: ICoin[] = [];
    this.resultCoins().forEach(coin => {
      for (var code in coin) {
        if(code != "id" && coin[code].toString().toLowerCase().indexOf(searchValue) > -1 && code != "urlIcon"){
          filtered.push(coin);
          break;
        }
      }
    });
    this.coins.set(filtered);
  }

  /**
   * Сортировка монет по выбранному критерию
   * @param criteria - критерий сортировки (название поля)
   */
  sortBy(criteria: string): void {
    if(this.valueCriteria() == criteria){
      this.increaseDecrease.update(val => val * -1);
    }
    else{
      this.valueCriteria.set(criteria);
      this.increaseDecrease.set(1);
    }

    const sorted = [...this.coins()].sort((a, b) => {
      if (a[criteria] > b[criteria]) {
        return 1 * this.increaseDecrease();
      }
      if (a[criteria] < b[criteria]) {
        return -1 * this.increaseDecrease();
      }
      return 0;
    });
    this.coins.set(sorted);
  }

  /**
   * Получение списка всех монет
   */
  outputCoins(){
    return this.urlService.getAllCoins()
    .subscribe({
      next: (result : ICoin[]) =>
      {
        this.coins.set(result);
        this.resultCoins.set(result);
      },
      error: (e:HttpErrorResponse) => this.errorService.handle(e.message)
    });
  }

  /**
   * Добавление новой монеты
   * @param name - название монеты
   * @param ticks - временная метка
   */
  addCoins(name: string, ticks: number){
    this.urlService.addCoin(name, ticks)
    .pipe(
      mergeMap(() => this.urlService.getAllCoins()),
      catchError((error: any) => {
        this.errorService.handle(error.error);
        return throwError(() => new Error());
      })
    )
    .subscribe({
      next: (result : ICoin[]) =>
      {
        this.coins.set(result);
        this.resultCoins.set(result);
      },
      error: (e:HttpErrorResponse) => this.errorService.handle(e.error)
    });
  }

  /**
   * Удаление монеты
   * @param id - идентификатор монеты
   */
  deleteCoins(id: number){
    this.urlService.deleteCoin(id)
    .pipe(
      mergeMap(() => this.urlService.getAllCoins()),
      catchError((error: any) => {
        this.errorService.handle(error.error);
        return throwError(() => new Error());
      })
    )
    .subscribe({
      next: (result : ICoin[]) =>
      {
        this.coins.set(result);
        this.resultCoins.set(result);
      },
      error: (e:HttpErrorResponse) => this.errorService.handle(e.error)
    });
  }

  /**
   * Обновление данных монеты
   * @param id - идентификатор монеты
   */
  refreshCoins(id: number){
    this.urlService.updateCoin(id)
    .pipe(
      mergeMap(() => this.urlService.getAllCoins()),
      catchError((error: any) => {
        this.errorService.handle(error.error);
        return throwError(() => new Error());
      })
    )
    .subscribe({
      next: (result : ICoin[]) =>
      {
        this.coins.set(result);
        this.resultCoins.set(result);
      },
      error: (e:HttpErrorResponse) => this.errorService.handle(e.error)
    });
  }

}