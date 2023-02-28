/*!
 * Timepicker Component for Twitter Bootstrap
 *
 * Copyright 2013 Joris de Wit and bootstrap-timepicker contributors
 *
 * Contributors https://github.com/jdewit/bootstrap-timepicker/graphs/contributors
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */
!function (a, b, c) { "use strict"; var d = function (b, c) { this.widget = "", this.$element = a(b), this.defaultTime = c.defaultTime, this.disableFocus = c.disableFocus, this.disableMousewheel = c.disableMousewheel, this.isOpen = c.isOpen, this.minuteStep = c.minuteStep, this.modalBackdrop = c.modalBackdrop, this.orientation = c.orientation, this.secondStep = c.secondStep, this.snapToStep = c.snapToStep, this.showInputs = c.showInputs, this.showMeridian = c.showMeridian, this.showSeconds = c.showSeconds, this.template = c.template, this.appendWidgetTo = c.appendWidgetTo, this.showWidgetOnAddonClick = c.showWidgetOnAddonClick, this.icons = c.icons, this.maxHours = c.maxHours, this.explicitMode = c.explicitMode, this.handleDocumentClick = function (a) { var b = a.data.scope; b.$element.parent().find(a.target).length || b.$widget.is(a.target) || b.$widget.find(a.target).length || b.hideWidget() }, this._init() }; d.prototype = { constructor: d, _init: function () { var b = this; this.showWidgetOnAddonClick && this.$element.parent().hasClass("input-group") && this.$element.parent().hasClass("bootstrap-timepicker") ? (this.$element.parent(".input-group.bootstrap-timepicker").find(".input-group-addon").on({ "click.timepicker": a.proxy(this.showWidget, this) }), this.$element.on({ "focus.timepicker": a.proxy(this.highlightUnit, this), "click.timepicker": a.proxy(this.highlightUnit, this), "keydown.timepicker": a.proxy(this.elementKeydown, this), "blur.timepicker": a.proxy(this.blurElement, this), "mousewheel.timepicker DOMMouseScroll.timepicker": a.proxy(this.mousewheel, this) })) : this.template ? this.$element.on({ "focus.timepicker": a.proxy(this.showWidget, this), "click.timepicker": a.proxy(this.showWidget, this), "blur.timepicker": a.proxy(this.blurElement, this), "mousewheel.timepicker DOMMouseScroll.timepicker": a.proxy(this.mousewheel, this) }) : this.$element.on({ "focus.timepicker": a.proxy(this.highlightUnit, this), "click.timepicker": a.proxy(this.highlightUnit, this), "keydown.timepicker": a.proxy(this.elementKeydown, this), "blur.timepicker": a.proxy(this.blurElement, this), "mousewheel.timepicker DOMMouseScroll.timepicker": a.proxy(this.mousewheel, this) }), this.template !== !1 ? this.$widget = a(this.getTemplate()).on("click", a.proxy(this.widgetClick, this)) : this.$widget = !1, this.showInputs && this.$widget !== !1 && this.$widget.find("input").each(function () { a(this).on({ "click.timepicker": function () { a(this).select() }, "keydown.timepicker": a.proxy(b.widgetKeydown, b), "keyup.timepicker": a.proxy(b.widgetKeyup, b) }) }), this.setDefaultTime(this.defaultTime) }, blurElement: function () { this.highlightedUnit = null, this.updateFromElementVal() }, clear: function () { this.hour = "", this.minute = "", this.second = "", this.meridian = "", this.$element.val("") }, decrementHour: function () { if (this.showMeridian) if (1 === this.hour) this.hour = 12; else { if (12 === this.hour) return this.hour--, this.toggleMeridian(); if (0 === this.hour) return this.hour = 11, this.toggleMeridian(); this.hour-- } else this.hour <= 0 ? this.hour = this.maxHours - 1 : this.hour-- }, decrementMinute: function (a) { var b; b = a ? this.minute - a : this.minute - this.minuteStep, 0 > b ? (this.decrementHour(), this.minute = b + 60) : this.minute = b }, decrementSecond: function () { var a = this.second - this.secondStep; 0 > a ? (this.decrementMinute(!0), this.second = a + 60) : this.second = a }, elementKeydown: function (a) { switch (a.which) { case 9: if (a.shiftKey) { if ("hour" === this.highlightedUnit) { this.hideWidget(); break } this.highlightPrevUnit() } else { if (this.showMeridian && "meridian" === this.highlightedUnit || this.showSeconds && "second" === this.highlightedUnit || !this.showMeridian && !this.showSeconds && "minute" === this.highlightedUnit) { this.hideWidget(); break } this.highlightNextUnit() } a.preventDefault(), this.updateFromElementVal(); break; case 27: this.updateFromElementVal(); break; case 37: a.preventDefault(), this.highlightPrevUnit(), this.updateFromElementVal(); break; case 38: switch (a.preventDefault(), this.highlightedUnit) { case "hour": this.incrementHour(), this.highlightHour(); break; case "minute": this.incrementMinute(), this.highlightMinute(); break; case "second": this.incrementSecond(), this.highlightSecond(); break; case "meridian": this.toggleMeridian(), this.highlightMeridian() } this.update(); break; case 39: a.preventDefault(), this.highlightNextUnit(), this.updateFromElementVal(); break; case 40: switch (a.preventDefault(), this.highlightedUnit) { case "hour": this.decrementHour(), this.highlightHour(); break; case "minute": this.decrementMinute(), this.highlightMinute(); break; case "second": this.decrementSecond(), this.highlightSecond(); break; case "meridian": this.toggleMeridian(), this.highlightMeridian() } this.update() } }, getCursorPosition: function () { var a = this.$element.get(0); if ("selectionStart" in a) return a.selectionStart; if (c.selection) { a.focus(); var b = c.selection.createRange(), d = c.selection.createRange().text.length; return b.moveStart("character", -a.value.length), b.text.length - d } }, getTemplate: function () { var a, b, c, d, e, f; switch (this.showInputs ? (b = '<input type="text" class="bootstrap-timepicker-hour" maxlength="2"/>', c = '<input type="text" class="bootstrap-timepicker-minute" maxlength="2"/>', d = '<input type="text" class="bootstrap-timepicker-second" maxlength="2"/>', e = '<input type="text" class="bootstrap-timepicker-meridian" maxlength="2"/>') : (b = '<span class="bootstrap-timepicker-hour"></span>', c = '<span class="bootstrap-timepicker-minute"></span>', d = '<span class="bootstrap-timepicker-second"></span>', e = '<span class="bootstrap-timepicker-meridian"></span>'), f = '<table><tr><td><a href="#" data-action="incrementHour"><span class="' + this.icons.up + '"></span></a></td><td class="separator">&nbsp;</td><td><a href="#" data-action="incrementMinute"><span class="' + this.icons.up + '"></span></a></td>' + (this.showSeconds ? '<td class="separator">&nbsp;</td><td><a href="#" data-action="incrementSecond"><span class="' + this.icons.up + '"></span></a></td>' : "") + (this.showMeridian ? '<td class="separator">&nbsp;</td><td class="meridian-column"><a href="#" data-action="toggleMeridian"><span class="' + this.icons.up + '"></span></a></td>' : "") + "</tr><tr><td>" + b + '</td> <td class="separator">:</td><td>' + c + "</td> " + (this.showSeconds ? '<td class="separator">:</td><td>' + d + "</td>" : "") + (this.showMeridian ? '<td class="separator">&nbsp;</td><td>' + e + "</td>" : "") + '</tr><tr><td><a href="#" data-action="decrementHour"><span class="' + this.icons.down + '"></span></a></td><td class="separator"></td><td><a href="#" data-action="decrementMinute"><span class="' + this.icons.down + '"></span></a></td>' + (this.showSeconds ? '<td class="separator">&nbsp;</td><td><a href="#" data-action="decrementSecond"><span class="' + this.icons.down + '"></span></a></td>' : "") + (this.showMeridian ? '<td class="separator">&nbsp;</td><td><a href="#" data-action="toggleMeridian"><span class="' + this.icons.down + '"></span></a></td>' : "") + "</tr></table>", this.template) { case "modal": a = '<div class="bootstrap-timepicker-widget modal hide fade in" data-backdrop="' + (this.modalBackdrop ? "true" : "false") + '"><div class="modal-header"><a href="#" class="close" data-dismiss="modal">&times;</a><h3>Pick a Time</h3></div><div class="modal-content">' + f + '</div><div class="modal-footer"><a href="#" class="btn btn-primary" data-dismiss="modal">OK</a></div></div>'; break; case "dropdown": a = '<div class="bootstrap-timepicker-widget dropdown-menu">' + f + "</div>" } return a }, getTime: function () { return "" === this.hour ? "" : this.hour + ":" + (1 === this.minute.toString().length ? "0" + this.minute : this.minute) + (this.showSeconds ? ":" + (1 === this.second.toString().length ? "0" + this.second : this.second) : "") + (this.showMeridian ? " " + this.meridian : "") }, hideWidget: function () { this.isOpen !== !1 && (this.$element.trigger({ type: "hide.timepicker", time: { value: this.getTime(), hours: this.hour, minutes: this.minute, seconds: this.second, meridian: this.meridian } }), "modal" === this.template && this.$widget.modal ? this.$widget.modal("hide") : this.$widget.removeClass("open"), a(c).off("mousedown.timepicker, touchend.timepicker", this.handleDocumentClick), this.isOpen = !1, this.$widget.detach()) }, highlightUnit: function () { this.position = this.getCursorPosition(), this.position >= 0 && this.position <= 2 ? this.highlightHour() : this.position >= 3 && this.position <= 5 ? this.highlightMinute() : this.position >= 6 && this.position <= 8 ? this.showSeconds ? this.highlightSecond() : this.highlightMeridian() : this.position >= 9 && this.position <= 11 && this.highlightMeridian() }, highlightNextUnit: function () { switch (this.highlightedUnit) { case "hour": this.highlightMinute(); break; case "minute": this.showSeconds ? this.highlightSecond() : this.showMeridian ? this.highlightMeridian() : this.highlightHour(); break; case "second": this.showMeridian ? this.highlightMeridian() : this.highlightHour(); break; case "meridian": this.highlightHour() } }, highlightPrevUnit: function () { switch (this.highlightedUnit) { case "hour": this.showMeridian ? this.highlightMeridian() : this.showSeconds ? this.highlightSecond() : this.highlightMinute(); break; case "minute": this.highlightHour(); break; case "second": this.highlightMinute(); break; case "meridian": this.showSeconds ? this.highlightSecond() : this.highlightMinute() } }, highlightHour: function () { var a = this.$element.get(0), b = this; this.highlightedUnit = "hour", a.setSelectionRange && setTimeout(function () { b.hour < 10 ? a.setSelectionRange(0, 1) : a.setSelectionRange(0, 2) }, 0) }, highlightMinute: function () { var a = this.$element.get(0), b = this; this.highlightedUnit = "minute", a.setSelectionRange && setTimeout(function () { b.hour < 10 ? a.setSelectionRange(2, 4) : a.setSelectionRange(3, 5) }, 0) }, highlightSecond: function () { var a = this.$element.get(0), b = this; this.highlightedUnit = "second", a.setSelectionRange && setTimeout(function () { b.hour < 10 ? a.setSelectionRange(5, 7) : a.setSelectionRange(6, 8) }, 0) }, highlightMeridian: function () { var a = this.$element.get(0), b = this; this.highlightedUnit = "meridian", a.setSelectionRange && (this.showSeconds ? setTimeout(function () { b.hour < 10 ? a.setSelectionRange(8, 10) : a.setSelectionRange(9, 11) }, 0) : setTimeout(function () { b.hour < 10 ? a.setSelectionRange(5, 7) : a.setSelectionRange(6, 8) }, 0)) }, incrementHour: function () { if (this.showMeridian) { if (11 === this.hour) return this.hour++, this.toggleMeridian(); 12 === this.hour && (this.hour = 0) } return this.hour === this.maxHours - 1 ? void (this.hour = 0) : void this.hour++ }, incrementMinute: function (a) { var b; b = a ? this.minute + a : this.minute + this.minuteStep - this.minute % this.minuteStep, b > 59 ? (this.incrementHour(), this.minute = b - 60) : this.minute = b }, incrementSecond: function () { var a = this.second + this.secondStep - this.second % this.secondStep; a > 59 ? (this.incrementMinute(!0), this.second = a - 60) : this.second = a }, mousewheel: function (b) { if (!this.disableMousewheel) { b.preventDefault(), b.stopPropagation(); var c = b.originalEvent.wheelDelta || -b.originalEvent.detail, d = null; switch ("mousewheel" === b.type ? d = -1 * b.originalEvent.wheelDelta : "DOMMouseScroll" === b.type && (d = 40 * b.originalEvent.detail), d && (b.preventDefault(), a(this).scrollTop(d + a(this).scrollTop())), this.highlightedUnit) { case "minute": c > 0 ? this.incrementMinute() : this.decrementMinute(), this.highlightMinute(); break; case "second": c > 0 ? this.incrementSecond() : this.decrementSecond(), this.highlightSecond(); break; case "meridian": this.toggleMeridian(), this.highlightMeridian(); break; default: c > 0 ? this.incrementHour() : this.decrementHour(), this.highlightHour() } return !1 } }, changeToNearestStep: function (a, b) { return a % b === 0 ? a : Math.round(a % b / b) ? (a + (b - a % b)) % 60 : a - a % b }, place: function () { if (!this.isInline) { var c = this.$widget.outerWidth(), d = this.$widget.outerHeight(), e = 10, f = a(b).width(), g = a(b).height(), h = a(b).scrollTop(), i = parseInt(this.$element.parents().filter(function () { return "auto" !== a(this).css("z-index") }).first().css("z-index"), 10) + 10, j = this.component ? this.component.parent().offset() : this.$element.offset(), k = this.component ? this.component.outerHeight(!0) : this.$element.outerHeight(!1), l = this.component ? this.component.outerWidth(!0) : this.$element.outerWidth(!1), m = j.left, n = j.top; this.$widget.removeClass("timepicker-orient-top timepicker-orient-bottom timepicker-orient-right timepicker-orient-left"), "auto" !== this.orientation.x ? (this.$widget.addClass("timepicker-orient-" + this.orientation.x), "right" === this.orientation.x && (m -= c - l)) : (this.$widget.addClass("timepicker-orient-left"), j.left < 0 ? m -= j.left - e : j.left + c > f && (m = f - c - e)); var o, p, q = this.orientation.y; "auto" === q && (o = -h + j.top - d, p = h + g - (j.top + k + d), q = Math.max(o, p) === p ? "top" : "bottom"), this.$widget.addClass("timepicker-orient-" + q), "top" === q ? n += k : n -= d + parseInt(this.$widget.css("padding-top"), 10), this.$widget.css({ top: n, left: m, zIndex: i }) } }, remove: function () { a("document").off(".timepicker"), this.$widget && this.$widget.remove(), delete this.$element.data().timepicker }, setDefaultTime: function (a) { if (this.$element.val()) this.updateFromElementVal(); else if ("current" === a) { var b = new Date, c = b.getHours(), d = b.getMinutes(), e = b.getSeconds(), f = "AM"; 0 !== e && (e = Math.ceil(b.getSeconds() / this.secondStep) * this.secondStep, 60 === e && (d += 1, e = 0)), 0 !== d && (d = Math.ceil(b.getMinutes() / this.minuteStep) * this.minuteStep, 60 === d && (c += 1, d = 0)), this.showMeridian && (0 === c ? c = 12 : c >= 12 ? (c > 12 && (c -= 12), f = "PM") : f = "AM"), this.hour = c, this.minute = d, this.second = e, this.meridian = f, this.update() } else a === !1 ? (this.hour = 0, this.minute = 0, this.second = 0, this.meridian = "AM") : this.setTime(a) }, setTime: function (a, b) { if (!a) return void this.clear(); var c, d, e, f, g, h; if ("object" == typeof a && a.getMonth) e = a.getHours(), f = a.getMinutes(), g = a.getSeconds(), this.showMeridian && (h = "AM", e > 12 && (h = "PM", e %= 12), 12 === e && (h = "PM")); else { if (c = (/a/i.test(a) ? 1 : 0) + (/p/i.test(a) ? 2 : 0), c > 2) return void this.clear(); if (d = a.replace(/[^0-9\:]/g, "").split(":"), e = d[0] ? d[0].toString() : d.toString(), this.explicitMode && e.length > 2 && e.length % 2 !== 0) return void this.clear(); f = d[1] ? d[1].toString() : "", g = d[2] ? d[2].toString() : "", e.length > 4 && (g = e.slice(-2), e = e.slice(0, -2)), e.length > 2 && (f = e.slice(-2), e = e.slice(0, -2)), f.length > 2 && (g = f.slice(-2), f = f.slice(0, -2)), e = parseInt(e, 10), f = parseInt(f, 10), g = parseInt(g, 10), isNaN(e) && (e = 0), isNaN(f) && (f = 0), isNaN(g) && (g = 0), g > 59 && (g = 59), f > 59 && (f = 59), e >= this.maxHours && (e = this.maxHours - 1), this.showMeridian ? (e > 12 && (c = 2, e -= 12), c || (c = 1), 0 === e && (e = 12), h = 1 === c ? "AM" : "PM") : 12 > e && 2 === c ? e += 12 : e >= this.maxHours ? e = this.maxHours - 1 : (0 > e || 12 === e && 1 === c) && (e = 0) } this.hour = e, this.snapToStep ? (this.minute = this.changeToNearestStep(f, this.minuteStep), this.second = this.changeToNearestStep(g, this.secondStep)) : (this.minute = f, this.second = g), this.meridian = h, this.update(b) }, showWidget: function () { this.isOpen || this.$element.is(":disabled") || (this.$widget.appendTo(this.appendWidgetTo), a(c).on("mousedown.timepicker, touchend.timepicker", { scope: this }, this.handleDocumentClick), this.$element.trigger({ type: "show.timepicker", time: { value: this.getTime(), hours: this.hour, minutes: this.minute, seconds: this.second, meridian: this.meridian } }), this.place(), this.disableFocus && this.$element.blur(), "" === this.hour && (this.defaultTime ? this.setDefaultTime(this.defaultTime) : this.setTime("0:0:0")), "modal" === this.template && this.$widget.modal ? this.$widget.modal("show").on("hidden", a.proxy(this.hideWidget, this)) : this.isOpen === !1 && this.$widget.addClass("open"), this.isOpen = !0) }, toggleMeridian: function () { this.meridian = "AM" === this.meridian ? "PM" : "AM" }, update: function (a) { this.updateElement(), a || this.updateWidget(), this.$element.trigger({ type: "changeTime.timepicker", time: { value: this.getTime(), hours: this.hour, minutes: this.minute, seconds: this.second, meridian: this.meridian } }) }, updateElement: function () { this.$element.val(this.getTime()).change() }, updateFromElementVal: function () { this.setTime(this.$element.val()) }, updateWidget: function () { if (this.$widget !== !1) { var a = this.hour, b = 1 === this.minute.toString().length ? "0" + this.minute : this.minute, c = 1 === this.second.toString().length ? "0" + this.second : this.second; this.showInputs ? (this.$widget.find("input.bootstrap-timepicker-hour").val(a), this.$widget.find("input.bootstrap-timepicker-minute").val(b), this.showSeconds && this.$widget.find("input.bootstrap-timepicker-second").val(c), this.showMeridian && this.$widget.find("input.bootstrap-timepicker-meridian").val(this.meridian)) : (this.$widget.find("span.bootstrap-timepicker-hour").text(a), this.$widget.find("span.bootstrap-timepicker-minute").text(b), this.showSeconds && this.$widget.find("span.bootstrap-timepicker-second").text(c), this.showMeridian && this.$widget.find("span.bootstrap-timepicker-meridian").text(this.meridian)) } }, updateFromWidgetInputs: function () { if (this.$widget !== !1) { var a = this.$widget.find("input.bootstrap-timepicker-hour").val() + ":" + this.$widget.find("input.bootstrap-timepicker-minute").val() + (this.showSeconds ? ":" + this.$widget.find("input.bootstrap-timepicker-second").val() : "") + (this.showMeridian ? this.$widget.find("input.bootstrap-timepicker-meridian").val() : ""); this.setTime(a, !0) } }, widgetClick: function (b) { b.stopPropagation(), b.preventDefault(); var c = a(b.target), d = c.closest("a").data("action"); d && this[d](), this.update(), c.is("input") && c.get(0).setSelectionRange(0, 2) }, widgetKeydown: function (b) { var c = a(b.target), d = c.attr("class").replace("bootstrap-timepicker-", ""); switch (b.which) { case 9: if (b.shiftKey) { if ("hour" === d) return this.hideWidget() } else if (this.showMeridian && "meridian" === d || this.showSeconds && "second" === d || !this.showMeridian && !this.showSeconds && "minute" === d) return this.hideWidget(); break; case 27: this.hideWidget(); break; case 38: switch (b.preventDefault(), d) { case "hour": this.incrementHour(); break; case "minute": this.incrementMinute(); break; case "second": this.incrementSecond(); break; case "meridian": this.toggleMeridian() } this.setTime(this.getTime()), c.get(0).setSelectionRange(0, 2); break; case 40: switch (b.preventDefault(), d) { case "hour": this.decrementHour(); break; case "minute": this.decrementMinute(); break; case "second": this.decrementSecond(); break; case "meridian": this.toggleMeridian() } this.setTime(this.getTime()), c.get(0).setSelectionRange(0, 2) } }, widgetKeyup: function (a) { (65 === a.which || 77 === a.which || 80 === a.which || 46 === a.which || 8 === a.which || a.which >= 48 && a.which <= 57 || a.which >= 96 && a.which <= 105) && this.updateFromWidgetInputs() } }, a.fn.timepicker = function (b) { var c = Array.apply(null, arguments); return c.shift(), this.each(function () { var e = a(this), f = e.data("timepicker"), g = "object" == typeof b && b; f || e.data("timepicker", f = new d(this, a.extend({}, a.fn.timepicker.defaults, g, a(this).data()))), "string" == typeof b && f[b].apply(f, c) }) }, a.fn.timepicker.defaults = { defaultTime: "current", disableFocus: !1, disableMousewheel: !1, isOpen: !1, minuteStep: 15, modalBackdrop: !1, orientation: { x: "auto", y: "auto" }, secondStep: 15, snapToStep: !1, showSeconds: !1, showInputs: !0, showMeridian: !0, template: "dropdown", appendWidgetTo: "body", showWidgetOnAddonClick: !0, icons: { up: "glyphicon glyphicon-chevron-up", down: "glyphicon glyphicon-chevron-down" }, maxHours: 24, explicitMode: !1 }, a.fn.timepicker.Constructor = d, a(c).on("focus.timepicker.data-api click.timepicker.data-api", '[data-provide="timepicker"]', function (b) { var c = a(this); c.data("timepicker") || (b.preventDefault(), c.timepicker()) }) }(jQuery, window, document);