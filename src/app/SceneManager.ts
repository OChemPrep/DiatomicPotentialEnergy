import { Scene } from "@babylonjs/core/scene";
import { Vector3, Matrix } from "@babylonjs/core/Maths/math";
import { PickingInfo } from "@babylonjs/core/Collisions/pickingInfo";
import "@babylonjs/core/Culling/ray";
import { AbstractMesh } from "@babylonjs/core/Meshes/abstractMesh";
import AtomView from "./view/AtomView";
import App from "./App";

export default class SceneManager {

  static getScreenPosition(worldPosition: Vector3): Vector3 {
    const engine = App.scene.getEngine();
    const camera = App.scene.activeCamera!;

    return Vector3.Project(
      worldPosition,
      Matrix.IdentityReadOnly,
      App.scene.getTransformMatrix(),
      camera.viewport.toGlobal(engine.getRenderWidth(), engine.getRenderHeight()),
    );
  }

  static screenToWorldPosition(x: number, y: number): Vector3 {
    const engine = App.scene.getEngine();
    const vector = Vector3.Unproject(
      new Vector3(x, y, 0),
      engine.getRenderWidth(),
      engine.getRenderHeight(),
      Matrix.Identity(),
      App.scene.getViewMatrix(),
      App.scene.getProjectionMatrix());

      vector.z = 0;

    return vector;
  }

  static getObjectsAtScreenPosition(x: number, y: number): PickingInfo[] | null {
    const pickInfos = App.scene.multiPick(x, y);
    return pickInfos;
  }

  static getClosestView(x: number, y: number ): AtomView | null {
    const picks = this.getObjectsAtScreenPosition(x, y);
    if (!picks || picks.length == 0) return null;
    
    const sorted = picks.sort((a, b) => a.distance - b.distance);

      return sorted[0].pickedMesh?.parent as AtomView;
  }
}