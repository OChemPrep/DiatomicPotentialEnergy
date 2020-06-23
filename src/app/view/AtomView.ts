import { Vector3, Color3 } from "@babylonjs/core/Maths/math";
import { StandardMaterial, TransformNode, EventState, AbstractMesh, Mesh } from "@babylonjs/core";
import State from "../State";

export default class AtomView extends AbstractMesh {

  public symbol: string;
  public graphic: Mesh;
  public isSkewerAtom: boolean;

  constructor(symbol: string, graphic: Mesh, symbolMesh: Mesh, isSkewerAtom: boolean) {
    super('atom', State.scene );

    this.symbol = symbol;
    this.graphic = graphic;
    this.isSkewerAtom = isSkewerAtom;
    
    graphic.parent = this;
    symbolMesh.parent = this;

    symbolMesh.position.z = 1;
  }
}