import { IContactCharts, ITimeStep } from "./types";

export class Constants {
    public static apiURL = "http://localhost:1001/";
}

export const timeStep: ITimeStep[] = [
    { time: 24, name: "1DAY" },
    { time: 72, name: "3DAY" },
    { time: 120, name: "5DAY" },
    { time: 168, name: "7DAY" },
];

export const dataCharts: IContactCharts[] = [
    { id: 0, label: "Prices", backgroundColor:'rgba(255, 165, 0, 0.7)', above: 'rgba(255, 165, 0, 0.5)', volume: []},
    { id: 1, label: "Volume Traded", backgroundColor:'rgba(120, 120, 120, 0.7)', above: 'rgba(255, 99, 71, 0.5)', volume: []},
  ];
