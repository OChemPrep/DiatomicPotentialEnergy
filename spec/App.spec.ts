import { NullEngine } from "@babylonjs/core/Engines/nullEngine";
import { Scene } from "@babylonjs/core/scene"
import App from "../src/app/App";
import { Vector3 } from "@babylonjs/core/Maths/math";
import Chem3dNode from "../src/view/Chem3dNode";
import { SpotLight, Camera } from "@babylonjs/core";

describe("App.ts", () => {

  const engine = new NullEngine();
  let scene: Scene;
  let app: App;

  beforeEach(() => {
    scene = new Scene(engine);
    new Camera("camera", Vector3.Zero(), scene);
    app = new App(scene, document.createElement("canvas"));
  })

  it("should create an app", () => {
    expect(app).toBeTruthy();
  })

  it("should create an atom", () => {
    const view = app.createAtom(6);
    expect(view).toBeTruthy();
  })

  it("should add the atom view to the scene", () => {
    const view = app.createAtom(6);
    const sceneNode = scene.rootNodes.find((node) => node instanceof Chem3dNode);
    expect(sceneNode).toBe(view.node);
  })

  it("should set the position of an atom", () => {
    const view = app.createAtom(6);
    const newPosition = new Vector3(1, 2, 3);
    app.setObjectPosition(view, newPosition, true);
    expect(view.node.position).toEqual(newPosition);
  })
})