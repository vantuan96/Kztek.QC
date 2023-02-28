/*jslint browser: true*/
/*jslint jquery: true*/
!function (a) { function b(b) { if ("string" == typeof b.data && (b.data = { keys: b.data }), b.data && b.data.keys && "string" == typeof b.data.keys) { var c = b.handler, d = b.data.keys.toLowerCase().split(" "); b.handler = function (b) { if (this === b.target || !(a.hotkeys.options.filterInputAcceptingElements && a.hotkeys.textInputTypes.test(b.target.nodeName) || a.hotkeys.options.filterContentEditable && a(b.target).attr("contenteditable") || a.hotkeys.options.filterTextInputs && a.inArray(b.target.type, a.hotkeys.textAcceptingInputTypes) > -1)) { var e = "keypress" !== b.type && a.hotkeys.specialKeys[b.which], f = String.fromCharCode(b.which).toLowerCase(), g = "", h = {}; a.each(["alt", "ctrl", "shift"], function (a, c) { b[c + "Key"] && e !== c && (g += c + "+") }), b.metaKey && !b.ctrlKey && "meta" !== e && (g += "meta+"), b.metaKey && "meta" !== e && g.indexOf("alt+ctrl+shift+") > -1 && (g = g.replace("alt+ctrl+shift+", "hyper+")), e ? h[g + e] = !0 : (h[g + f] = !0, h[g + a.hotkeys.shiftNums[f]] = !0, "shift+" === g && (h[a.hotkeys.shiftNums[f]] = !0)); for (var i = 0, j = d.length; j > i; i++) if (h[d[i]]) return c.apply(this, arguments) } } } } a.hotkeys = { version: "0.2.0", specialKeys: { 8: "backspace", 9: "tab", 10: "return", 13: "return", 16: "shift", 17: "ctrl", 18: "alt", 19: "pause", 20: "capslock", 27: "esc", 32: "space", 33: "pageup", 34: "pagedown", 35: "end", 36: "home", 37: "left", 38: "up", 39: "right", 40: "down", 45: "insert", 46: "del", 59: ";", 61: "=", 96: "0", 97: "1", 98: "2", 99: "3", 100: "4", 101: "5", 102: "6", 103: "7", 104: "8", 105: "9", 106: "*", 107: "+", 109: "-", 110: ".", 111: "/", 112: "f1", 113: "f2", 114: "f3", 115: "f4", 116: "f5", 117: "f6", 118: "f7", 119: "f8", 120: "f9", 121: "f10", 122: "f11", 123: "f12", 144: "numlock", 145: "scroll", 173: "-", 186: ";", 187: "=", 188: ",", 189: "-", 190: ".", 191: "/", 192: "`", 219: "[", 220: "\\", 221: "]", 222: "'" }, shiftNums: { "`": "~", 1: "!", 2: "@", 3: "#", 4: "$", 5: "%", 6: "^", 7: "&", 8: "*", 9: "(", 0: ")", "-": "_", "=": "+", ";": ": ", "'": '"', ",": "<", ".": ">", "/": "?", "\\": "|" }, textAcceptingInputTypes: ["text", "password", "number", "email", "url", "range", "date", "month", "week", "time", "datetime", "datetime-local", "search", "color", "tel"], textInputTypes: /textarea|input|select/i, options: { filterInputAcceptingElements: !0, filterTextInputs: !0, filterContentEditable: !0 } }, a.each(["keydown", "keyup", "keypress"], function () { a.event.special[this] = { add: b } }) }(jQuery || this.jQuery || window.jQuery);