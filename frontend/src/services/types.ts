import { Directive, Injectable, ViewContainerRef } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';

@Injectable({
    providedIn: 'root'
})

@Directive({
    selector: "[dynamic-ref]",
    standalone: true
})
export class DynamicDirective {
    constructor(public viewContainerRef: ViewContainerRef) {}
}

export class FullChart{
    id: number;
    name: string;
    urlIcon:string;
    genesisDate: string;
    description: string;
}

interface IObjectKeys {
    [key: string]: string | number;
}

export interface ICoin extends IObjectKeys{
    name:string;
    urlIcon:string;
    id: number;
    prices:number;
    volumeTraded:number;
}

export interface ICoinExchange{
    time:number;
    prices:number;
    volumeTraded:number;
}

export interface ITimeStep{
    name: string;
    time: number;
}

export interface IContactCharts{
    id: number;
    label: string;
    backgroundColor:string;
    above: string;
    volume: number[];
}