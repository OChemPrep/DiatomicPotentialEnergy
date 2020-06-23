import { ElementHelper } from '@alchemiesolns/elements';
import { Vector2 } from "@babylonjs/core";
import diatomicData from "./DiatomicData";
import GraphController from './chart/GraphController';



export default class EnergyHelper {

    static getEnergyGraphPoints(atomSymbol1: string, atomSymbol2: string): Vector2[] {
        const sigma = this.calculateSigma(atomSymbol1, atomSymbol2);
        const vectors = this.calculatePotentialEnergyPoints(atomSymbol1, atomSymbol2, sigma * 0.7, 1500, 100);

        return vectors;
    }

    static calculatePotentialEnergyPoints(a: string, b: string, minRadius: number, maxRadius: number, numPoints: number): Vector2[] {
        var values: Vector2[] = [];

        const epsilon = this.getDiatomicBondStrength(a, b);

        if (epsilon > 0) {
            var sigma = this.calculateSigma(a, b);

            let radius = minRadius;
            const radiusDelta = (maxRadius - minRadius) / (numPoints - 1);

            for (let i = 0; i < numPoints; i++) {
                values.push(new Vector2(radius, this.calculatePotentialEnergySigma(sigma, epsilon, radius)));
                radius += radiusDelta;
            }
        }

        return values;
    }

    static calculatePotentialEnergyPoint(a: string, b: string, distance: number): number {
        const epsilon = this.getDiatomicBondStrength(a, b);

        if (epsilon > 0) {
            var sigma = this.calculateSigma(a, b);

            return this.calculatePotentialEnergySigma(sigma, epsilon, distance);
        }

        return 0;
    }


    // CalculatePotentialEnergy(a: string, b: string, radius: number): number {
    //     var sigma = this.calculateSigma(a, b);

    //     const epsilon = this.getDiatomicBondStrength(a, b);

    //     return this.calculatePotentialEnergySigma(sigma, epsilon, radius);
    // }


    static calculateSigma(a: string, b: string): number {
        const element1 = ElementHelper.getElementBySymbol(a);
        const element2 = ElementHelper.getElementBySymbol(b);
        const sigma = element1.vanDelWaalsRadius + element2.vanDelWaalsRadius; // pm

        return sigma;
    }


    static calculatePotentialEnergySigma(sigma: number, epsilon: number, radius: number): number {
        // Equation taken from https://en.wikipedia.org/wiki/Lennard-Jones_potential

        const s_r = sigma / radius;
        const sr6 = s_r * s_r * s_r * s_r * s_r * s_r;
        const V = 4 * epsilon * ((sr6 * sr6) - sr6);

        return V;
    }

    static getDiatomicBondStrength(a: string, b: string): number {
        if (a > b) {
            const x = a;
            a = b;
            b = x;
        }

        const key = `${a} - ${b}`;
        const strength = diatomicData.get(key);

        return strength || 0;
    }
}
