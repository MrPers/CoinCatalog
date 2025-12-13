import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, signal, computed, effect, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart } from 'chart.js/auto';
import { dataCharts, timeStep } from '../../../../../services/constants';
import { ErrorService } from '../../../../../services/error';
import { FullChart, ICoinExchange, ITimeStep } from '../../../../../services/types';
import { URLService } from '../../../../../services/url';
import { CommonModule, UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

/**
 * Компонент для отображения графиков криптовалют
 * Использует Chart.js для визуализации данных о ценах и объемах торгов
 */
@Component({
  selector: 'app-chart',
  standalone: true,
  imports: [CommonModule, FormsModule, UpperCasePipe],
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartCoinComponent implements OnInit {
  // Инъекция зависимостей через inject()
  private route = inject(ActivatedRoute);
  private url = inject(URLService);
  private errorService = inject(ErrorService);

  // Сигналы для реактивного состояния
  private id = signal<number>(0);
  labelTime = signal<string[]>([]);
  contactMethods = signal(dataCharts);
  constatadataChart = signal<number>(0);
  fullChart = signal<FullChart>(new FullChart());
  myChart = signal<any>(null);
  loading = signal<boolean>(false);
  timesStep = signal<ITimeStep[]>([]);
  timeStep = signal<number>(0);

  /**
   * Инициализация компонента
   * Загружает данные о монете и её курсах
   */
  ngOnInit()
  {
    this.id.set(this.route.snapshot.params['id']);
    this.timesStep.set(timeStep);
    this.timeStep.set(this.timesStep()[0].time);
    this.loading.set(true);
    this.getCoinById();
    this.getCoinsById();
  }

  /**
   * Выбор временного интервала для отображения данных
   * @param id - идентификатор временного интервала
   */
  chooseStep(id: number) {
    this.timeStep.set(id);
    this.myChart()?.destroy();
    this.loading.set(true);
    this.getCoinsById();
  }

  /**
   * Изменение типа отображаемых данных (цена или объем)
   * @param id - идентификатор типа данных
   */
  onChange(id: number) {
    this.constatadataChart.set(id);
    this.myChart()?.destroy();
    this.drawChart(id);
  }

  /**
   * Отрисовка графика с использованием Chart.js
   * @param id - идентификатор типа данных для отображения
   */
  drawChart(id: number) {
    const chart = new Chart("myChart", {
      type: 'line',
      data: {
          labels: this.labelTime(),
          datasets: [{
            fill: {
              target: 'origin',
              above: this.contactMethods()[id].above,
            },
            label: this.contactMethods()[id].label,
            data: this.contactMethods()[id].volume,
            backgroundColor: this.contactMethods()[id].backgroundColor,
        }]
      },
      options: {
        plugins: {
          legend: {
            display: false
          }
        },
        scales: {
          y: {
            title: {
              display: true,
              text: 'Price',
              font: {
                family:'Helvetica Neue',
                size: 17
              }
            }
          },
          x: {
            title: {
              display: true,
              text: 'Period',
              font: {
                family:'Helvetica Neue',
                size: 17
              }
            }
          }
        }
      }
   });
   this.myChart.set(chart);
  }

  /**
   * Получение информации о монете по ID
   */
  getCoinById() {
    this.url.getCoinById(this.id())
    .subscribe({
      next: (result: FullChart) => this.fullChart.set(result),
      error: (e:HttpErrorResponse) => this.errorService.handle(e.error)
    });
  }

  /**
   * Получение данных о курсах монеты за выбранный период
   */
  getCoinsById() {
    this.url.getCoinsById({id: this.id(), step: this.timeStep()})
    .subscribe({
      next: (result: ICoinExchange[]) =>
      {
        const methods = this.contactMethods();
        methods[0].volume = [];
        methods[1].volume = [];
        const labels: string[] = [];

        for (let index = 0; index < result.length; index++) {
          methods[0].volume.push(result[index].prices);
          methods[1].volume.push(result[index].volumeTraded);
          labels.push(new Date(result[index].time).toLocaleString());
        }

        this.contactMethods.set(methods);
        this.labelTime.set(labels);
        this.loading.set(false);
        this.drawChart(this.constatadataChart());
      },
      error: (e:HttpErrorResponse) => this.errorService.handle(e.error)
    });
  }
}