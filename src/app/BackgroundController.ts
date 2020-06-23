import { Scene, Vector3, SpriteManager, Sprite } from '@babylonjs/core';
import App from './App';

class CustomSprite extends Sprite {
	offset: number = 0;
	radius: number = 0;
	speed: number = 0;
	startingPosition: Vector3 = Vector3.Zero();

	constructor(manager: SpriteManager) {
		super("sprite", manager);
	}
}

export default class BackgroundController {
	static manager: SpriteManager | null = null;
	static context: CanvasRenderingContext2D | null = null;
	static time: number = 0;
	static bubbles: Array<CustomSprite> = [];

	static init() {
		this.manager = new SpriteManager("bubbleManager", './images/bubble.png', 2000, 64, App.scene);

		const backgroundCanvas = document.getElementById('background') as HTMLCanvasElement;
		if (backgroundCanvas) {
			this.context = backgroundCanvas.getContext('2d');
			for (var x = 0; x <= 32; x++) {
				for (var y = 0; y <= 32; y++) {
					const rgb = Math.floor(192 + 20 * Math.cos((x * x - y * y) / 300));

					this.context!.fillStyle = "rgb(" + rgb + "," + rgb + "," + rgb + ")";
					this.context!.fillRect(x, y, 1, 1);
				}
			}
		}

		for (let i = 0; i < 100; i++) {
			const sprite = new CustomSprite(this.manager);
			sprite.position = this.getRandomPoint();
			sprite.size = Math.random() * 2 + 0.6;
			sprite.offset = Math.random() * 2 - 1;
			sprite.radius = Math.random() * 1 + 1;
			sprite.speed = Math.random() * 1 - 0.5;
			sprite.startingPosition = sprite.position.clone();
			this.bubbles.push(sprite);
		}
	}

	static getRandomPoint(): Vector3 {
		const pt = new Vector3(Math.random() * 2 - 1, Math.random() * 2 - 1, Math.random() * 2 - 1);
		return pt.normalize().scaleInPlace((12 + Math.random() * 8));
	}

	static update(deltaTime: number) {
		if (!this.context) return;

		// if deltaTime is too high then there was a time skip so don't include the jump
		if (deltaTime > 0.04) return;

		this.time += deltaTime;

		for (var x = 0; x <= 32; x++) {
			for (var y = 0; y <= 32; y++) {
				const rgb = Math.floor(192 + 20 * Math.cos((x * x - y * y) / 300 + this.time));

				this.context.fillStyle = "rgb(" + rgb + "," + rgb + "," + rgb + ")";
				this.context.fillRect(x, y, 1, 1);
			}
		}

		for (let i = 0; i < this.bubbles.length; i++) {
			const bubble = this.bubbles[i];
			const x = bubble.startingPosition.x + Math.sin(this.time * bubble.speed + bubble.offset) * bubble.radius;
			const y = bubble.startingPosition.y + Math.cos(this.time * bubble.speed + bubble.offset) * bubble.radius;
			const z = bubble.startingPosition.z + Math.sin(this.time * bubble.speed + bubble.offset) * bubble.radius;
			bubble.position = new Vector3(x, y, z);
		}
	}
}