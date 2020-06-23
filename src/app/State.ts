import { Scene } from "@babylonjs/core";

interface QueryParams {
    useAngstroms?: boolean;
}

export default class State {
    static scene: Scene;
    static params: QueryParams = {};
}