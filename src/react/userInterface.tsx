import React from 'react';
import './app.css';
import { AtomPool } from './atomPool/atomPool';
import ChartData from './chartData';

interface IUserInterfaceProps {
	mode: string;
}

const authorModeDiv =
	<div className='user-interface'>
		<AtomPool />
		<ChartData />
	</div>

const exploreModeDiv =
	<div className='user-interface'>
		<AtomPool />
		<ChartData />
	</div>

const presentModeDiv =
	<div className='user-interface'>
	</div>

const assessModeDiv =
	<div className='user-interface'>
		<AtomPool />
		<ChartData />
	</div>

export class UserInterface extends React.Component<IUserInterfaceProps, {}> {
	constructor(props: Readonly<IUserInterfaceProps>) {
		super(props);
	}

	public render() {
		if (this.props.mode === 'author') {
			return authorModeDiv;
		} else if (this.props.mode === 'present') {
			return presentModeDiv;
		} else if (this.props.mode === 'assess') {
			return assessModeDiv;
		} else {
			return exploreModeDiv;
		}
	}
}