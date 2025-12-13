import { NgModule } from '@angular/core';
import { TableCoinComponent } from './pages/table-coin/table.component';
import { ChartCoinComponent } from './pages/chart-coin/chart.component';
import { RefreshCoinComponent } from './pages/refresh-coin/refresh-coin.component';
import { AddCoinComponent } from './pages/add-coin/add-coin.component';
import { DeleteCoinComponent } from './pages/delete-coin/delete-coin.component';
import { CoinComponent } from './coin.component';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CoinRouttes } from './coin.routing';
import { CommonModule } from '@angular/common';
import { DynamicDirective } from '../../../services/types';

@NgModule({
declarations: [
],
imports: [
  CommonModule, // using these in-built directives like "ngFor, ngIf"
  HttpClientModule,
  FormsModule,
  ReactiveFormsModule,
  CoinRouttes,
  CoinComponent,
  TableCoinComponent,
  ChartCoinComponent,
  RefreshCoinComponent,
  AddCoinComponent,
  DeleteCoinComponent,
  DynamicDirective
],
providers: [
],
bootstrap: [CoinComponent]
})

export class CoinModule { }
