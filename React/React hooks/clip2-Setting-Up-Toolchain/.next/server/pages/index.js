/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
(function() {
var exports = {};
exports.id = "pages/index";
exports.ids = ["pages/index"];
exports.modules = {

/***/ "./pages/index.js":
/*!************************!*\
  !*** ./pages/index.js ***!
  \************************/
/***/ (function(__unused_webpack_module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"react/jsx-dev-runtime\");\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react */ \"react\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);\n\nvar _jsxFileName = \"/Users/diego.guillen/Documents/GitHub/dev_docs/React/React hooks/clip2-Setting-Up-Toolchain/pages/index.js\";\n\n\nconst InputElement = () => {\n  const {\n    0: isLoading,\n    1: setIsLoading\n  } = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(true);\n  (0,react__WEBPACK_IMPORTED_MODULE_1__.useEffect)(() => {\n    setTimeout(() => {\n      setIsLoading(false);\n    }, 2000);\n  });\n  return isLoading ? /*#__PURE__*/(0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n    children: \"Loading...\"\n  }, void 0, false, {\n    fileName: _jsxFileName,\n    lineNumber: 11,\n    columnNumber: 24\n  }, undefined) : /*#__PURE__*/(0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"input\", {\n    placeholder: \"Enter Some Text\"\n  }, void 0, false, {\n    fileName: _jsxFileName,\n    lineNumber: 11,\n    columnNumber: 48\n  }, undefined);\n};\n\n/* harmony default export */ __webpack_exports__[\"default\"] = (InputElement);\n/*\nconst InputElement = () => {\n    const [inputText, setInputText] = useState(\"\");\n    const [historyList, setHistoryList] = useState([]);\n\n    return <div>\n    <input onChange={(e)=>{\n        setInputText(e.target.value);\n        setHistoryList([...historyList, e.target.value]);\n    }}placeholder=\"Enter some text\"/><br/>\n    {inputText}\n    <hr/><br/>\n    <ul>\n        {historyList.map((rec)=> {\n            return <div>{rec}</div>\n        })}\n    </ul>\n    </div>\n};\n\nexport default InputElement;*///# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9jbGlwMi1TZXR0aW5nLVVwLVRvb2xjaGFpbi8uL3BhZ2VzL2luZGV4LmpzPzQ0ZDgiXSwibmFtZXMiOlsiSW5wdXRFbGVtZW50IiwiaXNMb2FkaW5nIiwic2V0SXNMb2FkaW5nIiwidXNlU3RhdGUiLCJ1c2VFZmZlY3QiLCJzZXRUaW1lb3V0Il0sIm1hcHBpbmdzIjoiOzs7Ozs7O0FBQUE7O0FBRUEsTUFBTUEsWUFBWSxHQUFHLE1BQU07QUFDdkIsUUFBTTtBQUFBLE9BQUNDLFNBQUQ7QUFBQSxPQUFZQztBQUFaLE1BQTRCQywrQ0FBUSxDQUFDLElBQUQsQ0FBMUM7QUFFQUMsa0RBQVMsQ0FBQyxNQUFLO0FBQ1hDLGNBQVUsQ0FBQyxNQUFJO0FBQ1hILGtCQUFZLENBQUMsS0FBRCxDQUFaO0FBQ0gsS0FGUyxFQUVQLElBRk8sQ0FBVjtBQUdILEdBSlEsQ0FBVDtBQUtBLFNBQU9ELFNBQVMsZ0JBQUc7QUFBQTtBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQUEsZUFBSCxnQkFBMkI7QUFBTyxlQUFXLEVBQUM7QUFBbkI7QUFBQTtBQUFBO0FBQUE7QUFBQSxlQUEzQztBQUNILENBVEQ7O0FBV0EsK0RBQWVELFlBQWY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EiLCJmaWxlIjoiLi9wYWdlcy9pbmRleC5qcy5qcyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCBSZWFjdCwge3VzZUVmZmVjdCwgdXNlU3RhdGV9IGZyb20gJ3JlYWN0JztcblxuY29uc3QgSW5wdXRFbGVtZW50ID0gKCkgPT4ge1xuICAgIGNvbnN0IFtpc0xvYWRpbmcsIHNldElzTG9hZGluZ10gPSB1c2VTdGF0ZSh0cnVlKTtcblxuICAgIHVzZUVmZmVjdCgoKT0+IHtcbiAgICAgICAgc2V0VGltZW91dCgoKT0+e1xuICAgICAgICAgICAgc2V0SXNMb2FkaW5nKGZhbHNlKVxuICAgICAgICB9LCAyMDAwKTtcbiAgICB9KVxuICAgIHJldHVybiBpc0xvYWRpbmcgPyA8ZGl2PkxvYWRpbmcuLi48L2Rpdj4gOiA8aW5wdXQgcGxhY2Vob2xkZXI9XCJFbnRlciBTb21lIFRleHRcIi8+O1xufTtcblxuZXhwb3J0IGRlZmF1bHQgSW5wdXRFbGVtZW50O1xuLypcbmNvbnN0IElucHV0RWxlbWVudCA9ICgpID0+IHtcbiAgICBjb25zdCBbaW5wdXRUZXh0LCBzZXRJbnB1dFRleHRdID0gdXNlU3RhdGUoXCJcIik7XG4gICAgY29uc3QgW2hpc3RvcnlMaXN0LCBzZXRIaXN0b3J5TGlzdF0gPSB1c2VTdGF0ZShbXSk7XG5cbiAgICByZXR1cm4gPGRpdj5cbiAgICA8aW5wdXQgb25DaGFuZ2U9eyhlKT0+e1xuICAgICAgICBzZXRJbnB1dFRleHQoZS50YXJnZXQudmFsdWUpO1xuICAgICAgICBzZXRIaXN0b3J5TGlzdChbLi4uaGlzdG9yeUxpc3QsIGUudGFyZ2V0LnZhbHVlXSk7XG4gICAgfX1wbGFjZWhvbGRlcj1cIkVudGVyIHNvbWUgdGV4dFwiLz48YnIvPlxuICAgIHtpbnB1dFRleHR9XG4gICAgPGhyLz48YnIvPlxuICAgIDx1bD5cbiAgICAgICAge2hpc3RvcnlMaXN0Lm1hcCgocmVjKT0+IHtcbiAgICAgICAgICAgIHJldHVybiA8ZGl2PntyZWN9PC9kaXY+XG4gICAgICAgIH0pfVxuICAgIDwvdWw+XG4gICAgPC9kaXY+XG59O1xuXG5leHBvcnQgZGVmYXVsdCBJbnB1dEVsZW1lbnQ7Ki8iXSwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./pages/index.js\n");

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "react" ***!
  \************************/
/***/ (function(module) {

"use strict";
module.exports = require("react");;

/***/ }),

/***/ "react/jsx-dev-runtime":
/*!****************************************!*\
  !*** external "react/jsx-dev-runtime" ***!
  \****************************************/
/***/ (function(module) {

"use strict";
module.exports = require("react/jsx-dev-runtime");;

/***/ })

};
;

// load runtime
var __webpack_require__ = require("../webpack-runtime.js");
__webpack_require__.C(exports);
var __webpack_exec__ = function(moduleId) { return __webpack_require__(__webpack_require__.s = moduleId); }
var __webpack_exports__ = (__webpack_exec__("./pages/index.js"));
module.exports = __webpack_exports__;

})();