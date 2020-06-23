import { PlaneBuilder, Scene, Camera, Mesh, Vector3 } from "@babylonjs/core";
import AtomView from "./view/AtomView";
import SceneManager from "./SceneManager";
import App from "./App";
import GraphController from "./chart/GraphController";

export default class DragController {
    static canvas: HTMLCanvasElement;
    static scene: Scene;
    static camera: Camera;
    static dragPlane: Mesh;
    static draggingAtom: AtomView | null = null;
    static dragOffset: Vector3 = Vector3.Zero();
    static controlsActive: boolean = false;
    static isDragging: boolean = false;

    static init(scene: Scene, camera: Camera, canvas: HTMLCanvasElement) {
        this.scene = scene;
        this.camera = camera;
        this.canvas = canvas;
    }

    static startDrag(object: AtomView, mousePosition?: Vector3) {
        this.draggingAtom = object;

        if (mousePosition) {
            const worldMousePosition = SceneManager.screenToWorldPosition(mousePosition.x, mousePosition.y)
            this.dragOffset = object.position.subtract(worldMousePosition);
        }
    }

    static drag(x: number, y: number) {

        const position = SceneManager.screenToWorldPosition(x, y).add(this.dragOffset);
        if (this.draggingAtom) {
            if (this.draggingAtom.isSkewerAtom) {
                this.dragSkewerAtom(position, this.draggingAtom);
            }
            else {
                this.dragPaletteAtom(position, this.draggingAtom);
            }
        }

        this.isDragging = true;
    }

    static dragSkewerAtom(mousePosition: Vector3, atom: AtomView) {
        const leftBoundary = App.getLeftBoundary(atom);
        const rightBoundary = App.getRightBoundary(atom);

        mousePosition.x = Math.min(Math.max(mousePosition.x, rightBoundary), leftBoundary);
        mousePosition.y = atom.getAbsolutePosition().y;

        atom.setAbsolutePosition(mousePosition);

        GraphController.updateIndicator();
    }

    static dragPaletteAtom(mousePosition: Vector3, atom: AtomView) {
        App.highlightClosestAtom(mousePosition);

        atom.setAbsolutePosition(mousePosition);
    }

    static endDrag() {
        if (!this.draggingAtom) return;

        if (!this.draggingAtom.isSkewerAtom) {
            App.endPaletteAtomDrag(this.draggingAtom);
        }

        this.draggingAtom = null;
        this.dragOffset = Vector3.Zero();
        this.isDragging = false;
    }

}