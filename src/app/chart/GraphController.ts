import { Vector2, Vector3 } from "@babylonjs/core";
import Plotly from "plotly.js-dist";
import App from "../App";
import EnergyHelper from "../EnergyHelper";
import State from "../State";


interface PlotlyTrace {
  x: number[];
  y: number[];
  mode: string;
  name: string;
  line: { shape: string };
}

export default class GraphController {

  static lineData: PlotlyTrace[] = [];
  static currentSymbol1: string;
  static currentSymbol2: string;
  static graphUpperLimit: number;
  static graphLowerLimit: number;

  static redraw() {
    Plotly.newPlot('energyChart', this.lineData, this.getLayout(), { staticPlot: true });
    this.updateIndicator();
  }

  static getEmptyTrace(): PlotlyTrace {
    return this.getTrace([], "", "");
  }

  static getTrace(points: Vector2[], symbol1: string, symbol2: string): PlotlyTrace {
    const xMultiplier = State.params.useAngstroms ? 0.01 : 1;
    const xValues = points.map(point => point.x * xMultiplier);
    const yValues = points.map(point => point.y);

    return {
      x: xValues,
      y: yValues,
      mode: 'lines',
      name: `${symbol1} - ${symbol2}`,
      line: { shape: 'spline' },
    };
  }

  static getLayout(): any {
    this.graphLowerLimit = [...this.lineData[3].y].sort((a, b) => a - b)[0] * 1.2;
    this.graphUpperLimit = Math.abs([...this.lineData[3].y].sort((a, b) => a - b)[0]) * 2;

    // const axisFont = {
    //   family: 'Courier New, monospace',
    //   size: 18,
    //   color: '#7f7f7f'
    // };

    const xAxisLabel = State.params.useAngstroms ? "Å" : "pm";
    const xAxisLimit = State.params.useAngstroms ? 10 : 1000;

    return {
      xaxis: { range: [0, xAxisLimit], title: { text: 'Distance (' + xAxisLabel + ')' } },
      yaxis: { range: [this.graphLowerLimit, this.graphUpperLimit], title: { text: 'Energy Potential (kJ/mol)' } },
      paper_bgcolor: 'rgba(0,0,0,0)',
      plot_bgcolor: 'rgba(0,0,0,0)',
      showlegend: true,
      legend: {
        xanchor: 'right',
      },
      colorway: ['#f3cec9', '#e7a4b6', '#cd7eaf', '#a262a9'],
    };
  }

  static createGraph(data: Vector2[], symbol1: string, symbol2: string) {
    this.currentSymbol1 = symbol1;
    this.currentSymbol2 = symbol2;

    const initialTrace = this.getTrace(data, symbol1, symbol2);

    this.lineData = [this.getEmptyTrace(), this.getEmptyTrace(), this.getEmptyTrace(), initialTrace];

    Plotly.newPlot('energyChart', this.lineData, this.getLayout(), { staticPlot: true });

    this.updateIndicator();
  }

  static updateGraph(data: Vector2[], symbol1: string, symbol2: string) {
    this.showIndicator(true);
    this.currentSymbol1 = symbol1;
    this.currentSymbol2 = symbol2;

    this.lineData.push(this.getTrace(data, symbol1, symbol2));
    this.lineData = this.lineData.slice(-4);

    const layout_update = this.getLayout();

    Plotly.react('energyChart', {
      data: [this.lineData[0], this.lineData[1], { ...this.lineData[2] }, { ...this.lineData[2] }],
      layout: layout_update,
    });

    Plotly.animate('energyChart', {
      data: [this.lineData[3]],
      traces: [3],
    }, {
      transition: {
        duration: 500,
        easing: 'cubic-in-out'
      },
      frame: {
        duration: 500
      }
    });

    this.updateIndicator();
  }

  static updateIndicator() {
    const distance = Vector3.Distance(App.rightSkewerAtom.position, App.leftSkewerAtom.position);
    const graphX = distance * 100;
    const graphY = EnergyHelper.calculatePotentialEnergyPoint(this.currentSymbol1, this.currentSymbol2, graphX);

    const graph = document.getElementById("energyChart");
    const graphIndicator = document.getElementById("chart-indicator") as HTMLDivElement;
    const graphContent = document.getElementsByClassName("bglayer")[0] as SVGGraphicsElement;
    const bounding = graphContent.getBBox();

    const xBase = graph.offsetLeft + bounding.x;
    const yBase = graph.offsetTop + bounding.y;
    const graphWidth = bounding.width;
    const graphHeight = bounding.height;
    const graphYRange = this.graphUpperLimit - this.graphLowerLimit;

    const x = xBase + graphX / 1000 * graphWidth;
    const y = yBase + (this.graphUpperLimit - graphY) / graphYRange * graphHeight;

    graphIndicator.style.top = y + "px";
    graphIndicator.style.left = x + "px";

    document.body.dispatchEvent(new CustomEvent("update-chart-stats", { detail: { energyPotential: graphY, distance: graphX } }));
  }

  static showIndicator(show: boolean) {
    const graphIndicator = document.getElementById("chart-indicator") as HTMLDivElement;
    graphIndicator.style.display = show ? "block" : "none";

    document.body.dispatchEvent(new CustomEvent("show-warning", { detail: { show: !show } }));
  }
}
