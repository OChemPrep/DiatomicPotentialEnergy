import React, { ReactNode } from "react";

interface ChartDataState {
    showWarning: boolean;
    energyPotential: number;
    distance: number;
}

export default class ChartData extends React.Component<{}, ChartDataState> {
    constructor(props: Readonly<{}>) {
        super(props);

        this.state = {
            showWarning: false,
            energyPotential: 0,
            distance: 0
        };

        document.body.addEventListener("update-chart-stats", (e: Event) => this.updateStats(e as CustomEvent));
        document.body.addEventListener("show-warning", (e: Event) => this.setState({ showWarning: (e as CustomEvent).detail.show }));
    }

    render(): ReactNode {
        return <div className="chart-data">
            {this.renderStats()}
        </div>;
    }

    renderStats(): ReactNode {
        if (this.state.showWarning) {
            return <div className="chart-warning">
                No Interaction
            </div>;
        }

        const energyPotential = Math.round(this.state.energyPotential * 100) / 100;
        const distance = Math.round(this.state.distance * 100) / 100;

        return <div className="chart-stats">
            <div className="chart-stats-row">
                Energy Potential: <div className="chart-stats-value">{energyPotential} kJ/mol</div>
            </div>
            <div className="chart-stats-row">
                Distance: <div className="chart-stats-value">{distance} pm</div>
            </div>
        </div>;
    }

    updateStats(e: CustomEvent) {
        this.setState({ energyPotential: e.detail.energyPotential, distance: e.detail.distance });
    }
}