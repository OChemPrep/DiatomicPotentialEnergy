import App from "./App";
import SceneManager from "./SceneManager";
import DragController from "./DragController";
import { Vector3, Scene, AbstractMesh } from "@babylonjs/core";
import AtomView from "./view/AtomView";

export default class MouseController {
    static x = 0;
    static y = 0;
    static isMouseMoving: boolean = false;
    static activeObject: AtomView | null = null;

    static update() {
    }

    static onMouseDown(x: number, y: number) {
        this.x = x;
        this.y = y;

        this.isMouseMoving = false;
        this.activeObject = SceneManager.getClosestView(this.x, this.y);

        if (this.activeObject) {
            DragController.startDrag(this.activeObject, new Vector3(x, y, 0));
        }
    }

    static onMouseMove(x: number, y: number) {
        this.x = x;
        this.y = y;

        this.isMouseMoving = true;

        if (!this.activeObject) return;

        DragController.drag(this.x, this.y);
    }

    static onMouseUp(x: number, y: number) {
        this.x = x;
        this.y = y;

        if (DragController.isDragging) {
            this.mouseUpDragging();
        }

        this.activeObject = null;

        DragController.endDrag();
    }

    static mouseUpDragging() {
    }

}