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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var react_1 = __importDefault(require("react"));
require("./app.css");
var electronPool_1 = require("./electronPool/electronPool");
var toggleWarnings_1 = require("./toggleWarnings");
var trashButton_1 = require("./trashButton");
var atomPool_1 = require("./atomPool/atomPool");
var resetButton_1 = require("./resetButton");
var saveButton_1 = require("./saveButton");
var modal_1 = require("./modal");
var toast_1 = require("./toast");
var playGuide_1 = require("./playGuide/playGuide");
var helpButton_1 = require("./helpButton");
var authorModeDiv = react_1.default.createElement("div", { className: 'user-interface' },
    react_1.default.createElement("div", { className: "top-left" },
        react_1.default.createElement(trashButton_1.TrashButton, null),
        react_1.default.createElement(resetButton_1.ResetButton, null),
        react_1.default.createElement(saveButton_1.SaveButton, null)),
    react_1.default.createElement(electronPool_1.ElectronPool, null),
    react_1.default.createElement(atomPool_1.AtomPool, null),
    react_1.default.createElement(toggleWarnings_1.ToggleWarnings, null),
    react_1.default.createElement(modal_1.Modal, null),
    react_1.default.createElement(toast_1.Toast, null));
var exploreModeDiv = react_1.default.createElement("div", { className: 'user-interface' },
    react_1.default.createElement("div", { className: "top-left" },
        react_1.default.createElement(trashButton_1.TrashButton, null),
        react_1.default.createElement(resetButton_1.ResetButton, null)),
    react_1.default.createElement(playGuide_1.PlayGuide, null),
    react_1.default.createElement(helpButton_1.HelpButton, null),
    react_1.default.createElement(electronPool_1.ElectronPool, null),
    react_1.default.createElement(atomPool_1.AtomPool, null),
    react_1.default.createElement(toggleWarnings_1.ToggleWarnings, null),
    react_1.default.createElement(modal_1.Modal, null),
    react_1.default.createElement(toast_1.Toast, null));
var presentModeDiv = react_1.default.createElement("div", { className: 'user-interface' },
    react_1.default.createElement("div", { className: "top-left" },
        react_1.default.createElement(resetButton_1.ResetButton, null)));
var assessModeDiv = react_1.default.createElement("div", { className: 'user-interface' },
    react_1.default.createElement("div", { className: "top-left" },
        react_1.default.createElement(resetButton_1.ResetButton, null)),
    react_1.default.createElement(playGuide_1.PlayGuide, null),
    react_1.default.createElement(helpButton_1.HelpButton, null),
    react_1.default.createElement(electronPool_1.ElectronPool, null),
    react_1.default.createElement(toggleWarnings_1.ToggleWarnings, null),
    react_1.default.createElement(modal_1.Modal, null),
    react_1.default.createElement(toast_1.Toast, null));
var UserInterface = /** @class */ (function (_super) {
    __extends(UserInterface, _super);
    function UserInterface(props) {
        return _super.call(this, props) || this;
    }
    UserInterface.prototype.render = function () {
        if (this.props.mode === 'author') {
            return authorModeDiv;
        }
        else if (this.props.mode === 'present') {
            return presentModeDiv;
        }
        else if (this.props.mode === 'assess') {
            return assessModeDiv;
        }
        else {
            return exploreModeDiv;
        }
    };
    return UserInterface;
}(react_1.default.Component));
exports.UserInterface = UserInterface;
