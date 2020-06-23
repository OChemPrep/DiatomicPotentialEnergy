import * as React from 'react';
import { ColorHelper, ElementHelper } from '@alchemiesolns/elements';
import './atomPool.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleRight } from '@fortawesome/free-solid-svg-icons';

interface IAtomPoolState {
	electronCount: number;
	pushed: boolean;
}

interface Elem {
	symbol: string;
	number: number;
}

const elements: Array<Elem> = [
	{ symbol: 'H', number: 1 },
	// { symbol: 'Li', number: 3 },
	{ symbol: 'Be', number: 4 },
	{ symbol: 'B', number: 5 },
	{ symbol: 'C', number: 6 },
	{ symbol: 'N', number: 7 },
	{ symbol: 'O', number: 8 },
	{ symbol: 'Mg', number: 12 },
	{ symbol: 'Al', number: 13 },
	{ symbol: 'Si', number: 14 },
	{ symbol: 'P', number: 15 },
	{ symbol: 'S', number: 16 },
	{ symbol: 'Cl', number: 17 },
	{ symbol: 'Ar', number: 18 },
	// { symbol: 'Se', number: 34 },
	{ symbol: 'Br', number: 35 },
	// { symbol: 'Te', number: 52 },
	// { symbol: 'I', number: 53 },
	{ symbol: 'Xe', number: 54 },
	// { symbol: 'Pt', number: 78 },
];

export class AtomPool extends React.Component<{}, IAtomPoolState> {
	holding: boolean = false;
	touchOutsideAtom: boolean = false;
	atomNumber: number = 0;
	lastX: number = 0;
	lastY: number = 0;

	constructor(props: Readonly<{}>) {
		super(props);

		this.state = {
			electronCount: 5,
			pushed: false
		};

		document.body.addEventListener("mouseup", (e) => this.onMouseUp(e.pageX, e.pageY));
	}

	public render() {
		const pushedStyle = this.state.pushed ? "pushed" : "";

		return <div className={`atom-pool-div ${pushedStyle}`}>
			{/* <button className='atom-pool-button button-round' onClick={() => this.pushOver()}>
				<FontAwesomeIcon icon={faAngleRight} />
			</button> */}
			<div className='atom-pool panel'>
				{elements.map((el, x) => {
					const element = ElementHelper.getElementByAtomicNumber(el.number);
					const luma = ColorHelper.getLumaFromColorString(element.cpkHexColor);
					const textColor = luma < 0.8 ? "white" : "#333";
					const textStyle = { color: textColor };
					const atomSize = element.vanDelWaalsRadius / 216 * 100;
					const fontSize = element.vanDelWaalsRadius / 216 * 40;
					const style = { backgroundColor: "#" + element.cpkHexColor, width: atomSize + "px", height: atomSize + "px", fontSize: fontSize + "px" };

					return <div className={'atom-pool-atom atom' + el.number} key={x} style={style}
						onClick={() => this.clickElement(el.number)}
						onMouseDown={() => this.onMouseDown(el.number)}
						onMouseLeave={(e) => this.onMouseOut(e.pageX, e.pageY)}
						onTouchStart={() => this.onMouseDown(el.number)}
						onTouchEnd={() => this.onTouchEnd()}>
						<div className='atom-pool-atom-symbol' style={textStyle}>{el.symbol}</div>
						<div className='atom-pool-atom-shadow'></div>
					</div>
				}
				)}
			</div>
		</div>
	}

	componentDidMount() {
		const atoms = document.getElementsByClassName("atom-pool-atom");
		for (let i = 0; i < atoms.length; i++) {
			atoms[i].addEventListener("touchmove", (e) => {
				this.onTouchMove(e);
				e.preventDefault();
			});
		}
	}

	onTouchMove(e: any) {
		e.preventDefault();

		const x = this.lastX = e.touches[0].clientX;
		const y = this.lastY = e.touches[0].clientY;
		const atom = document.getElementsByClassName("atom" + this.atomNumber)[0];

		if (!this.touchOutsideAtom && atom) {
			const rect = atom.getBoundingClientRect();

			const isTouchOverSvg = y > rect.top && y < rect.top + rect.height &&
				x > rect.left && x < rect.left + rect.width;

			if (!isTouchOverSvg) {
				this.onMouseOut(x, y);
				this.touchOutsideAtom = true;
			}
		}

		document.body.dispatchEvent(new CustomEvent('drag-atom', { detail: { x: x, y: y } }));
	}

	onTouchEnd() {
		// document.body.dispatchEvent(new Event('release-atom'));
		this.onMouseUp(this.lastX, this.lastY);
	}

	clickElement(atomNumber: number) {
		document.body.dispatchEvent(new CustomEvent('create-atom', { detail: { number: atomNumber } }));
	}

	onMouseUp(x: number, y: number) {
		this.atomNumber = 0;
		this.holding = false;

		document.body.dispatchEvent(new CustomEvent('mouse-up', { detail: { x: x, y: y } }));
	}

	onMouseDown(atomNumber: number) {
		this.atomNumber = atomNumber;
		this.touchOutsideAtom = false;
	}

	onMouseOut(x: number, y: number) {
		if (this.atomNumber && !this.holding) {
			this.holding = true;

			document.body.dispatchEvent(new CustomEvent('create-dragging-atom', { detail: { number: this.atomNumber, x: x, y: y } }));
		}
	}

	pushOver() {
		this.setState({ pushed: !this.state.pushed });
	}
}