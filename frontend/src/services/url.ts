import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { debounceTime, map, Observable, retry } from 'rxjs';
import { Constants } from './constants';
import { FullChart, ICoin, ICoinExchange } from './types';

/**
 * Сервис для работы с API криптовалют
 * Предоставляет методы для CRUD операций с монетами и их курсами
 */
@Injectable({
  providedIn: 'root'
})
export class URLService {
  private http = inject(HttpClient);

  /**
   * Добавление новой монеты с историей курсов
   * @param name - название монеты
   * @param ticks - временная метка начала отслеживания
   * @returns Observable с результатом операции
   */
  addCoin(name: string, ticks: number) : Observable<any> {
    const formData: FormData = new FormData();
    formData.append('ticks', ticks.toString());
    return this.http.post(Constants.apiURL + 'Coin/add-coin-&-coinExchanges/' + name, formData);
  }

  /**
   * Получение списка всех монет с предыдущей информацией
   * @returns Observable с массивом монет
   */
  getAllCoins() : Observable<ICoin[]> {
    return this.http.get<ICoin[]>(Constants.apiURL + 'Coin/get-coins-all-previous-information');
  }

  /**
   * Получение курсов монеты за определенный период
   * @param formData - объект с id монеты и шагом времени
   * @returns Observable с массивом курсов
   */
  getCoinsById(formData: {id: number, step: number}) : Observable<ICoinExchange[]> {
    return this.http.post<ICoinExchange[]>(Constants.apiURL + 'Coin/get-coinExchanges', formData);
  }

  /**
   * Получение полной информации о монете по ID
   * @param id - идентификатор монеты
   * @returns Observable с полной информацией о монете
   */
  getCoinById(id: number) : Observable<FullChart> {
    return this.http.get<FullChart>(Constants.apiURL + 'Coin/get-coin-full-information-by-coin-id/' + id)
    .pipe(retry(2));
  }

  /**
   * Удаление монеты и всех связанных курсов
   * @param id - идентификатор монеты
   * @returns Observable с результатом операции
   */
  deleteCoin(id: number) : Observable<any> {
    return this.http.delete(Constants.apiURL +"Coin/delete-coin-and-coinExchanges/" + id);
  }

  /**
   * Обновление данных монеты
   * @param id - идентификатор монеты
   * @returns Observable с результатом операции
   */
  updateCoin(id: number) : Observable<boolean> {
    return this.http.get<boolean>(Constants.apiURL +"Coin/update-coin-by-id-coin/" + id);
  }
}


