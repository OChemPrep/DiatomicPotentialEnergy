"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) result[k] = mod[k];
    result["default"] = mod;
    return result;
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = __importStar(require("react"));
var elements_1 = require("@alchemiesolns/elements");
require("./atomPool.css");
var react_fontawesome_1 = require("@fortawesome/react-fontawesome");
var free_solid_svg_icons_1 = require("@fortawesome/free-solid-svg-icons");
var elements = [
    { symbol: 'H', number: 1 },
    { symbol: 'Li', number: 3 },
    { symbol: 'Be', number: 4 },
    { symbol: 'B', number: 5 },
    { symbol: 'C', number: 6 },
    { symbol: 'N', number: 7 },
    { symbol: 'O', number: 8 },
    { symbol: 'F', number: 9 },
    { symbol: 'Al', number: 13 },
    { symbol: 'Si', number: 14 },
    { symbol: 'P', number: 15 },
    { symbol: 'S', number: 16 },
    { symbol: 'Cl', number: 17 },
    { symbol: 'Se', number: 34 },
    { symbol: 'Br', number: 35 },
    { symbol: 'Te', number: 52 },
    { symbol: 'I', number: 53 },
    { symbol: 'Xe', number: 54 },
    { symbol: 'Pt', number: 78 },
];
var AtomPool = /** @class */ (function (_super) {
    __extends(AtomPool, _super);
    function AtomPool(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            electronCount: 5,
            pushed: false
        };
        return _this;
    }
    AtomPool.prototype.render = function () {
        var _this = this;
        var pushedStyle = this.state.pushed ? "pushed" : "";
        return React.createElement("div", { className: "atom-pool-div " + pushedStyle },
            React.createElement("button", { className: 'atom-pool-button button-round', onClick: function () { return _this.pushOver(); } },
                React.createElement(react_fontawesome_1.FontAwesomeIcon, { className: 'fa-flip-horizontal', icon: free_solid_svg_icons_1.faAngleLeft })),
            React.createElement("div", { className: 'atom-pool panel' }, elements.map(function (el, x) {
                var element = elements_1.ElementHelper.getElementByAtomicNumber(el.number);
                var backgroundStyle = { backgroundColor: "#" + element.cpkHexColor };
                var luma = elements_1.ColorHelper.getLumaFromColorString(element.cpkHexColor);
                var textColor = luma < 0.8 ? "white" : "#333";
                var textStyle = { color: textColor };
                return React.createElement("div", { className: 'atom-pool-atom', onClick: function () { return _this.clickElement(el); }, key: x, style: backgroundStyle },
                    React.createElement("div", { className: 'atom-pool-atom-symbol', style: textStyle }, el.symbol),
                    React.createElement("div", { className: 'atom-pool-atom-shadow' }));
            })));
    };
    AtomPool.prototype.clickElement = function (element) {
        var canvas = document.getElementById('renderCanvas');
        if (canvas) {
            canvas.dispatchEvent(new CustomEvent('create-atom', { detail: { number: element.number } }));
        }
    };
    AtomPool.prototype.pushOver = function () {
        this.setState({ pushed: !this.state.pushed });
    };
    return AtomPool;
}(React.Component));
exports.AtomPool = AtomPool;
