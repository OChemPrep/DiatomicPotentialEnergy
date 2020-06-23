import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { UserInterface } from "./userInterface";
import State from '../app/State';
import * as querystring from "querystring";
import * as url from "url";

function renderApp() {

    const params = querystring.decode(url.parse(window.location.href).query || "");

    State.params.useAngstroms = params.angstroms == "true";

    ReactDOM.render(
        <UserInterface mode={params.mode?.toString()} />,
        document.getElementById('react-app')
    );
}

renderApp();