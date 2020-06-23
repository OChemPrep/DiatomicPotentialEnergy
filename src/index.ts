import { Engine } from "@babylonjs/core/Engines/engine";
import { Scene } from "@babylonjs/core/scene";
import { Color4, Vector3 } from "@babylonjs/core/Maths/math"
import { HemisphericLight } from "@babylonjs/core/Lights/hemisphericLight";
import { PointLight } from "@babylonjs/core/Lights/pointLight";
import { ArcRotateCamera } from "@babylonjs/core/Cameras/arcRotateCamera";
import * as querystring from "querystring";
import * as url from "url";
import App from "./app/App";
import FontFaceObserver from 'fontfaceobserver';
import './images/bubble.png';
import MouseController from "./app/MouseController";
import State from "./app/State";
import { Camera } from "@babylonjs/core";


{
  const version = "0.0.1";
  console.log("Alchemie Lewis Structure 3D Explorer " + version);
  console.log("Proprietary and Confidential");
  console.log("Copyright Alchemie 2019-2020");

  // Disable right-click in the app.
  document.addEventListener("contextmenu", function (e) {
    e.preventDefault();
  }, false);

  const params = querystring.decode(url.parse(window.location.href).query || "");

  const canvas = document.getElementById("foreground") as HTMLCanvasElement; // Get the canvas element 
  const engine = new Engine(canvas, true, { stencil: true }); // Generate the BABYLON 3D engine
  const scene = new Scene(engine);
  State.scene = scene;

  engine.renderEvenInBackground = false;

  scene.clearColor = new Color4(0, 0, 0, 0);

  // Add a camera to the scene and attach it to the canvas
  const radius = 10;
  const camera = new ArcRotateCamera("Camera", Math.PI / 2, Math.PI / 2, radius, Vector3.Zero(), scene);
  camera.lowerRadiusLimit = radius;
  camera.upperRadiusLimit = radius;
  camera.mode = Camera.ORTHOGRAPHIC_CAMERA;
  camera.orthoLeft = -window.innerWidth / 100;
  camera.orthoRight = window.innerWidth / 100;
  camera.orthoTop = window.innerHeight / 100;
  camera.orthoBottom = -window.innerHeight / 100;

  // Add lights to the scene
  const light1 = new HemisphericLight("light1", new Vector3(1, 1, 0), scene);
  const light2 = new PointLight("light2", camera.position, scene);

  App.init(scene, canvas);

  // load the app once the font has been loaded
  new FontFaceObserver('PT Sans').load().then(function () {
    App.loadOptions(params);
  });

  scene.beforeRender = () => {
    App.update(engine.getDeltaTime() / 1000.0);
  };

  scene.onPointerDown = (evt: PointerEvent) => {
    MouseController.onMouseDown(evt.clientX, evt.clientY);
  }

  scene.onPointerMove = (evt: PointerEvent) => {
    MouseController.onMouseMove(evt.clientX, evt.clientY);
  }

  scene.onPointerUp = (evt: PointerEvent) => {
    MouseController.onMouseUp(evt.clientX, evt.clientY);
  }

  // after 500ms, disable rendering
  // Note, _windowIsBackground listens to the blur and focus 
  // setTimeout(() => {
  //   engine._windowIsBackground = true;
  // }, 500);

  // window.addEventListener("pointerdown", () => {
  //   engine._windowIsBackground = false;
  // });


  // Install external communication handlers.
  // window.onmessage = function (e) {
  //   if (e.data === 'submit') {
  //     const achieved = app.isGoalAchieved();
  //     const result = {
  //       type: 'goal-completed',
  //       success: achieved,
  //       name: e.target.name
  //     }
  //     window.top.postMessage(result, event.origin);
  //   } else if (e.data === 'get-configuration') {
  //     const config = app.getConfigString();
  //     const result = {
  //       type: 'configuration',
  //       config: config,
  //       name: e.target.name
  //     }
  //     window.top.postMessage(result, event.origin);
  //   }
  // }

  // Register a render loop to repeatedly render the scene
  engine.runRenderLoop(function () {
    scene.render();
  });

  // Watch for browser/canvas resize events
  window.addEventListener("resize", function () {
    camera.orthoLeft = -window.innerWidth / 100;
    camera.orthoRight = window.innerWidth / 100;
    camera.orthoTop = window.innerHeight / 100;
    camera.orthoBottom = -window.innerHeight / 100;

    engine.resize();
    App.resize();
  });

  setTimeout(() => {
    // after 1 second when page is loaded, manually resize the canvas to match the screen
    engine.resize();
  }, 1000);

  document.body.addEventListener('create-dragging-atom', (e) =>
    App.createDraggingAtom((e as CustomEvent).detail.number, (e as CustomEvent).detail.x, (e as CustomEvent).detail.y), false);
  document.body.addEventListener('mouse-up', (e) => MouseController.onMouseUp((e as CustomEvent).detail.x, (e as CustomEvent).detail.y), false);
  document.body.addEventListener('drag-atom', (e) => MouseController.onMouseMove((e as CustomEvent).detail.x, (e as CustomEvent).detail.y), false);

}
