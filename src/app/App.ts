import { Scene } from "@babylonjs/core/scene";
import { Vector3, Matrix, Color4 } from "@babylonjs/core/Maths/math"
import AtomView from "./view/AtomView";
import SceneManager from "./SceneManager";
import BackgroundController from "./BackgroundController";
import { Mesh, Ray, RayHelper, Camera, DeepImmutableObject, MeshBuilder } from "@babylonjs/core";
import DragController from "./DragController";
import MouseController from "./MouseController";
import GraphicFactory from "./view/GraphicFactory";
import * as querystring from "querystring";
import { ElementHelper } from "@alchemiesolns/elements";
import EnergyHelper from "./EnergyHelper";
import GraphController from "./chart/GraphController";

export default class App {
  static scene: Scene;
  static camera: Camera;
  static rightSkewerAtom: AtomView;
  static leftSkewerAtom: AtomView;
  static highlightedAtom: AtomView | null = null;

  private static configuration: string | string[] = "";

  static init(scene: Scene, canvas: HTMLCanvasElement) {
    this.scene = scene;
    this.camera = scene.cameras[0];

    BackgroundController.init();
    DragController.init(scene, scene.cameras[0], canvas);
  }


  static loadOptions(params: querystring.ParsedUrlQuery) {
    // if (params.configuration) {
    //   this.configuration = params.configuration;
    //   this.loadConfiguration(configuration);
    // }

    const myPoints = [
      new Vector3(-5, -3, 0),
      new Vector3(5, -3, 0)
    ];

    const skewerLine = MeshBuilder.CreateDashedLines("lines", { points: myPoints, updatable: true, dashNb: 10, gapSize: 3 }, this.scene);
    // skewerLine.scla = new Vector3(1, 2, 1);
    // console.log(skewerLine.getChildren());
    skewerLine.enableEdgesRendering();
    skewerLine.edgesWidth = 10.0;
    skewerLine.edgesColor = new Color4(1, 1, 1, 1)

    this.rightSkewerAtom = this.createSkewerAtom(new Vector3(-3, -3, 0), 8);
    this.leftSkewerAtom = this.createSkewerAtom(new Vector3(3, -3, 0), 8);

    const graphPoints = EnergyHelper.getEnergyGraphPoints("O", "O");
    GraphController.createGraph(graphPoints, "O", "O");
  }

  static update(dt: number) {
    BackgroundController.update(dt);
    MouseController.update();
  }

  static createSkewerAtom(position: Vector3, atomicNumber: number): AtomView {
    const element = ElementHelper.getElementByAtomicNumber(atomicNumber);

    const graphic = GraphicFactory.createAtomGraphic(atomicNumber);
    const symbolMesh = GraphicFactory.createSymbolMesh(element.symbol);
    const view = new AtomView(element.symbol, graphic, symbolMesh, true);

    view.position.copyFrom(position);

    return view;
  }

  static createDraggingAtom(atomicNumber: number, screenX: number, screenY: number) {
    const position = SceneManager.screenToWorldPosition(screenX, screenY);
    const element = ElementHelper.getElementByAtomicNumber(atomicNumber);

    const graphic = GraphicFactory.createAtomGraphic(atomicNumber);
    const symbolMesh = GraphicFactory.createSymbolMesh(element.symbol);
    const view = new AtomView(element.symbol, graphic, symbolMesh, false);

    view.position.copyFrom(position || Vector3.Zero());

    MouseController.activeObject = view;
    DragController.startDrag(MouseController.activeObject);
  }

  static getLeftBoundary(draggingAtom: AtomView): number {
    if (draggingAtom == this.rightSkewerAtom) {
      return this.leftSkewerAtom.getAbsolutePosition().x - 1;
    }

    return 5;
  }

  static getRightBoundary(draggingAtom: AtomView): number {
    if (draggingAtom == this.leftSkewerAtom) {
      return this.rightSkewerAtom.getAbsolutePosition().x + 1;
    }

    return -5;
  }

  static getClosestSkewerAtom(position: Vector3): AtomView {
    const leftDistance = Vector3.Distance(position, this.leftSkewerAtom.position);
    const rightDistance = Vector3.Distance(position, this.rightSkewerAtom.position);

    if (leftDistance < rightDistance) {
      return this.leftSkewerAtom;
    }

    return this.rightSkewerAtom;
  }

  static highlightClosestAtom(mousePosition: Vector3) {
    const closestAtom = this.getClosestSkewerAtom(mousePosition);
    const distance = Vector3.Distance(mousePosition, closestAtom.position);

    if (this.highlightedAtom) {
      this.highlightedAtom.graphic.renderOutline = false;
      this.highlightedAtom = null;
    }
    if (distance <= 3) {
      this.highlightedAtom = closestAtom;
      this.highlightedAtom.graphic.renderOutline = true;
    }
  }

  static endPaletteAtomDrag(atom: AtomView) {
    if (this.highlightedAtom) {
      this.replaceHighlightedAtom(atom);
      const data = EnergyHelper.getEnergyGraphPoints(this.rightSkewerAtom.symbol, this.leftSkewerAtom.symbol);
      if (data.length) {
        GraphController.updateGraph(data, this.leftSkewerAtom.symbol, this.rightSkewerAtom.symbol);
      } else {
        GraphController.showIndicator(false);
      }
    } else {
      atom.dispose();
    }
  }

  static replaceHighlightedAtom(atom: AtomView) {
    atom.isSkewerAtom = true;
    atom.position = this.highlightedAtom!.position;

    if (this.highlightedAtom == this.rightSkewerAtom) {
      this.rightSkewerAtom = atom;
    } else {
      this.leftSkewerAtom = atom;
    }

    this.highlightedAtom!.dispose();
    this.highlightedAtom = null;
  }

  static resize() {
    GraphController.redraw();
  }
}