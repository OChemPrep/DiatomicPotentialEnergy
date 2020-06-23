import { Scene } from "@babylonjs/core/scene";
import { MeshBuilder } from "@babylonjs/core/Meshes/meshBuilder";
import { Mesh } from "@babylonjs/core/Meshes/mesh";
import { TransformNode } from "@babylonjs/core/Meshes/transformNode";
import { AdvancedDynamicTexture } from "@babylonjs/gui/2D/advancedDynamicTexture";
import { Image } from "@babylonjs/gui/2D/controls/image";
import { TextBlock } from "@babylonjs/gui/2D/controls/textBlock";
import { ElementHelper } from "@alchemiesolns/elements";
import { Vector3, Color3, Quaternion, Axis, Vector2 } from "@babylonjs/core/Maths/math";
import { StandardMaterial, DynamicTexture, Material, Camera, Texture } from "@babylonjs/core";
import AtomView from "./AtomView";
import App from "../App";

interface RGB {
  r: number;
  g: number;
  b: number;
}

export default class GraphicFactory {

  static textMaterials = new Map<string, Material>();
  static aa: Material;

  static generateTexture(plane: Mesh, color: number, text: string, fontSize: number, background: string = ""): Material {
    const key = text + " " + color + " " + background;
    const existingMaterial = this.textMaterials.get(key);
    if (existingMaterial) {
      return existingMaterial;
    }

    const texture = AdvancedDynamicTexture.CreateForMesh(plane);
    const textBlock = new TextBlock("symbolText " + text, text);
    textBlock.fontSize = fontSize;
    textBlock.fontFamily = "PT Sans";
    textBlock.color = "#" + color.toString(16);

    if (color == 16777215) {
      // if the color is white then thicken the text
      textBlock.shadowBlur = 10;
      textBlock.shadowColor = '#fff';
    }

    if (background) {
      texture.addControl(new Image("warning background", background));
    }

    texture.addControl(textBlock);

    this.textMaterials.set(key, plane.material!);
    return plane.material!;
  }

  static createAtomGraphic(atomicNumber: number): Mesh {
    const element = ElementHelper.getElementByAtomicNumber(atomicNumber);
    const color = ElementHelper.getElementColorBySymbolRGB(element.symbol);
    const diameter = element.vanDelWaalsRadius / 216 * 1.5;
    const mesh = this.createSphereMesh(element.symbol, color, diameter);

    return mesh;
  }

  static createSphereMesh(name: string, color: RGB | null, diameter: number = 1.0): Mesh {
    const sphere = MeshBuilder.CreateSphere(name, { diameter: diameter });

    const myMaterial = new StandardMaterial("sphereMaterial", App.scene);

    if (color) {
      myMaterial.diffuseColor = new Color3(color.r / 1000, color.g / 1000, color.b / 1000);
      myMaterial.specularColor = new Color3(0, 0, 0);
      myMaterial.emissiveColor = new Color3(color.r / 500, color.g / 500, color.b / 500);
    } else {
      myMaterial.diffuseColor = new Color3(0.25, 0.25, 0.25);
      myMaterial.specularColor = new Color3(0, 0, 0);
      myMaterial.emissiveColor = new Color3(0.5, 0.5, 0.5);
    }

    sphere.material = myMaterial;
    sphere.outlineColor = Color3.FromHexString("#ffff42");
    sphere.outlineWidth = 0.05;

    return sphere;
  }

  static createSymbolMesh(symbol: string): Mesh {
    const plane = MeshBuilder.CreatePlane("symbol plane " + symbol, { size: 1 });
    const symbolColor = ElementHelper.getSymbolColorBySymbol(symbol);

    plane.material = this.generateTexture(plane, symbolColor, symbol, 300);
    plane.isPickable = false;
    plane.lookAt(App.camera.position.negate());

    return plane;
  }

}