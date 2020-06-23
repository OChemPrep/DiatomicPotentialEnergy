"use strict";
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result["default"] = mod;
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = __importStar(require("react"));
var ReactDOM = __importStar(require("react-dom"));
var userInterface_1 = require("./userInterface");
function renderApp() {
    var querystring = require('querystring');
    var url = require('url');
    var u = url.parse(window.location.href);
    var params = querystring.decode(u.query);
    ReactDOM.render(React.createElement(userInterface_1.UserInterface, { mode: params.mode }), document.getElementById('react-app'));
}
renderApp();
