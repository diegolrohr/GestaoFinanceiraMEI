! function(e) {
    function t(t) {
        for (var a, s, r = t[0], c = t[1], l = t[2], u = 0, p = []; u < r.length; u++) s = r[u], n[s] && p.push(n[s][0]), n[s] = 0;
        for (a in c) Object.prototype.hasOwnProperty.call(c, a) && (e[a] = c[a]);
        for (d && d(t); p.length;) p.shift()();
        return o.push.apply(o, l || []), i()
    }

    function i() {
        for (var e, t = 0; t < o.length; t++) {
            for (var i = o[t], a = !0, r = 1; r < i.length; r++) {
                var c = i[r];
                0 !== n[c] && (a = !1)
            }
            a && (o.splice(t--, 1), e = s(s.s = i[0]))
        }
        return e
    }
    var a = {},
        n = {
            0: 0
        },
        o = [];

    function s(t) {
        if (a[t]) return a[t].exports;
        var i = a[t] = {
            i: t,
            l: !1,
            exports: {}
        };
        return e[t].call(i.exports, i, i.exports, s), i.l = !0, i.exports
    }
    s.m = e, s.c = a, s.d = function(e, t, i) {
        s.o(e, t) || Object.defineProperty(e, t, {
            enumerable: !0,
            get: i
        })
    }, s.r = function(e) {
        "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, {
            value: "Module"
        }), Object.defineProperty(e, "__esModule", {
            value: !0
        })
    }, s.t = function(e, t) {
        if (1 & t && (e = s(e)), 8 & t) return e;
        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
        var i = Object.create(null);
        if (s.r(i), Object.defineProperty(i, "default", {
                enumerable: !0,
                value: e
            }), 2 & t && "string" != typeof e)
            for (var a in e) s.d(i, a, function(t) {
                return e[t]
            }.bind(null, a));
        return i
    }, s.n = function(e) {
        var t = e && e.__esModule ? function() {
            return e.default
        } : function() {
            return e
        };
        return s.d(t, "a", t), t
    }, s.o = function(e, t) {
        return Object.prototype.hasOwnProperty.call(e, t)
    }, s.p = "";
    var r = window.webpackJsonp = window.webpackJsonp || [],
        c = r.push.bind(r);
    r.push = t, r = r.slice();
    for (var l = 0; l < r.length; l++) t(r[l]);
    var d = c;
    o.push([233, 1]), i()
}([function(module, __webpack_exports__, __webpack_require__) {
    "use strict";
    (function($) {
        __webpack_require__.d(__webpack_exports__, "e", function() {
            return initBaseTags
        }), __webpack_require__.d(__webpack_exports__, "i", function() {
            return makeId
        }), __webpack_require__.d(__webpack_exports__, "b", function() {
            return createElem
        }), __webpack_require__.d(__webpack_exports__, "c", function() {
            return fillForm
        }), __webpack_require__.d(__webpack_exports__, "l", function() {
            return processSidebar
        }), __webpack_require__.d(__webpack_exports__, "h", function() {
            return loadScriptFn
        }), __webpack_require__.d(__webpack_exports__, "q", function() {
            return updateHelpers
        }), __webpack_require__.d(__webpack_exports__, "o", function() {
            return submitErrorHandler
        }), __webpack_require__.d(__webpack_exports__, "j", function() {
            return maskFields
        }), __webpack_require__.d(__webpack_exports__, "r", function() {
            return updateTextFields
        }), __webpack_require__.d(__webpack_exports__, "f", function() {
            return loadFormScripts
        }), __webpack_require__.d(__webpack_exports__, "p", function() {
            return updateActiveMenu
        }), __webpack_require__.d(__webpack_exports__, "g", function() {
            return loadScript
        }), __webpack_require__.d(__webpack_exports__, "k", function() {
            return processAppConfig
        }), __webpack_require__.d(__webpack_exports__, "m", function() {
            return refresh
        }), __webpack_require__.d(__webpack_exports__, "n", function() {
            return setCookie
        }), __webpack_require__.d(__webpack_exports__, "d", function() {
            return getCookie
        }), __webpack_require__.d(__webpack_exports__, "a", function() {
            return cookie2Object
        });
        var jquery_ui_ui_effect__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(117),
            jquery_ui_ui_effect__WEBPACK_IMPORTED_MODULE_0___default = __webpack_require__.n(jquery_ui_ui_effect__WEBPACK_IMPORTED_MODULE_0__),
            jquery_ui_ui_widget__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(18),
            jquery_ui_ui_widget__WEBPACK_IMPORTED_MODULE_1___default = __webpack_require__.n(jquery_ui_ui_widget__WEBPACK_IMPORTED_MODULE_1__),
            jquery_ui_ui_widgets_autocomplete__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(118),
            jquery_ui_ui_widgets_autocomplete__WEBPACK_IMPORTED_MODULE_2___default = __webpack_require__.n(jquery_ui_ui_widgets_autocomplete__WEBPACK_IMPORTED_MODULE_2__),
            jquery_ui_ui_widgets_draggable__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(121),
            jquery_ui_ui_widgets_draggable__WEBPACK_IMPORTED_MODULE_3___default = __webpack_require__.n(jquery_ui_ui_widgets_draggable__WEBPACK_IMPORTED_MODULE_3__),
            jquery_ui_ui_widgets_resizable__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(126),
            jquery_ui_ui_widgets_resizable__WEBPACK_IMPORTED_MODULE_4___default = __webpack_require__.n(jquery_ui_ui_widgets_resizable__WEBPACK_IMPORTED_MODULE_4__),
            inputmask_dist_inputmask_jquery_inputmask__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(128),
            inputmask_dist_inputmask_jquery_inputmask__WEBPACK_IMPORTED_MODULE_5___default = __webpack_require__.n(inputmask_dist_inputmask_jquery_inputmask__WEBPACK_IMPORTED_MODULE_5__),
            inputmask_dist_inputmask_inputmask_date_extensions__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(130),
            inputmask_dist_inputmask_inputmask_date_extensions__WEBPACK_IMPORTED_MODULE_6___default = __webpack_require__.n(inputmask_dist_inputmask_inputmask_date_extensions__WEBPACK_IMPORTED_MODULE_6__),
            inputmask_dist_inputmask_inputmask_extensions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(131),
            inputmask_dist_inputmask_inputmask_extensions__WEBPACK_IMPORTED_MODULE_7___default = __webpack_require__.n(inputmask_dist_inputmask_inputmask_extensions__WEBPACK_IMPORTED_MODULE_7__),
            inputmask_dist_inputmask_inputmask_numeric_extensions__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(132),
            inputmask_dist_inputmask_inputmask_numeric_extensions__WEBPACK_IMPORTED_MODULE_8___default = __webpack_require__.n(inputmask_dist_inputmask_inputmask_numeric_extensions__WEBPACK_IMPORTED_MODULE_8__),
            _legacy_picker_date__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(133),
            _legacy_picker_date__WEBPACK_IMPORTED_MODULE_9___default = __webpack_require__.n(_legacy_picker_date__WEBPACK_IMPORTED_MODULE_9__),
            _legacy_picker_time__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(134),
            _legacy_picker_time__WEBPACK_IMPORTED_MODULE_10___default = __webpack_require__.n(_legacy_picker_time__WEBPACK_IMPORTED_MODULE_10__),
            _legacy_picker_month__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(135),
            _legacy_picker_month__WEBPACK_IMPORTED_MODULE_11___default = __webpack_require__.n(_legacy_picker_month__WEBPACK_IMPORTED_MODULE_11__),
            _legacy_tooltip__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(136),
            _legacy_validation__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(35),
            _legacy_passwords__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(138),
            _legacy_passwords__WEBPACK_IMPORTED_MODULE_14___default = __webpack_require__.n(_legacy_passwords__WEBPACK_IMPORTED_MODULE_14__),
            _loading__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(2),
            prismjs__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(43),
            prismjs__WEBPACK_IMPORTED_MODULE_16___default = __webpack_require__.n(prismjs__WEBPACK_IMPORTED_MODULE_16__),
            prismjs_components_prism_json__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(139),
            prismjs_components_prism_json__WEBPACK_IMPORTED_MODULE_17___default = __webpack_require__.n(prismjs_components_prism_json__WEBPACK_IMPORTED_MODULE_17__),
            prismjs_components_prism_javascript__WEBPACK_IMPORTED_MODULE_18__ = __webpack_require__(140),
            prismjs_components_prism_javascript__WEBPACK_IMPORTED_MODULE_18___default = __webpack_require__.n(prismjs_components_prism_javascript__WEBPACK_IMPORTED_MODULE_18__),
            prismjs_components_prism_css__WEBPACK_IMPORTED_MODULE_19__ = __webpack_require__(141),
            prismjs_components_prism_css__WEBPACK_IMPORTED_MODULE_19___default = __webpack_require__.n(prismjs_components_prism_css__WEBPACK_IMPORTED_MODULE_19__),
            prismjs_components_prism_markdown__WEBPACK_IMPORTED_MODULE_20__ = __webpack_require__(142),
            prismjs_components_prism_markdown__WEBPACK_IMPORTED_MODULE_20___default = __webpack_require__.n(prismjs_components_prism_markdown__WEBPACK_IMPORTED_MODULE_20__),
            prismjs_components_prism_clike__WEBPACK_IMPORTED_MODULE_21__ = __webpack_require__(143),
            prismjs_components_prism_clike__WEBPACK_IMPORTED_MODULE_21___default = __webpack_require__.n(prismjs_components_prism_clike__WEBPACK_IMPORTED_MODULE_21__),
            _toast__WEBPACK_IMPORTED_MODULE_22__ = __webpack_require__(3),
            _modal__WEBPACK_IMPORTED_MODULE_23__ = __webpack_require__(17),
            _navigation__WEBPACK_IMPORTED_MODULE_24__ = __webpack_require__(31),
            _ = __webpack_require__(14),
            setValueToElement = function(e, t) {
                if (e.is("pre")) {
                    var i = document.createElement("code");
                    e.textContent = "", i.textContent = t, e.append(i)
                } else if (e.is("select")) $("option", e).each(function() {
                    this.value === t && (this.selected = !0)
                });
                else if (e.is("textarea")) e.val(t);
                else if (e.attr("type")) switch (e.attr("type").toLowerCase()) {
                    case "checkbox":
                        t ? e.attr("checked", "checked") : e.removeAttr("checked");
                        break;
                    case "file":
                        document.getElementById(e.attr("id") + "Icon").style.display = t ? "none" : "block", t ? document.getElementById(e.attr("id") + "Thumb").src = t : document.getElementById(e.attr("id") + "Thumb").removeAttribute("src");
                        break;
                    default:
                        (e.hasClass("float") || e.hasClass("currency")) && "number" == typeof t && (t = t.toLocaleString("pt-BR", {
                            useGrouping: !1
                        })), e.val(t)
                }
                e.change()
            },
            initBaseTags = function() {
                0 == $("html[lang]").length && $("html").attr("lang", "pt-br"), 0 == $("meta[name=description]").length && $("head").append(createElem("meta", {
                    name: "description",
                    content: "O Bemacash é a solução de gestão que cabe no seu bolso"
                })), 0 == $("meta[name=viewport]").length && $("head").append(createElem("meta", {
                    name: "viewport",
                    content: "width=device-width, initial-scale=1"
                })), 0 == $("meta[charset=utf-8]").length && $("head").append(createElem("meta", {
                    charset: "utf-8"
                })), 0 == $("meta[http-equiv]").length && $("head").append(createElem("meta", {
                    "http-equiv": "Content-Language",
                    content: "pt-br"
                })), 0 == $("body footer").length && $("body").prepend(createElem("footer")), 0 == $("body main").length && $("body").prepend(createElem("main")), 0 == $("body aside").length && $("body").prepend(createElem("aside")), 0 == $("body header").length && $("body").prepend(createElem("header"))
            },
            makeId = function(e, t) {
                t = t || 5;
                for (var i = e || "", a = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", n = 0; n < t; n++) i += a.charAt(Math.floor(Math.random() * a.length));
                return i
            },
            createElem = function(e, t, i, a, n) {
                if (void 0 === e) return !1;
                var o;
                if (void 0 === i && (i = ""), o = void 0 === a ? document.createElement(e) : document.createElementNS(a, e), null !== t && "object" == typeof t)
                    for (var s in t) t.hasOwnProperty(s) && o.setAttribute(s, t[s]);
                if (null !== n && "object" == typeof n)
                    for (var r in n) n.hasOwnProperty(r) && o.setAttributeNS(null, r, n[r]);
                if (Array.isArray(i) || (i = [i]), null !== i)
                    for (var c = 0; c < i.length; c++) null !== i[c] && ("string" != typeof i[c] ? o.appendChild(i[c]) : o.appendChild(document.createTextNode(i[c])));
                return o
            },
            fillForm = function(formElem, data, callback) {
                if (formElem.find("input:text, input:password, input:file, select, textarea").val(""), formElem.find("input:checkbox").removeAttr("checked").removeAttr("selected"), $.each(data, function(e, t) {
                        if ("object" == typeof t) $.each(t, function(t, i) {
                            var a = formElem.find("[name=" + e + t.charAt(0).toUpperCase() + t.slice(1) + "]");
                            a && setValueToElement(a, i)
                        });
                        else {
                            var i = formElem.find("[name=" + e + "]");
                            i && setValueToElement(i, t)
                        }
                    }), callback && /^[a-zA-Z]+$/.test(callback)) {
                    var cb = eval("(" + callback + ")");
                    "function" == typeof cb && cb(data)
                }
            },
            processSidebar = function(e, t, i) {
                !document.getElementsByClassName("mat-sidenav").length && 0 == $("#nav-mobile").length || i ? Object(_navigation__WEBPACK_IMPORTED_MODULE_24__.a)(e, t) : updateActiveMenu(t)
            },
            loadScriptFn = function(e, t, i) {
                if (e)
                    if (0 == $("dynamicScripts").length && $("footer").append(createElem("dynamicScripts")), e.endsWith(".js")) 0 == $("script[src='" + e + "']").length && loadScript(e, i, "dynamicScripts", !1);
                    else if (t) {
                    var a = [];
                    t.map(function(e) {
                        -1 === $.inArray(e, a) && a.push(e)
                    }), a.length > 0 && loadScript(e + a, i, "dynamicScripts", !1)
                }
            },
            updateHelpers = function(e, t) {
                e.map(function(e) {
                    var t, i = {
                        class: "material-icons helper "
                    };
                    e.video && (t = "ondemand_video", i.class += "video", e.video.yt && (i["data-yt"] = e.video.yt)), e.tooltip && (t = "help_outline", i.class += "tooltip", i["data-tooltip"] = e.tooltip.text, i["data-position"] = e.tooltip.position || "bottom"), $("#" + e.id).parent().prepend(createElem("i", i, t))
                }), $(".helper.tooltip").tooltip();
                var i = $(".helper.video");
                if (i.length > 0) {
                    if (0 == $("#videoPlayer").length) {
                        $("body").append(createElem("div", {
                            id: "videoPlayer_wrapper"
                        }, [createElem("div", null, [createElem("i", {
                            class: "material-icons video-close"
                        }, "close")]), createElem("div", {
                            id: "videoPlayer"
                        })]));
                        var a = document.createElement("script");
                        a.src = "https://www.youtube.com/iframe_api";
                        var n = document.getElementsByTagName("script")[0];
                        n.parentNode.insertBefore(a, n)
                    }
                    i.click(function() {
                        t && t.destroy(), "" != $(this).data("yt") && ($("#videoPlayer").css("display", "block"), $("#videoPlayer_wrapper").css("display", "block"), t = new window.YT.Player("videoPlayer", {
                            height: "270",
                            width: "480",
                            videoId: $(this).data("yt"),
                            events: {
                                onReady: function(e) {
                                    e.target.playVideo()
                                }
                            },
                            playerVars: {
                                disablekb: 0,
                                showinfo: 0
                            }
                        }), $("#videoPlayer_wrapper").position({
                            my: "right top+24",
                            of: event,
                            collision: "fit"
                        }))
                    }), $("#videoPlayer_wrapper").draggable({
                        iframeFix: !0
                    }), $(document).on("click.video-close", ".video-close", function() {
                        t.stopVideo(), $("#videoPlayer_wrapper").css("display", "none")
                    })
                }
            },
            submitErrorHandler = function(e, t) {
                try {
                    var i = JSON.parse(t);
                    if (e) {
                        var a = {};
                        i.innerMessage.filter(function(e) {
                            return "" != (e.dataField || "")
                        }).forEach(function(e) {
                            $("#" + e.dataField).length > 0 ? a[e.dataField] = e.message : Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(e.message)
                        }), e.showErrors(a), i.innerMessage.filter(function(e) {
                            return "" == (e.dataField || "")
                        }).forEach(function(e) {
                            Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(e.message)
                        })
                    } else i.innerMessage.forEach(function(e) {
                        Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(e.message)
                    })
                } catch (e) {
                    Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(t)
                }
            },
            maskFields = function() {
                var e = this,
                    t = {
                        autoUnmask: !0,
                        placeholder: " ",
                        showMaskOnHover: !1,
                        jitMasking: !0
                    };
                $("input.mask").each(function(i, a) {
                    if ($(a).hasClass("cep")) $(a).inputmask($.extend({}, t, {
                        mask: "99999-999"
                    }));
                    else if ($(a).hasClass("tel")) $(a).inputmask($.extend({}, t, {
                        mask: ["(99) 9999-9999", "(99) 99999-9999"]
                    }));
                    else if ($(a).hasClass("currency")) $(a).inputmask({
                        alias: "currency",
                        radixPoint: ",",
                        groupSeparator: ".",
                        placeholder: "0",
                        digits: $(e).data("inputmask-digits") || 2,
                        clearMaskOnLostFocus: !0,
                        showMaskOnHover: !1,
                        digitsOptional: !1,
                        autoUnmask: !0,
                        allowMinus: $(e).data("inputmask-allowMinus") || !1
                    });
                    else if ($(a).hasClass("duration")) $(a).inputmask({
                        regex: "\\d+:[0-5][0-9]",
                        placeholder: "0"
                    });
                    else if ($(a).hasClass("float")) $(a).inputmask({
                        alias: "numeric",
                        radixPoint: ",",
                        groupSeparator: ".",
                        autoGroup: !0,
                        groupSize: 3,
                        placeholder: "0",
                        digits: $(e).data("inputmask-digits") || 2,
                        autoUnmask: !0,
                        clearMaskOnLostFocus: !0,
                        showMaskOnHover: !1,
                        digitsOptional: !1,
                        allowMinus: $(e).data("inputmask-allowMinus") || !1
                    });
                    else if ($(a).hasClass("cpfcnpj") || $(a).hasClass("cnpj") || $(a).hasClass("cpf")) {
                        var n = $.extend({}, t);
                        $(a).hasClass("cpfcnpj") ? n.mask = ["999.999.999-99", "99.999.999/9999-99"] : $(a).hasClass("cpf") ? n.mask = "999.999.999-99" : $(a).hasClass("cnpj") && (n.mask = "99.999.999/9999-99"), $(a).inputmask(n)
                    } else $(a).hasClass("numbers") ? $(a).inputmask({
                        alias: "numeric",
                        showMaskOnHover: !1
                    }) : $(a).hasClass("date") ? $(a).inputmask({
                        alias: "datetime",
                        inputFormat: "dd/mm/yyyy",
                        showMaskOnHover: !1,
                        jitMasking: !0,
                        clearIncomplete: !0
                    }) : $(a).inputmask();
                    $(a).removeClass("mask"), $(a).addClass("masked")
                })
            },
            updateTextFields = function(e) {
                var t = $((e = e ? "#" + e + " " : "") + "input[type=text]," + e + "input[type=password]," + e + "input[type=email]," + e + "input[type=url]," + e + "input[type=tel]," + e + "input[type=number]," + e + "input[type=search]," + e + "textarea");
                t.length > 0 && (t.each(function(e, t) {
                    var i = $(t);
                    i.hasClass("select-dropdown") && (i = $(t).parent()), $(t).val().length > 0 || $(t).is(":focus") || t.autofocus || void 0 !== $(t).attr("placeholder") ? i.siblings("label").addClass("active") : $(t)[0].validity ? i.siblings("label").toggleClass("active", !0 === $(t)[0].validity.badInput) : i.siblings("label").removeClass("active")
                }), $(e + "textarea").trigger("autoresize"))
            },
            loadFormScripts = function(config) {
                if (config.elements) {
                    var validateItems = config.elements.filter(function(e) {
                        return e.required || e.minLength || e.maxLength
                    });
                    $("#" + config.id).validate({
                        rules: validateItems.reduce(function(e, t) {
                            var i = {};
                            return t.required && (i.required = t.required), t.minLength && (i.minlength = t.minLength), t.maxLength && (i.maxlength = t.maxLength), e[t.labelName || t.labelId || t.name || t.id] = i, e
                        }, {})
                    }), config.elements.some(function(e) {
                        return "code" === e.type
                    }) && (prismjs__WEBPACK_IMPORTED_MODULE_16___default.a.fileHighlight(), prismjs__WEBPACK_IMPORTED_MODULE_16___default.a.highlightAll()), config.elements.filter(function(e) {
                        return "date" === e.type
                    }).map(function(e) {
                        _[e.id] = $("#" + e.id).pickadate({
                            max: e.max,
                            min: e.min,
                            editable: !0,
                            selectYears: !0,
                            selectMonths: !0
                        }), _[e.id].open = function(t) {
                            $(t.currentTarget).hasClass("disabled") || _[e.id].pickadate("picker").open(), t.stopPropagation()
                        }, _[e.id].next = function() {
                            var t = _[e.id].pickadate("picker"),
                                i = t.get("select");
                            null !== i && t.set("select", [i.year, i.month, i.date + 1])
                        }, _[e.id].prev = function() {
                            var t = _[e.id].pickadate("picker"),
                                i = t.get("select");
                            null !== i && t.set("select", [i.year, i.month, i.date - 1])
                        }, _[e.id].set = function(t) {
                            var i = _[e.id].pickadate("picker");
                            null !== t && i.set("select", t)
                        }
                    }), config.elements.filter(function(e) {
                        return "month" === e.type || "periodpicker" === e.type.toLowerCase() && !e.selectable
                    }).map(function(e) {
                        _[e.id] = $("#" + e.id).pickamonth({
                            max: e.max,
                            min: e.min,
                            clear: !1
                        }), _[e.id].open = function(t) {
                            $(t.currentTarget).hasClass("disabled") || _[e.id].pickamonth("picker").open(), t.stopPropagation()
                        }, _[e.id].next = function() {
                            var t = _[e.id].pickamonth("picker"),
                                i = t.get("select");
                            null !== i && t.set("select", [i.year, i.month + 1, 1])
                        }, _[e.id].prev = function() {
                            var t = _[e.id].pickamonth("picker"),
                                i = t.get("select");
                            null !== i && t.set("select", [i.year, i.month - 1, 1])
                        }, $("#" + e.id).off("focus")
                    }), config.elements.filter(function(e) {
                        return "periodpicker" === e.type.toLowerCase() && e.selectable
                    }).map(function(e) {
                        var t = e.inicio || e.id + "Inicio",
                            i = e.fim || e.id + "Fim";
                        _[e.id] = $("#" + e.id).pickadate({
                            max: e.max,
                            min: e.min,
                            selectYears: !0,
                            closeOnSelect: !1,
                            onSet: function(a) {
                                event.stopPropagation(), event.preventDefault();
                                var n = _[e.id].pickadate("picker");
                                a.select ? n.get("min").date == -1 / 0 ? n.set("min", new Date(a.select)) : n.set("max", new Date(a.select)) : null === a.clear && (n.set("min", !1), n.set("max", !1)), $("#" + t).val(n.get("min").date !== -1 / 0 ? n.get("min", "yyyy-mm-dd") : ""), $("#" + i).val(n.get("max").date !== 1 / 0 ? n.get("max", "yyyy-mm-dd") : "");
                                var o = $("#" + t).val().length + $("#" + i).val().length > 0 ? " - " : "",
                                    s = n.get("min").date !== -1 / 0 ? n.get("min", "dd/mm/yyyy") : "",
                                    r = n.get("max").date !== 1 / 0 ? n.get("max", "dd/mm/yyyy") : "";
                                $("#" + e.id).val("" + s + o + r), n.get("min").date !== -1 / 0 && n.get("max").date !== 1 / 0 && ($("#" + e.id + "_table div.picker__day:not(.picker__day--disabled)").addClass("picker__day--selected"), $("#" + e.id).trigger("change"), n.close())
                            }
                        }), _[e.id].open = function(t) {
                            $(t.currentTarget).hasClass("disabled") || _[e.id].pickadate("picker").open(), t.stopPropagation()
                        }, _[e.id].set = function(t) {
                            var i = _[e.id].pickadate("picker");
                            null !== t && i.set("select", t)
                        }
                    }), config.elements.filter(function(e) {
                        return "time" === e.type
                    }).map(function(e) {
                        _[e.id] = $("#" + e.id).pickatime({
                            editable: !0,
                            format: "HH:i"
                        }), _[e.id].open = function(t) {
                            $(t.currentTarget).hasClass("disabled") || _[e.id].pickatime("show"), t.stopPropagation()
                        }, $("#" + e.id).off("focus")
                    }), $("#" + config.id + " input.autocomplete").each(function(e, t) {
                        var i = $(t);
                        _[i.attr("id")] = $(t).autocomplete({
                            delay: 200,
                            minLength: 0,
                            classes: {
                                "ui-autocomplete": "autocomplete-content dropdown-content"
                            },
                            source: function(e, t) {
                                i.data("prefilter") && (e.prefilter = $("#" + i.data("prefilter")).val()), $.ajax({
                                    url: i.data("url"),
                                    data: e,
                                    success: function(e) {
                                        (i.data("urlPost") || i.data("urlPostModal")) && e.unshift({
                                            id: "",
                                            label: "",
                                            isAddMenu: !0,
                                            isOpenModal: !!i.data("urlPostModal"),
                                            url: i.data("urlPostModal") || i.data("urlPost")
                                        }), t(e)
                                    }
                                })
                            },
                            change: function(e, t) {
                                return t && t.item ? ($("#" + i.attr("id")).val(t.item.label), $("#" + i.data("target")).val(t.item.id)) : ($("#" + i.attr("id")).val(""), $("#" + i.data("target")).val("")), !1
                            },
                            select: function(e, t) {
                                var a = $("#" + i.attr("id")),
                                    n = i.data("postField") || "descricao";
                                if (t.item.isOpenModal) Object(_loading__WEBPACK_IMPORTED_MODULE_15__.a)(), $.ajax({
                                    type: "GET",
                                    url: t.item.url,
                                    success: function(e) {
                                        e.elements.filter(function(e) {
                                            return e.id == n
                                        }).map(function(e) {
                                            return e.value = i.val()
                                        }), Object(_modal__WEBPACK_IMPORTED_MODULE_23__.a)(e), $("#" + e.id + " #id").on("change", function() {
                                            var t = $("#" + e.id + " #id").val(),
                                                a = $("#" + e.id + " #" + n).val();
                                            t && ($("#" + i.attr("id")).val(a), $("#" + i.data("target")).val(t), updateTextFields(config.id))
                                        }), Object(_loading__WEBPACK_IMPORTED_MODULE_15__.b)()
                                    },
                                    error: function(e) {
                                        var t = e.status + " - " + e.statusText;
                                        Object(_loading__WEBPACK_IMPORTED_MODULE_15__.b)(), Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(t)
                                    }
                                });
                                else if (t.item.isAddMenu) {
                                    var o = $("#" + i.attr("id")).val();
                                    a.loading("start"), a.blur(), $.ajax({
                                        url: t.item.url,
                                        type: "POST",
                                        data: {
                                            term: o
                                        },
                                        success: function(e) {
                                            if (e.success) {
                                                var t = "Cadastrado com sucesso.";
                                                "" !== e.message && (t = e.message), $("#" + i.attr("id")).val(o), $("#" + i.data("target")).val(e.id), Object(_toast__WEBPACK_IMPORTED_MODULE_22__.c)(t)
                                            } else $("#" + i.attr("id")).val(""), $("#" + i.data("target")).val(""), submitErrorHandler(null, e.message);
                                            updateTextFields(config.id), i.loading("stop")
                                        },
                                        error: function(e, t) {
                                            Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)(t), i.loading("stop")
                                        }
                                    })
                                } else $("#" + i.attr("id")).val(t.item.label), $("#" + i.data("target")).val(t.item.id);
                                return !1
                            },
                            messages: {
                                noResults: "",
                                results: function() {}
                            }
                        }).autocomplete("instance"), _[i.attr("id")]._renderItem = function(e, t) {
                            var a = $("#" + i.attr("id")).val();
                            if (t.isAddMenu) return "" === a ? e : $("<li>").append('<div class="addMenu"><i class="material-icons right">add</i>Adicionar</div>').appendTo(e);
                            var n = t.level || 0,
                                o = t.label,
                                s = t.detail ? "<br><small>" + t.detail + "</small>" : "";
                            if ("" != a) {
                                var r = new RegExp("(" + a.replace(/[-/\\^$*+?.()|[\]{}]/g, "\\$&").split(" ").join("|") + ")", "gi");
                                o = t.label.replace(r, '<span class="highlight">$1</span>'), s = t.detail ? "<br><small>" + t.detail.replace(r, '<span class="highlight">$1</span>') + "</small>" : ""
                            }
                            return $("<li>").append('<span class="lvl-' + n + '">' + o + s + "</span>").appendTo(e)
                        }, _[i.attr("id")]._renderItemData = function(e, t) {
                            return _[i.attr("id")]._renderItem(e, t).data("ui-autocomplete-item", t)
                        }, _[i.attr("id")]._renderMenu = function(e, t) {
                            $.each(t, function(t, a) {
                                _[i.attr("id")]._renderItemData(e, a)
                            })
                        }, i.on("focus", function() {
                            $(t).prop("readonly") || $(t).prop("disabled") || $(t).autocomplete("search", $(t).val())
                        })
                    }), $("#" + config.id + " input[type=checkbox]").on("click", function() {
                        $(this).attr("checked") ? $(this).removeAttr("checked") : $(this).attr("checked", "checked")
                    }), maskFields(), $("#" + config.id + " .dropdown-button").length > 0 && $("#" + config.id + " .dropdown-button").dropdown(), $("#" + config.id + " textarea").length > 0 && $("#" + config.id + " textarea").trigger("autoresize"), $("#" + config.id + " textarea[data-length]").length > 0 && $("#" + config.id + " textarea[data-length]").characterCounter(), $("#" + config.id + " select").length > 0 && $("#" + config.id + " select").material_select(), config.elements.filter(function(e) {
                        return e.domEvents
                    }).map(function(input) {
                        input.domEvents.map(function(ev) {
                            ev.function && /^[a-zA-Z]+$/.test(ev.function) && $("#" + (input.labelId || input.id)).on(ev.domEvent, eval("(" + ev.function+")"))
                        })
                    }), $("#" + config.id + " select").on("change", function() {
                        $(this).material_select()
                    }), $("#" + config.id + " .buttongroup").each(function() {
                        var e = $(this);
                        e.on("click", ".tab a", function() {
                            e.find(".tab a.active").removeClass("active"), $(this).addClass("active")
                        })
                    }), $(".image-field input[type=file]").each(function() {
                        var e = $(this);
                        $("#" + e.attr("id") + "Clear").on("click", function() {
                            e.val("");
                            var t = document.getElementById(e.attr("id") + "Thumb"),
                                i = document.getElementById(e.attr("id") + "Icon");
                            t.removeAttribute("src"), i.style.display = "block", t.style.display = "none"
                        })
                    }), $(".image-field input[type=file]").on("change", function(e) {
                        var t = $(this),
                            i = document.getElementById(t.attr("id") + "Thumb"),
                            a = document.getElementById(t.attr("id") + "Icon");
                        if (a.style.display = i.src ? "none" : "block", i.style.display = i.src ? "inline" : "none", this.files && this.files[0])
                            if (new RegExp(this.accept.replace("*", ".*")).test(this.files[0].type)) {
                                var n = new FileReader;
                                n.readAsDataURL(this.files[0]), n.onloadend = function() {
                                    i.src = n.result, a.style.display = "none", i.style.display = "inline"
                                }
                            } else e.stopImmediatePropagation(), "image" === this.accept.replace("/*", "") ? Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)("O arquivo deve ser uma imagem") : -1 === this.accept.replace("image/", "").indexOf("/") ? Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)("O arquivo deve ser do tipo (" + this.accept.replace("image/", ".") + ")") : Object(_toast__WEBPACK_IMPORTED_MODULE_22__.b)("O arquivo deve ser do tipo (" + this.accept.replace("/*", "") + ")")
                    }), config.helpers && updateHelpers(config.helpers, _.videoPlayer), config.readyFn && /^[a-zA-Z]+$/.test(config.readyFn) && $("form#" + config.id).ready(eval("(" + config.readyFn + ")"))
                }
            },
            updateActiveMenu = function(e) {
                if (e) {
                    var t = "#nav-mobile > li > ul.side";
                    $(t + " a[data-go*='" + e + "'], " + t + " a[href*='" + e + "']").length > 0 && ($("#nav-mobile li.active").removeClass("active"), $("#nav-mobile a.has-active").removeClass("has-active"), $("#nav-mobile a[data-go*='" + e + "'], #nav-mobile a[href*='" + e + "']").closest("li:has(a:not(.collapsible-header))").addClass("active"), $(t + " .collapsible-body:has(li.active:not(.collapsible-header))").siblings().each(function(e, t) {
                        $(t).addClass("has-active"), $(t).hasClass("active") || $(t).click()
                    }))
                }
            },
            loadScript = function(e, t, i, a) {
                if (!(document.querySelectorAll('[src="' + e + '"]').length > 0)) {
                    var n = document.createElement("script");
                    n.src = e, n.type = "text/javascript", n.async = void 0 === a || a, t && (n.readyState ? n.onreadystatechange = function() {
                        "loaded" !== n.readyState && "complete" !== n.readyState || (n.onreadystatechange = null, t())
                    } : n.onload = function() {
                        t()
                    }), i || (i = "head"), $(i).append(n)
                }
            },
            processAppConfig = function(e) {
                var t = {};
                return e.target && (e.target.link ? t.href = e.target.link : e.target.go && (t.class = "linkGo", t["data-go"] = e.target.go)), /cdnfly01.(azureedge|blob.core.windows)(.net\/img\/icon\/.*\.)png/.test(e.icon) && (e.icon = e.icon.replace(/cdnfly01.(azureedge|blob.core.windows)(.net\/img\/icon\/.*\.)png/g, "mpn.azureedge$2svg")), e.attr = t, e = $.extend(!0, {}, {
                    icon: "https://mpn.azureedge.net/img/icon/default.png"
                }, e)
            },
            refresh = function(e) {
                var t;
                if (void 0 === e ? t = Object.keys(_) : Array.isArray(e) ? t = e : "string" == typeof e && (t = [e]), void 0 !== t)
                    for (var i = 0; i < t.length; i++) "object" == typeof _[t[i]] && "function" == typeof _[t[i]].refresh && _[t[i]].refresh()
            },
            setCookie = function(e, t, i) {
                var a = "",
                    n = t || "";
                if (i) {
                    var o = new Date;
                    o.setTime(o.getTime() + 24 * i * 60 * 60 * 1e3), a = "; expires=" + o.toUTCString()
                }
                document.cookie = e + "=" + encodeURIComponent(n) + a + "; path=/"
            },
            getCookie = function(e) {
                for (var t = e + "=", i = document.cookie.split(";"), a = 0; a < i.length; a++) {
                    for (var n = i[a];
                        " " == n.charAt(0);) n = n.substring(1, n.length);
                    if (0 == n.indexOf(t)) return decodeURIComponent(n.substring(t.length, n.length))
                }
                return null
            },
            eraseCookie = function(e) {
                setCookie(e, "", -1)
            },
            getScript = function(e, t) {
                if (Array.isArray(e)) getScript(e[0], e.length > 1 ? function() {
                    return getScript(e.slice(1), t)
                } : t);
                else if (document.querySelector('script[src="' + e + '"]')) t();
                else {
                    var i = document.createElement("script");
                    i.onload = t, i.src = e, document.head.appendChild(i)
                }
            },
            cookie2Object = function(e) {
                var t = document.cookie.split(";").find(function(t) {
                    return t.includes(e + "=")
                });
                return t ? (t = t.trim().replace(e + "=", ""), JSON.parse('{"' + t.replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g, '":"') + '"}')) : {}
            }
    }).call(this, __webpack_require__(1))
}, , function(module, __webpack_exports__, __webpack_require__) {
    "use strict";
    (function($) {
        __webpack_require__.d(__webpack_exports__, "b", function() {
            return stop
        }), __webpack_require__.d(__webpack_exports__, "a", function() {
            return start
        });
        var _functions__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0),
            stop = function(callback) {
                if ($(":loading").each(function(e, t) {
                        $(t).loading("resize")
                    }), 0 == $("#nav-mobile, #loginscreen, .mat-sidenav-container").length ? setTimeout(stop, 300) : $("body").addClass("loaded"), callback && /^[a-zA-Z]+$/.test(callback)) {
                    var cb = eval("(" + callback + ")");
                    "function" == typeof cb && cb()
                }
            },
            start = function() {
                0 == $("#loader-wrapper").length && $("body").append(Object(_functions__WEBPACK_IMPORTED_MODULE_0__.b)("div", {
                    id: "loader-wrapper"
                }, Object(_functions__WEBPACK_IMPORTED_MODULE_0__.b)("div", {
                    id: "loader"
                }))), $("body").removeClass("loaded")
            }
    }).call(this, __webpack_require__(1))
}, function(e, t, i) {
    "use strict";
    i.d(t, "b", function() {
        return r
    }), i.d(t, "c", function() {
        return c
    });
    var a = i(82),
        n = i(0),
        o = function(e) {
            try {
                var t = JSON.parse(e);
                e = o(t.error || t.errorMessage || t.message)
            } catch (e) {}
            return e
        },
        s = function(e, t, i) {
            e = o(e);
            var s = Object(n.b)("div", null, [Object(n.b)("span", null, e), Object(n.b)("button", {
                    onclick: "$(this).parent().remove();",
                    class: "btn-flat toast-action"
                }, "Fechar")]),
                r = i || ("error" === t ? 2e4 : 3e3);
            return new a.a(s.innerHTML, r, t + "-feedback")
        };
    t.a = s;
    var r = function(e, t) {
            return s(e, "error", t)
        },
        c = function(e, t) {
            return s(e, "success", t)
        }
}, function(e, t, i) {
    "use strict";
    var a = i(0);
    t.a = function(e, t, i) {
        return Object(a.b)("div", {
            id: e + "Field",
            class: "input-field " + (t || "col s12")
        }, i)
    }
}, function(e, t, i) {
    "use strict";
    (function(e) {
        i.d(t, "a", function() {
            return D
        }), i.d(t, "i", function() {
            return M
        }), i.d(t, "g", function() {
            return x
        }), i.d(t, "h", function() {
            return P
        }), i.d(t, "j", function() {
            return T
        }), i.d(t, "e", function() {
            return A
        }), i.d(t, "b", function() {
            return $
        }), i.d(t, "d", function() {
            return I
        }), i.d(t, "c", function() {
            return L
        }), i.d(t, "f", function() {
            return R
        }), i.d(t, "k", function() {
            return S
        });
        var a = i(4),
            n = i(84),
            o = i(85),
            s = i(86),
            r = i(87),
            c = i(88),
            l = i(89),
            d = i(90),
            u = i(91),
            p = i(92),
            f = i(93),
            h = i(113),
            m = i(94),
            b = i(95),
            _ = i(45),
            v = i(96),
            g = i(97),
            y = i(98),
            k = i(99),
            w = i(100),
            O = i(101),
            j = i(46),
            E = i(0),
            C = function(e) {
                var t;
                switch (e.id || (e.id = Object(E.i)(e.type.toLowerCase())), e.type.toLowerCase()) {
                    case "text":
                    case "email":
                        t = Object(c.a)(e);
                        break;
                    case "password":
                        t = Object(l.a)(e);
                        break;
                    case "button":
                        t = Object(h.a)(e);
                        break;
                    case "time":
                    case "date":
                    case "month":
                        t = Object(m.a)(e);
                        break;
                    case "checkbox":
                        t = Object(v.a)(e);
                        break;
                    case "autocomplete":
                        t = Object(b.a)(e);
                        break;
                    case "hidden":
                        t = Object(r.a)(e);
                        break;
                    case "rating":
                        t = Object(o.a)(e);
                        break;
                    case "range":
                        t = Object(p.a)(e);
                        break;
                    case "code":
                        t = Object(n.a)(e);
                        break;
                    case "statictext":
                        t = Object(s.a)(e);
                        break;
                    case "file":
                        t = Object(d.a)(e);
                        break;
                    case "select":
                        t = Object(_.a)(e);
                        break;
                    case "labelset":
                        t = Object(y.a)(e);
                        break;
                    case "textarea":
                        t = Object(g.a)(e);
                        break;
                    case "buttongroup":
                        t = Object(k.a)(e);
                        break;
                    case "periodpicker":
                        t = Object(w.a)(e);
                        break;
                    case "datatable":
                        t = Object(O.a)(e);
                        break;
                    case "div":
                        e.id || (e.id = Object(E.i)("div")), t = Object(E.b)("div", {
                            id: e.id + "Field",
                            class: "" + (e.class || "col s12")
                        });
                        break;
                    default:
                        t = Object(f.a)(e)
                }
                return t
            },
            D = function(e) {
                e.id || (e.id = Object(E.i)("app")), e = Object(E.k)(e);
                var t = Object(E.b)("a", e.attr, Object(E.b)("div", {
                    class: "app",
                    id: e.id
                }, [Object(E.b)("div", {
                    class: "app-content"
                }, [Object(E.b)("img", {
                    class: "app-icon",
                    src: e.icon,
                    onload: "$(this).addClass('loaded')",
                    alt: e.title
                }), Object(E.b)("span", {
                    class: "app-title"
                }, e.title)])]));
                return Object(E.b)("div", {
                    id: "d" + e.id,
                    class: e.class
                }, t)
            },
            M = function(e) {
                return Object(E.b)("div", {
                    id: "appMenu"
                }, [Object(E.b)("ul", {
                    class: "row"
                }, e.reduce(function(e, t) {
                    return t = Object(E.k)(t), e.concat(Object(E.b)("li", {
                        class: "col s2"
                    }, [Object(E.b)("a", t.attr, Object(E.b)("div", {
                        class: "app with-tooltip",
                        id: t.id,
                        "data-tooltip": t.title
                    }, [Object(E.b)("div", {
                        class: "app-content"
                    }, [Object(E.b)("img", {
                        class: "app-icon",
                        src: t.icon,
                        onload: "$(this).addClass('loaded')",
                        alt: t.title
                    })])]))]))
                }, []))])
            },
            x = function(t, i, n) {
                t.id || (t.id = Object(E.i)("form"));
                var o, s = {
                        id: t.id,
                        class: t.class || "col s12"
                    },
                    r = 0;
                return i && (s.action = i), t.method && (s.method = t.method), t.encType && (s.enctype = t.encType), t.labels = e.extend(!0, {
                    next: "Próximo",
                    previous: "Anterior",
                    submit: "Salvar"
                }, t.labels), t.steps ? (t.rule || (t.rule = "parallel"), o = Object(E.b)("ul", {
                    class: "stepper horizontal " + (t.rule || "")
                }, t.steps.reduce(function(e, i) {
                    var o = [];
                    i === t.steps[0] ? o.push(Object(E.b)("button", {
                        class: "btn next-step"
                    }, t.labels.next)) : i === t.steps[t.steps.length - 1] ? (o.push(Object(E.b)("button", {
                        class: "btn previous-step btn-secondary"
                    }, t.labels.previous)), o.push(Object(E.b)("button", {
                        class: "btn",
                        type: "submit"
                    }, t.labels.submit))) : (o.push(Object(E.b)("button", {
                        class: "btn previous-step btn-secondary"
                    }, t.labels.previous)), o.push(Object(E.b)("button", {
                        class: "btn next-step"
                    }, t.labels.next)));
                    var s, c = t.elements.slice(r, r + i.quantity).reduce(function(e, i) {
                        return t.readonly && (i.disabled = !0, i.readonly = !0), e.concat(C(i))
                    }, []);
                    return t.elements.slice(r, r + i.quantity).filter(function(e) {
                        return e.required
                    }).length > 0 && c.push(Object(a.a)("", "col s12", Object(E.b)("small", null, "* Campos obrigatórios"))), s = n ? Object(E.b)("li", {
                        id: i.id || "",
                        class: "step" + (0 == r ? " active" : "") + (t.showStepNumbers ? "" : " no-numbers")
                    }, [Object(E.b)("div", {
                        class: "step-title"
                    }, i.title), Object(E.b)("div", {
                        class: "step-content"
                    }, [Object(E.b)("div", {
                        class: "row"
                    }, c)]), Object(E.b)("div", {
                        class: "step-actions"
                    }, o)]) : Object(E.b)("li", {
                        id: i.id || "",
                        class: "step" + (0 == r ? " active" : "") + (t.showStepNumbers ? "" : " no-numbers")
                    }, [Object(E.b)("div", {
                        class: "step-title"
                    }, i.title), Object(E.b)("div", {
                        class: "step-content"
                    }, [Object(E.b)("div", {
                        class: "row"
                    }, c), Object(E.b)("div", {
                        class: "step-actions"
                    }, o)])]), r += i.quantity, e.concat(s)
                }, []))) : (o = t.elements.reduce(function(e, i) {
                    return t.readonly && (i.disabled = !0, i.readonly = !0), e.concat(C(i))
                }, []), t.elements.filter(function(e) {
                    return e.required
                }).length > 0 && o.push(Object(a.a)("", "col s12", Object(E.b)("small", null, "* Campos obrigatórios")))), Object(E.b)("form", s, o)
            },
            P = function(t, i) {
                var a = function(e, t) {
                        var a = {};
                        return a.id = e.id, a.class = (t || "") + " " + (e.class || ""), e.name && (a.name = e.name), e.onClickFn && (a.onclick = e.onClickFn + "(" + (i && "submit" === e.type ? '"#' + i + '"' : "event") + ");"), Object(E.b)("a", a, e.label || "")
                    },
                    n = function(e, t) {
                        return e.reduce(function(e, i) {
                            return e.concat(Object(E.b)("li", null, [a(i, t)]))
                        }, [])
                    },
                    o = [];
                if ((t = e.extend(!0, {}, t)).title && o.push(Object(E.b)("h4", {
                        class: "thin inline-block fly01-main-title"
                    }, t.title)), t.buttons) {
                    var s = [],
                        r = [];
                    if (0 == t.buttons.filter(function(e) {
                            return e.position
                        }).length) s = t.buttons.length >= 3 ? [Object(E.b)("li", null, [a(t.buttons.slice(0, 1)[0], "btn")]), Object(E.b)("li", null, [Object(E.b)("a", {
                        class: "btn btn-narrow dropdown-button",
                        "data-activates": "headerDropdown"
                    }, [Object(E.b)("i", {
                        class: "material-icons"
                    }, "expand_more")]), Object(E.b)("ul", {
                        class: "dropdown-content",
                        id: "headerDropdown"
                    }, n(t.buttons.slice(1)))])] : n(t.buttons, "btn");
                    else {
                        t.buttons.filter(function(e) {
                            return "out" == e.position
                        }).reduce(function(e, t) {
                            s.push(Object(E.b)("li", {
                                class: "hide-on-small-and-down"
                            }, [a(t, "btn btn-secondary")])), r.push(Object(E.b)("li", {
                                class: "hide-on-med-and-up"
                            }, [a(t)]))
                        }, []), s.push(Object(E.b)("li", null, [a(t.buttons.filter(function(e) {
                            return "main" == e.position
                        })[0], "btn")]));
                        var c = r.length;
                        if (t.buttons.filter(function(e) {
                                return void 0 === e.position || "out" !== e.position && "main" !== e.position
                            }).reduce(function(e, t) {
                                r.push(Object(E.b)("li", null, [a(t)]))
                            }, []), r.length > 0) {
                            var l = c == r.length ? {
                                class: "hide-on-med-and-up"
                            } : null;
                            s.push(Object(E.b)("li", l, [Object(E.b)("a", {
                                class: "btn btn-narrow dropdown-button",
                                "data-activates": "headerDropdown",
                                "data-beloworigin": "true",
                                "data-alignment": "right",
                                "data-constrainwidth": "false"
                            }, [Object(E.b)("i", {
                                class: "material-icons"
                            }, "expand_more")]), Object(E.b)("ul", {
                                class: "dropdown-content",
                                id: "headerDropdown"
                            }, r)]))
                        }
                    }
                    o.push(Object(E.b)("ul", {
                        class: "right valign-wrapper fly01-buttons"
                    }, s))
                }
                return o.length > 0 ? Object(E.b)("div", {
                    id: "headTop",
                    class: "z-depth-0-half"
                }, [Object(E.b)("div", {
                    class: "container"
                }, [Object(E.b)("div", {
                    class: "row"
                }, [Object(E.b)("div", {
                    class: "col s12 fly01-main-header"
                }, o)])])]) : ""
            },
            T = function(e, t) {
                var i = [],
                    a = {};
                return t && i.push(Object(E.b)("span", {
                    class: "labeltop"
                }, t)), e.link && (a.link = e.link), e.onClick && (a["data-go"] = e.onClick, a.class = "linkGo"), e.label && (a.label = e.label), (e.link || e.onClick || e.label) && i.push(Object(j.a)(a)), Object(E.b)("li", {
                    class: e.class || ""
                }, i)
            },
            A = function(e) {
                return e.id || (e.id = Object(E.i)("dt")), Object(E.b)("table", {
                    class: "display striped responsive nowrap " + (e.class || ""),
                    width: "100%",
                    id: e.id
                }, [Object(E.b)("thead", null, [Object(E.b)("tr", {
                    id: "columns"
                }, e.columns.reduce(function(t, i, a) {
                    return i.searchable && i.options && i.options.length > 0 ? (i.options.unshift({
                        label: "",
                        value: ""
                    }), t.concat(Object(E.b)("th", null, [Object(_.a)({
                        id: i.dataField,
                        type: i.type,
                        index: a,
                        name: i.dataField,
                        class: "hide-on-small-only fly01dt-select " + (i.class || ""),
                        label: i.displayName,
                        options: i.options
                    }), Object(E.b)("span", {
                        class: "show-on-small"
                    }, i.displayName)]))) : i.searchable ? t.concat(Object(E.b)("th", null, [Object(u.a)({
                        id: i.dataField,
                        type: i.type,
                        index: a,
                        name: i.dataField,
                        class: "hide-on-small-only fly01dt-filter " + (i.class || ""),
                        label: i.displayName
                    }), Object(E.b)("span", {
                        class: "show-on-small"
                    }, i.displayName)])) : i.displayName ? t.concat(Object(E.b)("th", null, [Object(E.b)("span", null, i.displayName)])) : !e.options || e.options.withoutRowMenu || e.options.withoutMenu ? t.concat(Object(E.b)("th")) : t.concat(Object(E.b)("th", {
                        class: "dt-controls"
                    }))
                }, []))]), Object(E.b)("tfoot", null, [Object(E.b)("tr", null, e.columns.reduce(function(e) {
                    return e.concat(Object(E.b)("th", null, [""]))
                }, []))])])
            },
            $ = function(e) {
                return e.id || (e.id = Object(E.i)("cal")), Object(E.b)("div", {
                    id: e.id,
                    class: e.class
                }, "")
            },
            I = function(e) {
                e.id || (e.id = Object(E.i)("chart"));
                var t = {
                    id: e.id,
                    height: 400
                };
                t.width = e.drawType && "bar" != e.drawType && "line" != e.drawType ? 400 : 800;
                var i = [];
                return e.title && "" != e.title && i.push(Object(E.b)("h6", null, e.title || "")), i.push(Object(E.b)("canvas", t)), Object(E.b)("div", {
                    id: "d" + e.id,
                    class: e.class
                }, i)
            },
            L = function(e) {
                e.id || (e.id = Object(E.i)("card"));
                var t = "";
                e.action && (e.action.class = "" + (e.action.class || ""), e.action.onClick && (e.action["data-go"] = e.action.onClick, e.action.class = "linkGo " + (e.action.class || ""), e.action.onClick = ""), t = Object(E.b)("div", {
                    class: "card-action right-align"
                }, Object(j.a)(e.action)));
                var i = e.color || "primary-color",
                    a = Object(E.b)("div", {
                        class: "card " + i + (e.class.indexOf("-text") < 0 ? " white-text" : ""),
                        id: e.id,
                        "data-color": i
                    }, [Object(E.b)("div", {
                        class: "card-content"
                    }, [Object(E.b)("span", {
                        class: "card-title condensed"
                    }, e.title || ""), Object(E.b)("h5", {
                        class: "center"
                    }, e.placeholder || "")]), t]);
                return Object(E.b)("div", {
                    id: "d" + e.id,
                    class: e.class
                }, a)
            },
            R = function(e) {
                e.id || (e.id = Object(E.i)("div"));
                var t = [];
                return e.elements && (t = e.elements.reduce(function(t, i) {
                    return e.readonly && (i.disabled = !0, i.readonly = !0), t.concat(C(i))
                }, []), e.elements.filter(function(e) {
                    return e.required
                }).length > 0 && t.push(Object(a.a)("", "col s12", Object(E.b)("small", null, "* Campos obrigatórios")))), Object(E.b)("div", {
                    id: e.id,
                    class: e.class || "col s12"
                }, t)
            },
            S = function(e) {
                return e.id || (e.id = Object(E.i)("tabs")), Object(E.b)("div", {
                    class: e.class
                }, [Object(E.b)("ul", {
                    id: e.id,
                    class: "tabs tabs-fixed-width"
                }, e.tabs.reduce(function(e, t) {
                    return e.concat(Object(E.b)("li", {
                        class: "tab" + (t.disabled ? " disabled" : "")
                    }, [Object(E.b)("a", {
                        href: "#" + t.id,
                        class: t.active ? "active" : ""
                    }, t.title)]))
                }, []))])
            }
    }).call(this, i(1))
}, , , function(e, t, i) {
    "use strict";
    var a = i(22),
        n = i(44);
    t.a = function(e, t, i, o) {
        "string" == typeof e ? Object(a.a)(n.a, e, t, i, o) : Object(n.a)(e, t, null, i, o)
    }
}, , , , , , function(e, t, i) {
    "use strict";
    i.r(t), t.default = {}
}, , , function(e, t, i) {
    "use strict";
    (function(e) {
        i(55), i(56);
        var a = i(2),
            n = i(3),
            o = i(8),
            s = i(0),
            r = i(5),
            c = i(14);
        t.a = function(t, i) {
            var l, d = [],
                u = [];
            t.cancelAction && (l = {
                class: "modal-action modal-close btn btn-secondary"
            }, t.cancelAction.onClickFn && (d.push(t.cancelAction.onClickFn), l.onclick = t.cancelAction.onClickFn + "()"), u.push(Object(s.b)("a", l, t.cancelAction.label))), t.confirmAction && (l = {
                class: "modal-action btn " + (t.confirmAction.class ? t.confirmAction.class : "")
            }, t.confirmAction.onClickFn ? (d.push(t.confirmAction.onClickFn), l.onclick = t.confirmAction.onClickFn + "()") : l.class = l.class.concat(" modal-submit"), u.push(Object(s.b)("a", l, t.confirmAction.label)));
            var p, f = void 0 === i && t.action ? t.action.create : t.action ? t.action.edit || t.action.create : "";
            e("div#md" + t.id).remove(), p = t.steps ? Object(s.b)("div", {
                id: "md" + t.id,
                class: "modal modal-fixed-footer"
            }, Object(r.g)(t, f, !0)) : Object(s.b)("div", {
                id: "md" + t.id,
                class: "modal modal-fixed-footer"
            }, [Object(s.b)("div", {
                class: "modal-header"
            }, [Object(s.b)("h5", null, t.title)]), Object(s.b)("div", {
                class: "modal-content"
            }, [Object(s.b)("div", {
                class: "row"
            }, Object(r.g)(t, f))]), Object(s.b)("div", {
                class: "modal-footer"
            }, u)]), e(t.parent || "main").append(p), t.readyFn && d.push(t.readyFn), t.afterLoadFn && d.push(t.afterLoadFn), t.elements.filter(function(e) {
                return e.domEvents
            }).map(function(e) {
                e.domEvents.map(function(e) {
                    d.push(e.function)
                })
            }), t.functions && (d = d.concat(t.functions)), Object(s.h)(t.urlFunctions, d), t.steps && e(".stepper").activateStepper({
                autoFocusInput: !1
            }), c[t.id] = {}, c[t.id].refresh = function(i) {
                Object(a.a)(), i && t.action && t.action.get ? e.ajax({
                    type: "GET",
                    url: t.action.get + i,
                    success: function(i) {
                        !1 === i.success ? (Object(n.a)(i.message, "error", 8e3), t.action.list && Object(o.a)(t.action.list)) : (Object(s.c)(e("#" + t.id), i, t.afterLoadFn), Object(s.f)(t), Object(s.r)(t.id), Object(a.b)())
                    }
                }) : (Object(s.f)(t), Object(s.r)(t.id), Object(a.b)())
            }, c[t.id].close = function() {
                e("div#md" + t.id + ".modal").modal("close")
            }, c[t.id].open = function() {
                e("div#md" + t.id + ".modal").modal("open")
            }, t.confirmAction && !t.confirmAction.onClickFn && (e("div#md" + t.id + " .modal-submit").on("click", function() {
                var t = e(this).parent().parent().find("form");
                t.attr("action") && t.submit()
            }), e("form#" + t.id).on("submit", function(i) {
                if (Object(a.a)(), i.preventDefault(), !e(this).valid()) return Object(a.b)(), !1;
                var r = e(this).validate();
                e.ajax({
                    url: e(this).attr("action"),
                    type: "POST",
                    data: e(this).serialize(),
                    success: function(i) {
                        if (i.success) {
                            var l = "Cadastrado com sucesso.";
                            "" !== i.message && (l = i.message), c[t.id].close(), t.action.list ? Object(o.a)(t.action.list) : i.id && (e("#" + t.id + " #id").val(i.id), e("#" + t.id + " #id").change()), Object(n.a)(l, "success")
                        } else Object(s.o)(r, i.message);
                        Object(a.b)()
                    },
                    error: function(e, t) {
                        Object(n.a)(t, "error"), Object(a.b)()
                    }
                })
            })), e("div#md" + t.id + ".modal").modal(), c[t.id].open(), c[t.id].refresh(i)
        }
    }).call(this, i(1))
}, , , , , function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(2),
            n = i(3);
        t.a = function(t, i, o, s, r) {
            e("dynamicScripts").html(""), Object(a.a)(), e.ajax({
                type: "GET",
                url: i,
                dataType: "json",
                crossDomain: !0,
                xhrFields: {
                    withCredentials: !0
                },
                success: function(e) {
                    t(e, o, i, s, r)
                },
                error: function(e) {
                    if (200 != e.status) {
                        var t = e.status + " - " + e.statusText;
                        Object(n.a)(t, "error"), Object(a.b)()
                    }
                }
            })
        }
    }).call(this, i(1))
}, , , , , , , , , function(e, t, i) {
    "use strict";
    (function(e, a) {
        i(202), i(204), i(205);
        var n = i(32),
            o = i(22),
            s = i(8),
            r = i(0),
            c = i(111),
            l = i(5),
            d = i(3),
            u = i(2),
            p = function(e) {
                e.success ? 302 !== e.code ? window.history.state ? Object(s.a)(window.history.state.urlJson, window.history.state.id, !0, !0) : location.reload() : window.location.replace(e.url) : e.message ? (Object(d.b)(e.message, "error"), Object(u.b)()) : location.reload()
            },
            f = function(e, t) {
                var i = Object(r.b)("span", null, e.description);
                if (t) {
                    var a = new RegExp("(" + t.replace(/[-/\\^$*+?.()|[\]{}]/g, "\\$&").split(" ").join("|") + ")", "gi");
                    i.innerHTML = e.description.replace(a, '<span class="black-text">$1</span>')
                }
                return Object(r.b)("li", {
                    class: "platform-item"
                }, [Object(r.b)("button", {
                    class: "btn btn-wide platformGo truncate",
                    "data-platform": e.id
                }, [i])])
            },
            h = function(t, i) {
                Object(r.e)();
                var o = [],
                    s = [],
                    h = function(e, t) {
                        return e.reduce(function(e, i) {
                            return i.items && i.items.length > 0 ? e.concat(Object(r.b)("li", {
                                class: i.class || ""
                            }, [Object(r.b)("a", {
                                class: "collapsible-header"
                            }, [Object(r.b)("i", {
                                class: "material-icons expand right"
                            }, "expand_more"), i.label]), Object(r.b)("div", {
                                class: "collapsible-body"
                            }, [Object(r.b)("ul", {
                                class: "side collapsible"
                            }, h(i.items, (t ? t + " > " : "") + i.label))])])) : (s.push(Object(l.j)(i, t)), e.concat(Object(l.j)(i)))
                        }, [])
                    };
                t || (t = {}), t.menuItems && t.menuItems.length > 0 && (o = h(t.menuItems, "")), e(t.parent || "header").html(Object(r.b)("form", {
                    action: "Account/LogOff",
                    id: "logoutForm",
                    method: "post",
                    class: "no-margin"
                }));
                var m, b = [],
                    _ = [];
                if (_.push(Object(r.b)("li", null, Object(r.b)("a", {
                        href: "#",
                        "data-activates": "nav-mobile",
                        class: "button-collapse"
                    }, [Object(r.b)("i", {
                        class: "material-icons"
                    }, "menu")]))), t.menuApps && t.menuApps.length > 0 ? (e(t.parent || "header").append(Object(l.i)(t.menuApps)), _.push(Object(r.b)("li", null, Object(r.b)("a", {
                        href: "javascript:void(0)",
                        class: "appMenuToggler hide-on-large-only"
                    }, [Object(r.b)("i", {
                        class: "material-icons"
                    }, "apps")])))) : e("main").addClass("no-apps"), t.userMenuItems && t.userMenuItems.length > 0 && (e(t.parent || "header").append(Object(r.b)("ul", {
                        id: "user-menu",
                        class: "dropdown-content"
                    }, t.userMenuItems.reduce(function(e, t) {
                        return e.concat(Object(l.j)(t))
                    }, []))), b.push(Object(r.b)("li", {
                        class: "flex-0"
                    }, [Object(r.b)("a", {
                        class: "dropdown-button",
                        "data-activates": "user-menu",
                        "data-beloworigin": "true",
                        "data-alignment": "right"
                    }, [Object(r.b)("i", {
                        class: "material-icons"
                    }, "settings")])]))), e(t.parent || "header").append(Object(r.b)("ul", {
                        id: "notification-menu",
                        class: "dropdown-content"
                    }, [Object(r.b)("li", {
                        class: "no-notification"
                    }, [Object(r.b)("span", {
                        class: "center"
                    }, "Não há notificações")]), Object(r.b)("li", {
                        class: "notifications-action"
                    }, [Object(r.b)("span", {
                        class: "action small"
                    }, [Object(r.b)("i", {
                        class: "material-icons right readNotification"
                    }, "done_all")])])])), b.unshift(Object(r.b)("li", {
                        class: "flex-0 notifications-action"
                    }, [Object(r.b)("a", {
                        class: "dropdown-button",
                        "data-activates": "notification-menu",
                        "data-closeonclick": "false",
                        "data-beloworigin": "true",
                        "data-alignment": "right"
                    }, [Object(r.b)("i", {
                        class: "material-icons"
                    }, "notifications_none")])])), function(t) {
                        t = t || "/Account/Platforms", e.ajax({
                            type: "GET",
                            url: t,
                            success: function(t) {
                                if (t.success && t.platforms.length > 0) {
                                    var i = [Object(r.b)("li", {
                                        class: "hide"
                                    }, [Object(r.b)("form", {
                                        id: "platform",
                                        action: t.postFormUrl || t.loginUrl
                                    }, [Object(r.b)("input", {
                                        id: "PlatformId",
                                        name: "PlatformId",
                                        type: "hidden",
                                        value: ""
                                    })])])];
                                    t.platforms.length > 4 && i.push(Object(r.b)("li", {
                                        class: "input-field orange-text"
                                    }, [Object(r.b)("input", {
                                        type: "search",
                                        id: "platformSelect",
                                        autocomplete: "off"
                                    }), Object(r.b)("i", {
                                        class: "orange-text material-icons"
                                    }, "search"), Object(r.b)("label", {
                                        class: "truncate"
                                    }, "Pesquise o ambiente")])), e("header").append(Object(r.b)("ul", {
                                        id: "select-platform",
                                        class: "dropdown-content platform search"
                                    }, t.platforms.slice(0, 10).reduce(function(e, t) {
                                        return e.concat(f(t))
                                    }, i))), e("#platforms-menu").html(Object(r.b)("a", {
                                        class: "dropdown-button",
                                        "data-activates": "select-platform",
                                        "data-closeonclick": "false",
                                        "data-beloworigin": "true",
                                        "data-alignment": "right"
                                    }, [Object(r.b)("i", {
                                        class: "material-icons"
                                    }, "dns")])), e("#platforms-menu .dropdown-button").dropdown(), t.platforms.length > 4 && (e("input#platformSelect").on("blur.platformSelect", function() {
                                        e(".search.platform li.selected").removeClass("selected")
                                    }), e("input#platformSelect").on("keyup.platformSelect", function(i) {
                                        var a = e(i.currentTarget).val().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase(),
                                            n = i.keyCode || i.which;
                                        if (40 === n || 38 === n) {
                                            var o = e(".search.platform li:visible"),
                                                s = o.index(e("li.selected")) + (n - 39); - 1 == s ? s += o.length : s == o.length && (s = 0), e(".search.platform li.selected").removeClass("selected"), e(o[s]).addClass("selected")
                                        } else if (13 === n) e(".search.platform li.selected:visible > button").click();
                                        else {
                                            var r;
                                            if (a) {
                                                var c = new RegExp("(?=.*" + a.split(/\s+/).join(")(?=.*") + ").*", "i");
                                                r = t.platforms.filter(function(e) {
                                                    return c.test("" + e.description.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase())
                                                }).slice(0, 10)
                                            } else r = t.platforms;
                                            e(".platform-item").remove(), e(".search.platform").append(r.slice(0, 10).reduce(function(e, t) {
                                                return e.concat(f(t, a))
                                            }, []))
                                        }
                                    })), e(document).off("click.platformGo"), e(document).on("click.platformGo", ".platformGo", function(i) {
                                        Object(u.a)(), i.preventDefault();
                                        var a = (e(i.target).hasClass("platformGo") ? e(i.target) : e(i.target).closest(".platformGo")).data("platform");
                                        e.ajax({
                                            url: t.postFormUrl || t.loginUrl,
                                            type: "POST",
                                            data: "PlatformId=" + a,
                                            success: p,
                                            error: function(e, t) {
                                                Object(d.b)(t), Object(u.b)()
                                            }
                                        })
                                    })
                                }
                            }
                        })
                    }(t.platformsUrl), b.unshift(Object(r.b)("li", {
                        class: "flex-0",
                        id: "platforms-menu"
                    })), t.widgets) {
                    var v = t.widgets;
                    if (v.conpass && !window.Conpass) {
                        var g = function() {
                            if (window.Conpass) {
                                var e = Object(r.a)("mpndata"),
                                    i = {
                                        name: t.name || e.UserName,
                                        email: t.email || e.UserEmail
                                    };
                                e.TrialUntil && (i.custom_fields = {
                                    dias: a(e.TrialUntil, "YYYY-MM-DD").diff(Date.now(), "days")
                                }), window.Conpass.init(i)
                            } else setTimeout(g, 300)
                        };
                        Object(r.g)("https://fast.conpass.io/rkg4qzhRtG.js"), g()
                    }
                    if (v.droz) {
                        var y = document.createElement("script");
                        y.innerHTML = "var prechat_inputs = [];", document.getElementsByTagName("head")[0].append(y), window.prechat_inputs.name = t.name, window.prechat_inputs.email = t.email,
                            function(e, t, i, a, n, o, s) {
                                n = e.getElementsByTagName("head")[0], (o = e.createElement("script")).async = 1, s = Math.floor(1e6 * Math.random()) + 1, o.src = t + "/v1/droz.js?i=" + a + "&u=" + t + "&v=" + s, n.appendChild(o)
                            }(document, "https://chat-app.meudroz.com", 0, v.droz.key || "e711e6e414e8d5ce01a103fc46805ebacd7c180b")
                    } else v.zendesk && "" != v.zendesk.appTag && (window.zEmbed || function(e, t) {
                        var i, a, n, o, s = [],
                            r = document.createElement("iframe");
                        window.zEmbed = function() {
                            s.push(arguments)
                        }, window.zE = window.zE || window.zEmbed, r.src = "javascript:false", r.title = "", r.role = "presentation", (r.frameElement || r).style.cssText = "display: none", (n = (n = document.getElementsByTagName("script"))[n.length - 1]).parentNode.insertBefore(r, n), o = r.contentWindow.document;
                        try {
                            a = o
                        } catch (e) {
                            i = document.domain, r.src = 'javascript:var d=document.open();d.domain="' + i + '";void(0);', a = o
                        }
                        a.open()._l = function() {
                            var e = this.createElement("script");
                            i && (this.domain = i), e.id = "js-iframe-async", e.async = !0, e.src = "https://assets.zendesk.com/embeddable_framework/main.js", this.t = +new Date, this.zendeskHost = "totvssuporte.zendesk.com", this.zEQueue = s, this.body.appendChild(e)
                        }, a.write('<body onload="document._l();">'), a.close()
                    }(), window.zESettings = {
                        webWidget: {
                            position: {
                                horizontal: "left",
                                vertical: "bottom"
                            },
                            contactForm: {
                                tags: [v.zendesk.appTag]
                            }
                        }
                    }, window.zE && window.zE(function() {
                        window.$zopim(function() {
                            window.$zopim.livechat.set({
                                language: "pt-BR",
                                name: t.name,
                                email: t.email
                            }), window.$zopim.livechat.departments.filter(v.zendesk.appName), window.$zopim.livechat.departments.setVisitorDepartment(v.zendesk.appName)
                        }), window.zE.show()
                    }));
                    if (v.intercom && "" != v.intercom.appId && Object(r.g)("https://widget.intercom.io/widget/" + v.intercom.appId, function() {
                            window.Intercom && window.Intercom("boot", {
                                app_id: v.intercom.appId,
                                email: t.email
                            })
                        }), v.insights && "" != v.insights.key) {
                        var k = window.appInsights || function(e) {
                            function t(e) {
                                i[e] = function() {
                                    var t = arguments;
                                    i.queue.push(function() {
                                        i[e].apply(i, t)
                                    })
                                }
                            }
                            var i = {
                                    config: e
                                },
                                a = document,
                                n = window;
                            setTimeout(function() {
                                var t = a.createElement("script");
                                t.src = e.url || "https://az416426.vo.msecnd.net/scripts/a/ai.0.js", a.getElementsByTagName("script")[0].parentNode.appendChild(t)
                            });
                            try {
                                i.cookie = a.cookie
                            } catch (e) {}
                            i.queue = [];
                            for (var o = ["Event", "Exception", "Metric", "PageView", "Trace", "Dependency"]; o.length;) t("track" + o.pop());
                            if (t("setAuthenticatedUserContext"), t("clearAuthenticatedUserContext"), t("startTrackEvent"), t("stopTrackEvent"), t("startTrackPage"), t("stopTrackPage"), t("flush"), !e.disableExceptionTracking) {
                                t("_" + (o = "onerror"));
                                var s = n[o];
                                n[o] = function(e, t, a, n, r) {
                                    var c = s && s(e, t, a, n, r);
                                    return !0 !== c && i["_" + o](e, t, a, n, r), c
                                }
                            }
                            return i
                        }({
                            instrumentationKey: v.insights.key
                        });
                        window.appInsights = k, k.queue && 0 === k.queue.length && k.trackPageView()
                    }
                }(b.unshift(Object(r.b)("li", {
                    class: "flex-1",
                    id: "user-info"
                }, [Object(r.b)("span", {
                    id: "platform-name"
                }, ""), Object(r.b)("input", {
                    type: "hidden",
                    id: "platform-id"
                }, ""), Object(r.b)("input", {
                    type: "hidden",
                    id: "platform-url"
                }, ""), Object(r.b)("br"), Object(r.b)("small", {
                    id: "user-email"
                }, "")])), e("header").prepend(Object(r.b)("div", {
                    id: "trial-container",
                    class: "top-banner hide"
                }, [Object(r.b)("div", {
                    id: "trial",
                    class: "toast black white-text",
                    style: "top: 0px; opacity: 1;"
                }, [Object(r.b)("span", null, ["Restam ", Object(r.b)("strong", {
                    id: "diastrial"
                }, "0"), " dias de degustação!"]), Object(r.b)("a", {
                    href: "https://store.totvs.com/checkout/bemacash/gestao",
                    target: "_blank",
                    rel: "noreferrer",
                    class: "btn-flat toast-action"
                }, [Object(r.b)("i", {
                    class: "material-icons right"
                }, "shopping_cart"), "Comprar"])])])), b.push(Object(r.b)("li", {
                    class: "flex-0"
                }, [Object(r.b)("a", {
                    href: "javascript:void(0)",
                    class: "right-trigger hide-on-large-only"
                }, [Object(r.b)("i", {
                    class: "material-icons"
                }, "close")])])), e(t.parent || "header").append(Object(r.b)("nav", {
                    class: "top-nav z-depth-0"
                }, [Object(r.b)("div", {
                    class: "nav-wrapper"
                }, [Object(r.b)("ul", {
                    class: "left-buttons"
                }, _), Object(r.b)("a", {
                    id: "page-title",
                    class: "page-title"
                }, t.appName || ""), Object(r.b)("a", {
                    id: "rightTrigger",
                    href: "javascript:void(0)",
                    class: "right-trigger hide-on-large-only"
                }, [Object(r.b)("i", {
                    class: "material-icons"
                }, "chevron_left")]), Object(r.b)("ul", {
                    class: "right-buttons"
                }, b)])])), t.images && t.images.main) ? m = new RegExp("https?://.*|/.*..*").test(t.images.main) ? Object(r.b)("img", {
                    src: t.images.main,
                    alt: t.appName
                }) : Object(r.b)("svg", {
                    width: 175,
                    height: 72
                }, Object(r.b)("text", {
                    x: "50%",
                    y: "50%",
                    dy: ".3em",
                    "text-anchor": "middle",
                    class: "logo"
                }, t.images.main)): m = Object(r.b)("picture", null, [Object(r.b)("source", {
                    type: "image/webp",
                    srcset: "https://mpn.azureedge.net/img/logo/175/btcb.webp"
                }), Object(r.b)("source", {
                    type: "image/png",
                    srcset: "https://mpn.azureedge.net/img/logo/175/btcb.png"
                }), Object(r.b)("img", {
                    src: "https://mpn.azureedge.net/img/logo/175/btcb.png",
                    alt: t.appName
                })]);
                e(t.parent || "aside").append(Object(r.b)("ul", {
                    id: "nav-mobile",
                    class: "side-nav fixed",
                    style: "transform: translateX(-100%);"
                },
				// [Object(r.b)("li", {
                    // class: "logo"
                // }, [Object(r.b)("a", {
                    // id: "logo-container",
                    // href: "/",
                    // class: "brand-logo"
                // },
				[m])]), Object(r.b)("li", {
                    class: "input-field"
                }, [Object(r.b)("input", {
                    id: "searchMenu",
                    type: "search"
                }), Object(r.b)("i", {
                    class: "material-icons"
                }, "search"), Object(r.b)("label", {
                    for: "searchMenu",
                    style: "opacity:0"
                }, "Pesquisar")]), Object(r.b)("li", {
                    class: "no-padding"
                }, [Object(r.b)("ul", {
                    class: "side collapsible"
                }, o)]), Object(r.b)("li", {
                    class: "no-padding"
                }, [Object(r.b)("ul", {
                    class: "search collapsible"
                }, s)])])), e(".app.with-tooltip").tooltip({
                    position: "left",
                    delay: 20
                }), e(".button-collapse").sideNav(), e(".dropdown-button").dropdown(), e(".side.collapsible").collapsible(), e(".search.collapsible").hide(), e(".side-nav").length > 0 && new n.a(".side-nav"), e(".appMenuToggler").click(function() {
                    e("#appMenu").slideToggle(600, "easeInOutSine")
                }), e(".right-trigger").click(function() {
                    e(".right-buttons").toggle("slide", function() {
                        return e(".right-buttons").toggleClass("display-flex")
                    })
                }), e(window).on("scroll", function(t) {
                    e(t.currentTarget).scrollTop() > e("nav.top-nav").height() ? e("#appMenu, #headTop, .top-banner, #page-title").addClass("pinned") : e("#appMenu, #headTop, .top-banner, #page-title").removeClass("pinned")
                }), e("input#searchMenu[type=search]").on("blur.searchmenu", function() {
                    e(".side-nav li.selected").removeClass("selected")
                }), e("input#searchMenu[type=search]").on("keyup.searchmenu", function(t) {
                    var i = e(t.currentTarget).val().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase(),
                        a = t.keyCode || t.which;
                    if (40 === a || 38 === a) {
                        var n = e("" != i ? ".search.collapsible li:visible" : ".side.collapsible li:visible"),
                            o = n.index(e("li.selected")) + (a - 39); - 1 == o ? o += n.length : o == n.length && (o = 0), e(".side-nav li.selected").removeClass("selected"), e(n[o]).addClass("selected")
                    } else 13 === a ? e("li.selected:visible > a").click() : e(".side-nav li.selected").removeClass("selected");
                    "" != i ? (e(".side.collapsible").hide(), e(".search.collapsible").show(), e(".search.collapsible li").each(function(t, a) {
                        e(a).toggle(RegExp("(?=.*" + i.split(/\s+/).join(")(?=.*") + ").*", "i").test(e(a).find(".linkGo").text().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase() + " > " + e(a).find(".labeltop").text().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase()))
                    })) : (e(".search.collapsible").hide(), e(".side.collapsible").show())
                }), t.notification && Object(c.a)(t.notification, t.email), i && Object(r.p)(i), Object(u.b)(), e("main").addClass("loaded")
            };
        t.a = function(e, t) {
            "string" == typeof e ? Object(o.a)(h, e, t) : h(e, t)
        }
    }).call(this, i(1), i(10))
}, , , , function(e, t, i) {
    "use strict";
    (function(e) {
        i(54), i(137);
        e.validator && (e.validator.methods.date = e.validator.methods.dateITA, e.validator.setDefaults({
            errorClass: "invalid",
            validClass: "valid",
            ignore: ":disabled, :not(:visible)",
            errorPlacement: function(t, i) {
                var a = e(i).closest("form").find('label[for="' + i.attr("id") + '"]');
                a.attr("data-error", t.text()), a.addClass("active")
            }
        }), e.extend(!0, e.validator.messages, {
            required: "Este campo &eacute; requerido.",
            remote: "Corrija este campo.",
            email: "Digite um e-mail v&aacute;lido.",
            url: "Digite uma URL v&aacute;lida.",
            date: "Digite uma data v&aacute;lida.",
            dateISO: "Digite uma data v&aacute;lida (ISO).",
            number: "Digite um n&uacute;mero v&aacute;lido.",
            digits: "Digite somente d&iacute;gitos.",
            creditcard: "Digite um cart&atilde;o de cr&eacute;dito v&aacute;lido.",
            equalTo: "Digite o mesmo valor novamente.",
            accept: "Digite um valor com uma extens&atilde;o v&aacute;lida.",
            maxlength: e.validator.format("Digite no m&aacute;ximo {0} caracteres."),
            minlength: e.validator.format("Digite no m&iacute;nimo {0} caracteres."),
            rangelength: e.validator.format("Digite entre {0} e {1} caracteres."),
            range: e.validator.format("Digite um valor entre {0} e {1}."),
            max: e.validator.format("Digite um valor menor ou igual a {0}."),
            min: e.validator.format("Digite um valor maior ou igual a {0}.")
        }))
    }).call(this, i(1))
}, , , , , , , , , function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(2),
            n = i(83),
            o = i(102),
            s = i(103),
            r = i(104),
            c = i(105),
            l = i(106),
            d = i(107),
            u = i(108),
            p = i(17),
            f = i(109),
            h = i(0),
            m = i(5),
            b = {
                shortcut: n.a,
                calendar: o.a,
                card: s.a,
                chart: r.a,
                datatable: c.a,
                div: l.a,
                form: d.a,
                tabs: u.a,
                modal: p.a
            };
        t.a = function(t, i, n, o, s) {
            if (Object(h.e)(), t.loginUrl || t.postFormUrl) Object(f.a)(t);
            else {
                Object(h.l)(t.sidebarUrl, n, s), e(":loading").loading("stop");
                var r, c = e.extend(!0, {
                    parent: "main"
                }, t);
                if (c.history && !o) r = i ? (c.history.withParams || c.history.default) + "/" + i : c.history.default, window.location.pathname !== r && (window.history.pushState({
                    urlJson: n,
                    id: i
                }, null, r), window.Conpass && window.Conpass.routeChange());
                if (e(c.parent).html(Object(m.h)(e.extend(!0, {}, c.header), i)), c.urlFunctions) {
                    var l = [];
                    c.header && c.header.buttons && c.header.buttons.map(function(e) {
                        l.push(e.onClickFn)
                    }), c.functions && (l = l.concat(c.functions)), Object(h.h)(c.urlFunctions, l)
                }
                if (c.content) {
                    var d = Object(h.i)("row");
                    c.content.map(function(t) {
                        d = t.rowId || d;
                        var a = t.parent ? "#" + t.parent : "main > .container > .row#" + d,
                            n = {
                                class: "row",
                                id: d
                            };
                        e(a).length || e("main").append(Object(h.b)("div", {
                            class: "container"
                        }, Object(h.b)("div", n)));
                        var o = e(a),
                            s = t;
                        switch (t.value && "object" == typeof t.value && (s = t.value), t.type.toLowerCase()) {
                            case "wizard":
                            case "form":
                                b.form(s, i, o);
                                break;
                            case "app":
                            case "shortcut":
                                b.shortcut(s, o);
                                break;
                            case "modal":
                                b.modal(s, i, o);
                                break;
                            default:
                                "function" == typeof b[t.type.toLowerCase()] && b[t.type.toLowerCase()](s, o)
                        }
                    })
                }
                e(".dropdown-button").dropdown(), Object(a.b)(c.readyFn)
            }
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            name: e.name || e.id
        };
        return e.index && (t["data-index"] = e.index), e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.constrainWidth && (t.constrainWidth = e.constrainWidth), Object(a.a)(e.id, e.class + " visible", [Object(n.b)("select", t, e.options.reduce(function(e, t) {
            var i = {
                value: t.value || ""
            };
            return t.selected && (i.selected = "selected"), e.concat(Object(n.b)("option", i, t.label || ""))
        }, [])), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(0);
    t.a = function(e) {
        var t = {
            href: e.link || "javascript:void(0)"
        };
        return e.link && e.link.indexOf("://") > 0 && (t.rel = "nofollow noreferrer", t.target = "_blank"), e.onClick && (t.onclick = e.onClick), e.class && (t.class = e.class), e["data-go"] && (t["data-go"] = e["data-go"]), Object(a.b)("a", t, e.label)
    }
}, , , , , , , function(e, t, i) {
    /*!
     * pickadate.js v3.5.0, 2014/04/13
     * By Amsul, http://amsul.ca
     * Hosted on http://amsul.github.io/pickadate.js
     * Licensed under MIT
     */
    e.exports = function(e) {
        var t = e(window),
            i = e(document),
            a = e(document.documentElement),
            n = function t(n, r, l, d) {
                if (!n) return t;
                var u = !1,
                    p = {
                        id: n.id || "P" + Math.abs(~~(Math.random() * new Date))
                    },
                    f = l ? e.extend(!0, {}, l.defaults, d) : d || {},
                    h = e.extend({}, t.klasses(), f.klass),
                    m = e(n),
                    b = function() {
                        return this.start()
                    },
                    _ = b.prototype = {
                        constructor: b,
                        $node: m,
                        start: function() {
                            return p && p.start ? _ : (p.methods = {}, p.start = !0, p.open = !1, p.type = n.type, n.autofocus = n == c(), n.readOnly = !f.editable, n.id = n.id || p.id, "text" != n.type && (n.type = "text"), _.component = new l(_, f), _.$root = e(t._.node("div", v(), h.picker, 'id="' + n.id + '_root" tabindex="0"')), _.$root.on({
                                keydown: g,
                                focusin: function(e) {
                                    _.$root.removeClass(h.focused), e.stopPropagation()
                                },
                                "mousedown click": function(t) {
                                    var i = t.target;
                                    i != _.$root.children()[0] && (t.stopPropagation(), "mousedown" != t.type || e(i).is("input, select, textarea, button, option") || (t.preventDefault(), _.$root.eq(0).focus()))
                                }
                            }).on({
                                focus: function() {
                                    m.addClass(h.target)
                                },
                                blur: function() {
                                    m.removeClass(h.target)
                                }
                            }).on("focus.toOpen", y).on("click", "[data-pick], [data-nav], [data-clear], [data-close]", function() {
                                var t = e(this),
                                    i = t.data(),
                                    a = t.hasClass(h.navDisabled) || t.hasClass(h.disabled),
                                    n = c();
                                n = n && (n.type || n.href) && n, (a || n && !e.contains(_.$root[0], n)) && _.$root.eq(0).focus(), !a && i.nav ? _.set("highlight", _.component.item.highlight, {
                                    nav: i.nav
                                }) : !a && "pick" in i ? (_.set("select", i.pick), f.closeOnSelect && _.close(!0)) : i.clear ? (_.clear(), f.closeOnSelect && _.close(!0)) : i.close && _.close(!0)
                            }), s(_.$root[0], "hidden", !0), f.formatSubmit && function() {
                                var t;
                                !0 === f.hiddenName ? (t = n.name, n.name = "") : t = (t = ["string" == typeof f.hiddenPrefix ? f.hiddenPrefix : "", "string" == typeof f.hiddenSuffix ? f.hiddenSuffix : "_submit"])[0] + n.name + t[1], _._hidden = e('<input type=hidden name="' + t + '"' + (m.data("value") || n.value ? ' value="' + _.get("select", f.formatSubmit) + '"' : "") + ">")[0], m.on("change." + p.id, function() {
                                    _._hidden.value = n.value ? _.get("select", f.formatSubmit) : ""
                                }), f.container ? e(f.container).append(_._hidden) : m.before(_._hidden)
                            }(), m.data(r, _).addClass(h.input).attr("tabindex", -1).val(m.data("value") ? _.get("select", f.format) : n.value), f.editable || m.on("focus." + p.id + " click." + p.id, function(e) {
                                e.preventDefault(), _.$root.eq(0).focus()
                            }).on("keydown." + p.id, g), s(n, {
                                haspopup: !0,
                                expanded: !1,
                                readonly: !1,
                                owns: n.id + "_root"
                            }), f.container ? e(f.container).append(_.$root) : m.before(_.$root), _.on({
                                start: _.component.onStart,
                                render: _.component.onRender,
                                stop: _.component.onStop,
                                open: _.component.onOpen,
                                close: _.component.onClose,
                                set: _.component.onSet
                            }).on({
                                start: f.onStart,
                                render: f.onRender,
                                stop: f.onStop,
                                open: f.onOpen,
                                close: f.onClose,
                                set: f.onSet
                            }), u = function(e) {
                                var t;
                                return e.currentStyle ? t = e.currentStyle.position : window.getComputedStyle && (t = getComputedStyle(e).position), "fixed" == t
                            }(_.$root.children()[0]), n.autofocus && _.open(), _.trigger("start").trigger("render"))
                        },
                        render: function(e) {
                            return e ? _.$root.html(v()) : _.$root.find("." + h.box).html(_.component.nodes(p.open)), _.trigger("render")
                        },
                        stop: function() {
                            return p.start ? (_.close(), _._hidden && _._hidden.parentNode.removeChild(_._hidden), _.$root.remove(), m.removeClass(h.input).removeData(r), setTimeout(function() {
                                m.off("." + p.id)
                            }, 0), n.type = p.type, n.readOnly = !1, _.trigger("stop"), p.methods = {}, p.start = !1, _) : _
                        },
                        open: function(r) {
                            return p.open ? _ : (m.addClass(h.active), s(n, "expanded", !0), setTimeout(function() {
                                _.$root.addClass(h.opened), s(_.$root[0], "hidden", !1)
                            }, 0), !1 !== r && (p.open = !0, u && a.css("overflow", "hidden").css("padding-right", "+=" + o()), _.$root.eq(0).focus(), i.on("click." + p.id + " focusin." + p.id, function(e) {
                                var t = e.target;
                                t != n && t != document && 3 != e.which && _.close(t === _.$root.children()[0])
                            }).on("keydown." + p.id, function(i) {
                                var a = i.keyCode,
                                    n = _.component.key[a],
                                    o = i.target;
                                27 == a ? _.close(!0) : o != _.$root[0] || !n && 13 != a ? e.contains(_.$root[0], o) && 13 == a && (i.preventDefault(), o.click()) : (i.preventDefault(), n ? t._.trigger(_.component.key.go, _, [t._.trigger(n)]) : _.$root.find("." + h.highlighted).hasClass(h.disabled) || (_.set("select", _.component.item.highlight), f.closeOnSelect && _.close(!0)))
                            })), _.trigger("open"))
                        },
                        close: function(e) {
                            return e && (_.$root.off("focus.toOpen").eq(0).focus(), setTimeout(function() {
                                _.$root.on("focus.toOpen", y)
                            }, 0)), m.removeClass(h.active), s(n, "expanded", !1), setTimeout(function() {
                                _.$root.removeClass(h.opened + " " + h.focused), s(_.$root[0], "hidden", !0)
                            }, 0), p.open ? (p.open = !1, u && a.css("overflow", "").css("padding-right", "-=" + o()), i.off("." + p.id), _.trigger("close")) : _
                        },
                        clear: function(e) {
                            return _.set("clear", null, e)
                        },
                        set: function(t, i, a) {
                            var n, o, s = e.isPlainObject(t),
                                r = s ? t : {};
                            if (a = s && e.isPlainObject(i) ? i : a || {}, t) {
                                for (n in s || (r[t] = i), r) o = r[n], n in _.component.item && (void 0 === o && (o = null), _.component.set(n, o, a)), "select" != n && "clear" != n || m.val("clear" == n ? "" : _.get(n, f.format)).trigger("change");
                                _.render()
                            }
                            return a.muted ? _ : _.trigger("set", r)
                        },
                        get: function(e, i) {
                            if (void 0 === e && (e = "value"), null != p[e]) return p[e];
                            if ("valueSubmit" == e) {
                                if (_._hidden) return _._hidden.value;
                                e = "value"
                            }
                            if ("value" == e) return n.value;
                            if (e in _.component.item) {
                                if ("string" == typeof i) {
                                    var a = _.component.get(e);
                                    return a ? t._.trigger(_.component.formats.toString, _.component, [i, a]) : ""
                                }
                                return _.component.get(e)
                            }
                        },
                        on: function(t, i, a) {
                            var n, o, s = e.isPlainObject(t),
                                r = s ? t : {};
                            if (t)
                                for (n in s || (r[t] = i), r) o = r[n], a && (n = "_" + n), p.methods[n] = p.methods[n] || [], p.methods[n].push(o);
                            return _
                        },
                        off: function() {
                            var e, t, i, a = arguments;
                            for (e = 0, i = a.length; e < i; e += 1)(t = a[e]) in p.methods && delete p.methods[t];
                            return _
                        },
                        trigger: function(e, i) {
                            var a = function(e) {
                                var a = p.methods[e];
                                a && a.map(function(e) {
                                    t._.trigger(e, _, [i])
                                })
                            };
                            return a("_" + e), a(e), _
                        }
                    };

                function v() {
                    return t._.node("div", t._.node("div", t._.node("div", t._.node("div", _.component.nodes(p.open), h.box), h.wrap), h.frame), h.holder)
                }

                function g(e) {
                    var t = e.keyCode,
                        i = /^(8|46)$/.test(t);
                    if (27 == t) return _.close(), !1;
                    (32 == t || i || !p.open && _.component.key[t]) && (e.preventDefault(), e.stopPropagation(), i ? _.clear().close() : _.open())
                }

                function y(e) {
                    e.stopPropagation(), "focus" == e.type && _.$root.addClass(h.focused), _.open()
                }
                return new b
            };

        function o() {
            if (a.height() <= t.height()) return 0;
            var i = e('<div style="visibility:hidden;width:100px" />').appendTo("body"),
                n = i[0].offsetWidth;
            i.css("overflow", "scroll");
            var o = e('<div style="width:100%" />').appendTo(i),
                s = o[0].offsetWidth;
            return i.remove(), n - s
        }

        function s(t, i, a) {
            if (e.isPlainObject(i))
                for (var n in i) r(t, n, i[n]);
            else r(t, i, a)
        }

        function r(e, t, i) {
            e.setAttribute(("role" == t ? "" : "aria-") + t, i)
        }

        function c() {
            try {
                return document.activeElement
            } catch (e) {}
        }
        return n.klasses = function(e) {
            return void 0 === e && (e = "picker"), {
                picker: e,
                opened: e + "--opened",
                focused: e + "--focused",
                input: e + "__input",
                active: e + "__input--active",
                target: e + "__input--target",
                holder: e + "__holder",
                frame: e + "__frame",
                wrap: e + "__wrap",
                box: e + "__box"
            }
        }, n.extend = function(t, i) {
            e.fn[t] = function(a, o) {
                var s = this.data(t);
                return "picker" == a ? s : s && "string" == typeof a ? n._.trigger(s[a], s, [o]) : this.each(function() {
                    e(this).data(t) || new n(this, t, i, a)
                })
            }, e.fn[t].defaults = i.defaults
        }, n._ = {
            group: function(e) {
                for (var t, i = "", a = n._.trigger(e.min, e); a <= n._.trigger(e.max, e, [a]); a += e.i) t = n._.trigger(e.item, e, [a]), i += n._.node(e.node, t[0], t[1], t[2]);
                return i
            },
            node: function(t, i, a, n) {
                return i ? (i = e.isArray(i) ? i.join("") : i, "<" + t + (a = a ? ' class="' + a + '"' : "") + (n = n ? " " + n : "") + ">" + i + "</" + t + ">") : ""
            },
            lead: function(e) {
                return (e < 10 ? "0" : "") + e
            },
            trigger: function(e, t, i) {
                return "function" == typeof e ? e.apply(t, i || []) : e
            },
            digits: function(e) {
                return /\d/.test(e[1]) ? 2 : 1
            },
            isDate: function(e) {
                return {}.toString.call(e).includes("Date") && this.isInteger(e.getDate())
            },
            isInteger: function(e) {
                return {}.toString.call(e).includes("Number") && e % 1 == 0
            },
            ariaAttr: function(t, i) {
                for (var a in e.isPlainObject(t) || (t = {
                        attribute: i
                    }), i = "", t) {
                    var n = ("role" == a ? "" : "aria-") + a,
                        o = t[a];
                    i += null == o ? "" : n + '="' + t[a] + '"'
                }
                return i
            }
        }, n
    }(i(1))
}, , function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(0),
            a = i(14);
        e(document).ready(function() {
            var i = "input[type=text], input[type=password], input[type=email], input[type=url], input[type=tel], input[type=number], input[type=search], textarea";

            function n(e) {
                var t = void 0 !== e.attr("data-length"),
                    i = parseInt(e.attr("data-length")),
                    a = e.val().length;
                0 !== e.val().length || !1 !== e[0].validity.badInput || e.is(":required") ? e.hasClass("validate") && (e.is(":valid") && t && a <= i || e.is(":valid") && !t ? (e.removeClass("invalid"), e.addClass("valid")) : (e.removeClass("valid"), e.addClass("invalid"))) : e.hasClass("validate") && (e.removeClass("valid"), e.removeClass("invalid"))
            }
            e(document).on("change", i, function(t) {
                var i = e(t.currentTarget);
                i.hasClass("select-dropdown") && (i = e(t.currentTarget).parent()), 0 === e(t.currentTarget).val().length && void 0 === e(t.currentTarget).attr("placeholder") || i.siblings("label").addClass("active"), n(e(t.currentTarget))
            }), e(document).ready(function() {
                Object(t.r)()
            }), e(document).on("reset", function(t) {
                var a = e(t.target);
                a.is("form") && (a.find(i).removeClass("valid").removeClass("invalid"), a.find(i).each(function(t, i) {
                    var a = e(i);
                    a.hasClass("select-dropdown") && (a = e(i).parent()), "" === e(i).attr("value") && a.siblings("label").removeClass("active")
                }), a.find("select.initialized").each(function() {
                    var e = a.find("option[selected]").text();
                    a.siblings("input.select-dropdown").val(e)
                }))
            }), e(document).on("focus", i, function(t) {
                var i = e(t.currentTarget);
                i.hasClass("select-dropdown") && (i = e(t.currentTarget).parent()), i.siblings("label, .prefix").addClass("active")
            }), e(document).on("blur", i, function(t) {
                var i = e(t.currentTarget),
                    a = ".prefix";
                0 !== e(t.currentTarget).val().length && ("object" != typeof e(t.currentTarget).val() || e(t.currentTarget).val().isValid()) || !0 === e(t.currentTarget)[0].validity.badInput || void 0 !== e(t.currentTarget).attr("placeholder") || (a += ", label"), n(e(t.currentTarget)), i.siblings(a).removeClass("active")
            });
            e(document).on("keyup.radio", "input[type=radio], input[type=checkbox]", function(t) {
                if (9 === t.which) return e(t.currentTarget).addClass("tabbed"), void e(t.currentTarget).one("blur", function() {
                    e(t.currentTarget).removeClass("tabbed")
                })
            });
            var o = e(".hiddendiv").first();
            o.length || (o = e('<div class="hiddendiv common"></div>'), e("body").append(o));
            e(".materialize-textarea").each(function(t, i) {
                var a = e(i);
                a.data("original-height", a.height()), a.data("previous-length", a.val().length)
            }), e("body").on("keyup keydown autoresize", ".materialize-textarea", function(t) {
                ! function(t) {
                    var i = t.css("font-family"),
                        a = t.css("font-size"),
                        n = t.css("line-height"),
                        s = t.css("padding");
                    a && o.css("font-size", a), i && o.css("font-family", i), n && o.css("line-height", n), s && o.css("padding", s), t.data("original-height") || t.data("original-height", t.height()), "off" === t.attr("wrap") && o.css("overflow-wrap", "normal").css("white-space", "pre"), o.text(t.val() + "\n");
                    var r = o.html().replace(/\n/g, "<br>");
                    o.html(r), t.is(":visible") ? o.css("width", t.width()) : o.css("width", e(window).width() / 2), t.data("original-height") <= o.height() ? t.css("height", o.height()) : t.val().length < t.data("previous-length") && t.css("height", t.data("original-height")), t.data("previous-length", t.val().length)
                }(e(t.currentTarget))
            }), e(document).on("change", '.file-field input[type="file"]', function(t) {
                for (var i = e(t.currentTarget).closest(".file-field"), a = i.find("input.file-path"), n = i.find("label"), o = e(t.currentTarget)[0].files, s = [], r = 0; r < o.length; r++) s.push(o[r].name);
                n.toggleClass("active", s.length > 0), a.val(s.join(", ")), a.trigger("change")
            }), e(document).on("click.picker-control", ".picker-control", function(t) {
                var i = e(t.currentTarget);
                i.hasClass("next") && a[i.data("id")].next(t), i.hasClass("prev") && a[i.data("id")].prev(t), i.hasClass("open") && a[i.data("id")].open(t)
            })
        }), e.fn.serializeForm = function() {
            var t = e(this),
                i = t.serializeArray();
            return i.concat(t.find("input[type=checkbox]:not(:checked)").map(function(t, i) {
                return {
                    name: e(i).attr("name"),
                    value: !1
                }
            }).get()), t.find("input[type=file]").map(function(t, a) {
                return i.push({
                    name: e(a).attr("id"),
                    value: e("#" + e(a).attr("id") + "Thumb").attr("src")
                })
            }), i
        }, e.fn.material_select = function(t) {
            function i(e, t, i) {
                var a = e.indexOf(t),
                    n = -1 === a;
                return n ? e.push(t) : e.splice(a, 1), i.siblings("ul.dropdown-content").find("li:not(.optgroup)").eq(t).toggleClass("active"), i.find("option").eq(t).prop("selected", n),
                    function(e, t) {
                        for (var i = "", a = 0, n = e.length; a < n; a++) {
                            var o = t.find("option").eq(e[a]).text();
                            i += 0 === a ? o : ", " + o
                        }
                        "" === i && (i = t.find("option:disabled").eq(0).text());
                        t.siblings("input.select-dropdown").val(i)
                    }(e, i), n
            }
            e(this).each(function() {
                var a = e(this);
                if (!a.hasClass("browser-default")) {
                    var n = !!a.attr("multiple"),
                        o = a.attr("data-select-id");
                    if (o && (a.parent().find("span.caret").remove(), a.parent().find("input").remove(), a.unwrap(), e("ul#select-options-" + o).remove()), "destroy" === t) return a.removeAttr("data-select-id").removeClass("initialized"), void e(window).off("click.select");
                    var s = Math.random().toString(36).substr(2, 7);
                    a.attr("data-select-id", s);
                    var r = e('<div class="select-wrapper"></div>');
                    r.addClass(a.attr("class")), a.is(":disabled") && r.addClass("disabled");
                    var c = e('<ul id="select-options-' + s + '" class="dropdown-content select-dropdown ' + (n ? "multiple-select-dropdown" : "") + '"></ul>'),
                        l = a.children("option, optgroup"),
                        d = [],
                        u = !1,
                        p = a.find("option:selected").html() || a.find("option:first").html() || "",
                        f = function(t, i, a) {
                            var o = i.is(":disabled") ? "disabled " : "",
                                s = "optgroup-option" === a ? "optgroup-option " : "",
                                r = n ? '<input type="checkbox"' + o + "/><label></label>" : "",
                                l = i.data("icon"),
                                d = i.attr("class");
                            if (l) {
                                var u = "";
                                return d && (u = ' class="' + d + '"'), c.append(e('<li class="' + o + s + '"><img alt="" src="' + l + '"' + u + "><span>" + r + i.html() + "</span></li>")), !0
                            }
                            c.append(e('<li class="' + o + s + '"><span>' + r + i.html() + "</span></li>"))
                        };
                    l.length && l.each(function() {
                        if (e(this).is("option")) n ? f(0, e(this), "multiple") : f(0, e(this));
                        else if (e(this).is("optgroup")) {
                            var t = e(this).children("option");
                            c.append(e('<li class="optgroup"><span>' + e(this).attr("label") + "</span></li>")), t.each(function() {
                                f(0, e(this), "optgroup-option")
                            })
                        }
                    }), c.find("li:not(.optgroup)").each(function(o) {
                        e(this).click(function(s) {
                            if (!e(this).hasClass("disabled") && !e(this).hasClass("optgroup")) {
                                var r = !0;
                                n ? (e('input[type="checkbox"]', this).prop("checked", function(e, t) {
                                    return !t
                                }), r = i(d, o, a), b.trigger("focus")) : (c.find("li").removeClass("active"), e(this).toggleClass("active"), b.val(e(this).text()), "" === e(this).text() && e(this).parent().parent().siblings("label").removeClass("active")), _(c, e(this)), a.find("option").eq(o).prop("selected", r), a.trigger("change"), void 0 !== t && t()
                            }
                            s.stopPropagation()
                        })
                    }), a.wrap(r);
                    var h = e('<span class="material-icons caret">arrow_drop_down</span>'),
                        m = p.replace(/"/g, "&quot;"),
                        b = e('<input type="text" class="select-dropdown" readonly="true" ' + (a.is(":disabled") ? "disabled" : "") + ' data-activates="select-options-' + s + '" value="' + m + '" data-constrainwidth="' + a.attr("constrainWidth") + '"/>');
                    a.before(b), b.before(h), b.after(c), a.is(":disabled") || b.dropdown(), a.attr("tabindex") && e(b[0]).attr("tabindex", a.attr("tabindex")), a.addClass("initialized"), b.on({
                        focus: function() {
                            if (e("ul.select-dropdown").not(c[0]).is(":visible") && (e("input.select-dropdown").trigger("close"), e(window).off("click.select")), !c.is(":visible")) {
                                e(this).trigger("open", ["focus"]);
                                var t = e(this).val();
                                n && t.includes(",") && (t = t.split(",")[0]);
                                var i = c.find("li").filter(function() {
                                    return e(this).text().toLowerCase() === t.toLowerCase()
                                })[0];
                                _(c, i, !0), e(window).off("click.select").on("click.select", function() {
                                    n && (u || b.trigger("close")), e(window).off("click.select")
                                })
                            }
                        },
                        click: function(e) {
                            e.stopPropagation()
                        }
                    }), b.on("blur", function() {
                        n || (e(this).trigger("close"), e(window).off("click.select")), c.find("li.selected").removeClass("selected")
                    }), c.hover(function() {
                        u = !0
                    }, function() {
                        u = !1
                    }), n && a.find("option:selected:not(:disabled)").each(function() {
                        var e = this.index;
                        i(d, e, a), c.find("li:not(.optgroup)").eq(e).find(":checkbox").prop("checked", !0)
                    });
                    var _ = function(t, i, a) {
                            if (i) {
                                t.find("li.selected").removeClass("selected");
                                var o = e(i);
                                o.addClass("selected"), n && !a || c.scrollTo(o)
                            }
                        },
                        v = [];
                    b.on("keydown", function(t) {
                        if (9 != t.which)
                            if (40 != t.which || c.is(":visible")) {
                                if (13 != t.which || c.is(":visible")) {
                                    t.preventDefault();
                                    var i = String.fromCharCode(t.which).toLowerCase();
                                    if (i && ![9, 13, 27, 38, 40].includes(t.which)) {
                                        v.push(i);
                                        var a = v.join(""),
                                            o = c.find("li").filter(function() {
                                                return 0 === e(this).text().toLowerCase().indexOf(a)
                                            })[0];
                                        o && _(c, o)
                                    }
                                    if (13 == t.which) {
                                        var s = c.find("li.selected:not(.disabled)")[0];
                                        s && (e(s).trigger("click"), n || b.trigger("close"))
                                    }
                                    40 == t.which && (o = c.find("li.selected").length ? c.find("li.selected").next("li:not(.disabled)")[0] : c.find("li:not(.disabled)")[0], _(c, o)), 27 == t.which && b.trigger("close"), 38 == t.which && (o = c.find("li.selected").prev("li:not(.disabled)")[0]) && _(c, o), setTimeout(function() {
                                        v = []
                                    }, 1e3)
                                }
                            } else b.trigger("open");
                        else b.trigger("close")
                    })
                }
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(11),
            a = i.n(t),
            n = {
                opacity: .5,
                inDuration: 300,
                outDuration: 200,
                ready: void 0,
                complete: void 0,
                dismissible: !1,
                startingTop: "4%",
                endingTop: "7%"
            },
            o = function t(i, a) {
                i[0].M_Modal && i[0].M_Modal.destroy(), this.$el = i, this.options = e.extend({}, t.defaults, a), this.isOpen = !1, this.$el[0].M_Modal = this, this.id = i.attr("id"), this.openingTrigger = void 0, this.$overlay = e('<div class="modal-overlay"></div>'), t._increment++, t._count++, this.$overlay[0].style.zIndex = 1e3 + 2 * t._increment, this.$el[0].style.zIndex = 1e3 + 2 * t._increment + 1, this.setupEventHandlers()
            },
            s = {
                defaults: {
                    configurable: !0
                }
            };
        s.defaults.get = function() {
            return n
        }, o.init = function(t, i) {
            var a = [];
            return t.each(function() {
                a.push(new o(e(this), i))
            }), a
        }, o.prototype.getInstance = function() {
            return this
        }, o.prototype.destroy = function() {
            this.removeEventHandlers(), this.$el[0].removeAttribute("style"), this.$overlay[0].parentNode && this.$overlay[0].parentNode.removeChild(this.$overlay[0]), this.$el[0].M_Modal = void 0, o._count--
        }, o.prototype.setupEventHandlers = function() {
            this.handleOverlayClickBound = this.handleOverlayClick.bind(this), this.handleModalCloseClickBound = this.handleModalCloseClick.bind(this), 1 === o._count && document.body.addEventListener("click", this.handleTriggerClick), this.$overlay[0].addEventListener("click", this.handleOverlayClickBound), this.$el[0].addEventListener("click", this.handleModalCloseClickBound)
        }, o.prototype.removeEventHandlers = function() {
            0 === o._count && document.body.removeEventListener("click", this.handleTriggerClick), this.$overlay[0].removeEventListener("click", this.handleOverlayClickBound), this.$el[0].removeEventListener("click", this.handleModalCloseClickBound)
        }, o.prototype.handleTriggerClick = function(t) {
            var i = e(t.target).closest(".modal-trigger");
            if (t.target && i.length) {
                var a = i[0].getAttribute("href");
                a = a ? a.slice(1) : i[0].getAttribute("data-target");
                var n = document.getElementById(a).M_Modal;
                n && n.open(i), t.preventDefault()
            }
        }, o.prototype.handleOverlayClick = function() {
            this.options.dismissible && this.close()
        }, o.prototype.handleModalCloseClick = function(t) {
            var i = e(t.target).closest(".modal-close");
            t.target && i.length && this.close()
        }, o.prototype.handleKeydown = function(e) {
            27 === e.keyCode && this.options.dismissible && this.close()
        }, o.prototype.animateIn = function() {
            var t = this;
            e.extend(this.$el[0].style, {
                display: "block",
                opacity: 0
            }), e.extend(this.$overlay[0].style, {
                display: "block",
                opacity: 0
            }), a()(this.$overlay[0], {
                opacity: this.options.opacity
            }, {
                duration: this.options.inDuration,
                queue: !1,
                ease: "easeOutCubic"
            });
            var i = {
                duration: this.options.inDuration,
                queue: !1,
                ease: "easeOutCubic",
                complete: function() {
                    "function" == typeof t.options.ready && t.options.ready.call(t, t.$el, t.openingTrigger)
                }
            };
            this.$el[0].classList.contains("bottom-sheet") || this.$el[0].classList.contains("bottom-helper") ? a()(this.$el[0], {
                bottom: 0,
                opacity: 1
            }, i) : (a.a.hook(this.$el[0], "scaleX", .7), this.$el[0].style.top = this.options.startingTop, a()(this.$el[0], {
                top: this.options.endingTop,
                opacity: 1,
                scaleX: 1
            }, i))
        }, o.prototype.animateOut = function() {
            var e = this;
            a()(this.$overlay[0], {
                opacity: 0
            }, {
                duration: this.options.outDuration,
                queue: !1,
                ease: "easeOutQuart"
            });
            var t = {
                duration: this.options.outDuration,
                queue: !1,
                ease: "easeOutCubic",
                complete: function() {
                    e.$el[0].style.display = "none", "function" == typeof e.options.complete && e.options.complete.call(e, e.$el), e.$overlay[0].parentNode.removeChild(e.$overlay[0])
                }
            };
            this.$el[0].classList.contains("bottom-sheet") || this.$el[0].classList.contains("bottom-helper") ? a()(this.$el[0], {
                bottom: "-100%",
                opacity: 0
            }, t) : a()(this.$el[0], {
                top: this.options.startingTop,
                opacity: 0,
                scaleX: .7
            }, t)
        }, o.prototype.open = function(e) {
            if (!this.isOpen) {
                this.isOpen = !0;
                var t = document.body;
                return t.style.overflow = "hidden", this.$el[0].classList.add("open"), t.appendChild(this.$overlay[0]), this.openingTrigger = e || void 0, this.options.dismissible && (this.handleKeydownBound = this.handleKeydown.bind(this), document.addEventListener("keydown", this.handleKeydownBound)), this.animateIn(), this
            }
        }, o.prototype.close = function() {
            if (this.isOpen) return this.isOpen = !1, this.$el[0].classList.remove("open"), document.body.style.overflow = "", this.options.dismissible && document.removeEventListener("keydown", this.handleKeydownBound), this.animateOut(), this
        }, Object.defineProperties(o, s), o._increment = 0, o._count = 0, e.fn.modal = function(t) {
            return o.prototype[t] ? "get" === t.slice(0, 3) ? this.first()[0].M_Modal[t]() : this.each(function() {
                this.M_Modal[t]()
            }) : "object" != typeof t && t ? void e.error("Method " + t + " does not exist on jQuery.modal") : (o.init(this, arguments[0]), this)
        }
    }).call(this, i(1))
}, , , , , , , , , , , , , , , , , , , , , , , , , , function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(11),
            n = i.n(a),
            o = {
                displayLength: 1 / 0,
                inDuration: 300,
                outDuration: 375,
                className: void 0,
                completeCallback: void 0,
                activationPercent: .8
            },
            s = function t(i, a, n, o) {
                if (i) {
                    this.options = {
                        displayLength: a,
                        className: n,
                        completeCallback: o
                    }, this.options = e.extend({}, t.defaults, this.options), this.message = i, this.panning = !1, this.timeRemaining = this.options.displayLength, 0 === t._toasts.length && t._createContainer(), t._toasts.push(this);
                    var s = this.createToast();
                    s.M_Toast = this, this.el = s, this._animateIn(), this.setTimer()
                }
            },
            r = {
                defaults: {
                    configurable: !0
                }
            };
        r.defaults.get = function() {
            return o
        }, s._createContainer = function() {
            var e = document.createElement("div");
            e.setAttribute("id", "toast-container"), e.addEventListener("touchstart", s._onDragStart, {
                passive: !0
            }), e.addEventListener("touchmove", s._onDragMove, {
                passive: !0
            }), e.addEventListener("touchend", s._onDragEnd), e.addEventListener("mousedown", s._onDragStart), document.addEventListener("mousemove", s._onDragMove), document.addEventListener("mouseup", s._onDragEnd), document.body.appendChild(e), s._container = e
        }, s._removeContainer = function() {
            document.removeEventListener("mousemove", s._onDragMove), document.removeEventListener("mouseup", s._onDragEnd), s._container.parentNode.removeChild(s._container), s._container = null
        }, s._onDragStart = function(t) {
            if (t.target && e(t.target).closest(".toast").length) {
                var i = e(t.target).closest(".toast")[0].M_Toast;
                i.panning = !0, s._draggedToast = i, i.el.classList.add("panning"), i.el.style.transition = "", i.startingXPos = s._xPos(t), i.time = Date.now(), i.xPos = s._xPos(t)
            }
        }, s._onDragMove = function(e) {
            if (s._draggedToast) {
                e.preventDefault();
                var t = s._draggedToast;
                t.deltaX = Math.abs(t.xPos - s._xPos(e)), t.xPos = s._xPos(e), t.velocityX = t.deltaX / (Date.now() - t.time), t.time = Date.now();
                var i = t.xPos - t.startingXPos,
                    a = t.el.offsetWidth * t.options.activationPercent;
                t.el.style.transform = "translateX(" + i + "px)", t.el.style.opacity = 1 - Math.abs(i / a)
            }
        }, s._onDragEnd = function() {
            if (s._draggedToast) {
                var e = s._draggedToast;
                e.panning = !1, e.el.classList.remove("panning");
                var t = e.xPos - e.startingXPos,
                    i = e.el.offsetWidth * e.options.activationPercent;
                Math.abs(t) > i || e.velocityX > 1 ? (e.wasSwiped = !0, e.remove()) : (e.el.style.transition = "transform .2s, opacity .2s", e.el.style.transform = "", e.el.style.opacity = ""), s._draggedToast = null
            }
        }, s._xPos = function(e) {
            return e.targetTouches && e.targetTouches.length >= 1 ? e.targetTouches[0].clientX : e.clientX
        }, s.removeAll = function() {
            for (var e in s._toasts) s._toasts[e].remove()
        }, s.prototype.createToast = function() {
            var t = document.createElement("div");
            if (t.classList.add("toast"), this.options.className) {
                var i, a, n = this.options.className.split(" ");
                for (i = 0, a = n.length; i < a; i++) t.classList.add(n[i])
            }
            return ("object" == typeof HTMLElement ? this.message instanceof HTMLElement : this.message && "object" == typeof this.message && null !== this.message && 1 === this.message.nodeType && "string" == typeof this.message.nodeName) ? t.appendChild(this.message) : this.message instanceof e ? e(t).append(this.message) : t.innerHTML = this.message, s._container.appendChild(t), t
        }, s.prototype._animateIn = function() {
            n()(this.el, {
                top: 0,
                opacity: 1
            }, {
                duration: 300,
                easing: "easeOutCubic",
                queue: !1
            })
        }, s.prototype.setTimer = function() {
            var e = this;
            this.timeRemaining !== 1 / 0 && (this.counterInterval = setInterval(function() {
                e.panning || (e.timeRemaining -= 20), e.timeRemaining <= 0 && e.remove()
            }, 20))
        }, s.prototype.remove = function() {
            var e = this;
            window.clearInterval(this.counterInterval);
            var t = this.el.offsetWidth * this.options.activationPercent;
            this.wasSwiped && (this.el.style.transition = "transform .05s, opacity .05s", this.el.style.transform = "translateX(" + t + "px)", this.el.style.opacity = 0), n()(this.el, {
                opacity: 0,
                marginTop: "-40px"
            }, {
                duration: this.options.outDuration,
                easing: "easeOutExpo",
                queue: !1,
                complete: function() {
                    "function" == typeof e.options.completeCallback && e.options.completeCallback(), e.el.parentNode && e.el.parentNode.removeChild(e.el), s._toasts.splice(s._toasts.indexOf(e), 1), 0 === s._toasts.length && s._removeContainer()
                }
            })
        }, Object.defineProperties(s, r), s._toasts = [], s._container = null, s._draggedToast = null, t.a = s
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(2),
            n = i(0),
            o = i(5);
        t.a = function(t, i) {
            var s = e.extend(!0, {}, t);
            i ? i.append(Object(o.a)(s)) : e(s.parent || "main").append(Object(n.b)("div", {
                class: "container"
            }, Object(n.b)("div", {
                class: "row"
            }, Object(o.a)(s)))), e("#" + s.id).on("click", function() {
                return Object(a.a)()
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            name: e.id,
            type: e.type
        };
        return e.dataSrc && (t["data-src"] = e.dataSrc), e.language && (t.class = "language-" + e.language), Object(a.a)(e.id, e.class, [Object(n.b)("pre", t)])
    }
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(4),
            n = i(0);
        e(document).on("click.rating", ".rating", function(t) {
            e(t.currentTarget).val(e(t.target).val())
        }), t.a = function(e) {
            e.stars || (e.stars = 5);
            for (var t = [], i = 0; i < e.stars; i++) t.push(Object(n.b)("input", {
                type: "radio",
                name: e.name || e.id,
                id: "" + e.id + i,
                value: e.stars - i
            })), t.push(Object(n.b)("label", {
                for: "" + e.id + i
            }));
            return Object(a.a)(e.id, e.class, [Object(n.b)("div", {
                id: e.id,
                class: "rating"
            }, t), Object(n.b)("label", {
                class: "active truncate",
                for: e.id
            }, (e.label || "") + (e.required ? " *" : ""))])
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    var a = i(0);
    t.a = function(e) {
        return Object(a.b)("div", {
            id: e.id + "Field",
            class: "" + (e.class || "col s12")
        }, e.lines.reduce(function(e, t) {
            var i = {};
            return t.id && (i.id = t.id), t.class && (i.class = t.class), e.concat(Object(a.b)(t.tag, i, t.text || ""))
        }, []))
    }
}, function(e, t, i) {
    "use strict";
    var a = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: e.type,
            name: e.name || e.id
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.value && (t.value = e.value), Object(a.b)("input", t)
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: e.type,
            name: e.name || e.id,
            autocomplete: "off"
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.value && (t.value = e.value), Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: "password",
            name: e.name || e.id
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.value && (t.value = e.value), Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t, i = {
            id: e.id,
            type: e.type,
            name: e.name || e.id
        };
        if (e.disabled && (i.disabled = "disabled"), e.readonly && (i.readonly = "readonly"), e.value && (i.value = e.value), e.accept && (i.accept = e.accept), e.image) {
            var o = Object(n.b)("img", {
                id: e.id + "Thumb"
            });
            e.value ? (o.src = e.value, o.alt = e.label) : o.removeAttribute("src"), i.accept = i.accept || "image/*", e.class = "" + (e.class || "col s12"), t = [Object(n.b)("label", {
                class: "truncate active",
                for: e.id
            }, (e.label || "") + (e.required ? " *" : "")), Object(n.b)("div", {
                class: "image-field"
            }, [Object(n.b)("i", {
                class: "material-icons big",
                id: e.id + "Icon"
            }, "add_a_photo"), o, Object(n.b)("i", {
                class: "material-icons clear",
                id: e.id + "Clear"
            }, "delete"), Object(n.b)("input", i)])]
        } else e.class = "file-field " + (e.class || "col s12"), t = [Object(n.b)("i", {
            class: "material-icons"
        }, "file_upload"), Object(n.b)("input", i), Object(n.b)("div", {
            class: "file-path-wrapper"
        }, [Object(n.b)("input", {
            class: "file-path validate",
            type: "text"
        })]), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))];
        return Object(a.a)(e.id, e.class, t)
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
                id: e.id,
                name: e.name || e.id
            },
            i = Object(n.b)("i", {
                id: e.id + "Icon",
                class: "material-icons hide",
                style: "left:0px;right:auto;margin-left:0;"
            }, "date_range");
        return e.type ? (t.type = "text", t.class = "mask " + e.type) : t.type = "search", e.index && (t["data-index"] = e.index), e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.value && (t.value = e.value), e.type ? Object(a.a)(e.id, e.class, [Object(n.b)("input", t), "date" === e.type ? i : "", Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))]) : Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(4),
            n = i(0);

        function o(e) {
            var t = getComputedStyle(e.nextElementSibling).backgroundColor,
                i = (e.value - e.min) / (e.max - e.min) * 100;
            e.style.background = "linear-gradient(to right, " + t + " 0%, " + t + " " + i + "%, #c2c0c2 " + i + "%, #c2c0c2 100%)"
        }
        t.a = function(e) {
            var t = {
                id: e.id,
                type: e.type,
                min: e.min || 0,
                max: e.max || 100,
                step: e.step || 1,
                value: parseFloat(e.value) || 100
            };
            return Object(a.a)(e.id, (e.class || "col s12") + " visible", [Object(n.b)("p", {
                class: "range-field"
            }, [Object(n.b)("input", t), Object(n.b)("span", {
                class: "thumb"
            }, [Object(n.b)("span", {
                class: "value"
            })])])])
        }, document.addEventListener("input", function(e) {
            window.chrome && "range" === e.target.type && o(e.target)
        }), new MutationObserver(function(e) {
            e.forEach(function(e) {
                window.chrome && "range" === e.target.type && o(e.target)
            })
        }).observe(document.documentElement, {
            attributes: !0,
            subtree: !0,
            attributeFilter: ["value"]
        });
        var s = "input[type=range]",
            r = !1,
            c = function(e, t) {
                var i = parseInt(e.parent().css("padding-left")),
                    a = -11 - (parseInt(e.css("width")) - 40) / 2 + i + "px";
                t ? e.hasClass("active") ? e.css("margin-left", a) : e.velocity({
                    opacity: "1",
                    top: "0px",
                    marginLeft: a
                }, {
                    duration: 300,
                    easing: "easeOutExpo"
                }) : (e.velocity({
                    opacity: "0",
                    top: "10px",
                    marginLeft: a
                }, {
                    duration: 100
                }), e.removeClass("active"))
            },
            l = function(e) {
                var t = e.width() - 15,
                    i = parseFloat(e.attr("max")),
                    a = parseFloat(e.attr("min"));
                return (parseFloat(e.val()) - a) / (i - a) * t
            },
            d = function(e, t) {
                var i = {
                        style: "decimal"
                    },
                    a = parseFloat(t.val());
                a !== parseInt(t.val()) && (i.minimumFractionDigits = 2, i.maximumFractionDigits = 2), e.find(".value").html(a.toLocaleString("pt-BR", i))
            };
        e(document).on("change", s, function() {
            var t = e(this).siblings(".thumb");
            d(t, e(this)), c(t, !0);
            var i = l(e(this));
            t.addClass("active").css("left", i)
        }), e(document).on("mousedown touchstart", s, function(t) {
            var i = e(this).siblings(".thumb");
            if (d(i, e(this)), r = !0, e(this).addClass("active"), c(i, !0), "input" !== t.type) {
                var a = l(e(this));
                i.addClass("active").css("left", a)
            }
        }), e(document).on("mouseup touchend", ".range-field", function() {
            r = !1, e(this).removeClass("active")
        }), e(document).on("input mousemove touchmove", ".range-field", function() {
            var t = e(this).children(".thumb"),
                i = e(this).find(s);
            if (r) {
                d(t, t.siblings(s)), c(t, !0);
                var a = l(i);
                t.addClass("active").css("left", a)
            }
        }), e(document).on("mouseout touchleave", ".range-field", function() {
            r || c(e(this).children(".thumb"), !1)
        })
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: "text",
            name: e.name || e.id,
            class: "mask " + e.type
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.value ? t.value = "float" == e.type || "currency" == e.type ? e.value.toLocaleString("pt-BR") : e.value : t.value = "currency" == e.type ? "0" : e.value, e.data && e.data.inputmask && (t["data-inputmask"] = e.data.inputmask), e.digits && (t["data-inputmask-digits"] = e.digits), e.allowMinus && (t["data-inputmask-allowMinus"] = e.allowMinus), "currency" === e.type && (t["data-inputmask-prefix"] = "R$ "), Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: "text",
            name: e.name || e.id,
            class: e.type + "picker "
        };
        return "date" == e.type && (t.class += " mask date"), e.disabled && (t.disabled = "disabled"), e.value && (t.value = e.value), Object(a.a)(e.id, e.class, [Object(n.b)("i", {
            "data-id": e.id,
            class: "material-icons picker-control " + (e.disabled ? "disabled" : "open")
        }, "time" == e.type ? "access_time" : "date_range"), Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            type: "text",
            class: "autocomplete"
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.labelId && (t.id = e.labelId, t.name = e.labelName || e.labelId), e.dataUrl && (t["data-url"] = e.dataUrl), e.dataUrlPost && (t["data-url-post"] = e.dataUrlPost), e.dataUrlPostModal && (t["data-url-post-modal"] = e.dataUrlPostModal), e.dataPostField && (t["data-post-field"] = e.dataPostField), e.id && (t["data-target"] = e.id), e.preFilter && (t["data-prefilter"] = e.preFilter), Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.labelId
        }, (e.label || "") + (e.required ? " *" : "")), Object(n.b)("input", {
            type: "hidden",
            name: e.name || e.id,
            id: e.id
        })])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            type: e.type,
            name: e.name || e.id,
            value: e.value || "true"
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.checked && (t.checked = "checked"), Object(a.a)(e.id, e.class, [Object(n.b)("input", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            name: e.name || e.id,
            class: "materialize-textarea"
        };
        return e.disabled && (t.disabled = "disabled"), e.readonly && (t.readonly = "readonly"), e.maxLength && (t["data-length"] = e.maxLength), Object(a.a)(e.id, e.class, [Object(n.b)("textarea", t), Object(n.b)("label", {
            class: "truncate",
            for: e.id
        }, (e.label || "") + (e.required ? " *" : ""))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = {
            id: e.id,
            class: "thin-bordered"
        };
        return Object(a.a)(e.id, e.class, [Object(n.b)("h5", t, e.label || "")])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        return Object(a.a)(e.id, e.class, [Object(n.b)("ul", {
            id: e.id,
            class: "tabs-fixed-width buttongroup"
        }, e.options.reduce(function(t, i) {
            var a = {
                id: i.id,
                name: i.name || i.id,
                href: "javascript:void(0)"
            };
            i.disabled && (a.disabled = "disabled"), i.readonly && (a.readonly = "readonly");
            var o = i.value ? '("' + i.value + '")' : "()";
            return e.onClickFn && (a.onclick = "" + e.onClickFn + o), t.concat(Object(n.b)("li", {
                class: "tab"
            }, [Object(n.b)("a", a, i.label)]))
        }, []))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        var t = new Date;
        if (t = new Date(t.getFullYear(), t.getMonth(), 1), e.selectable) {
            var i = {
                id: e.id,
                type: "text",
                name: e.name || e.id,
                class: "datepicker "
            };
            return Object(a.a)(e.id, e.class, [Object(n.b)("i", {
                "data-id": e.id,
                class: "material-icons picker-control " + (e.disabled ? "disabled" : "open")
            }, "date_range"), Object(n.b)("input", i), Object(n.b)("label", {
                class: "truncate",
                for: e.id
            }, (e.label || "") + (e.required ? " *" : "")), Object(n.b)("input", {
                type: "hidden",
                id: e.inicio || e.id + "Inicio",
                name: e.inicio || i.name + "Inicio"
            }), Object(n.b)("input", {
                type: "hidden",
                id: e.fim || e.id + "Fim",
                name: e.fim || i.name + "Fim"
            })])
        }
        return Object(a.a)(e.id, e.class || "col s12 m6 offset-m3 l4 offset-l4", [Object(n.b)("div", {
            class: "col s2 picker-btn no-margin"
        }, Object(n.b)("button", {
            type: "button",
            class: "btn picker-control prev",
            "data-id": e.id
        }, Object(n.b)("i", {
            class: "material-icons"
        }, "chevron_left"))), Object(n.b)("div", {
            class: "col s8 picker-btn no-margin"
        }, Object(n.b)("div", {
            class: "btn picker-control main"
        }, Object(n.b)("input", {
            id: e.id,
            type: "text",
            name: e.name || e.id,
            class: "btn truncate " + e.type,
            "data-value": e.value || t
        }))), Object(n.b)("div", {
            class: "col s2 picker-btn no-margin"
        }, Object(n.b)("button", {
            type: "button",
            class: "btn picker-control next",
            "data-id": e.id
        }, Object(n.b)("i", {
            class: "material-icons"
        }, "chevron_right")))])
    }
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        return Object(a.a)(e.id, e.class, [Object(n.b)("table", {
            class: "display striped responsive nowrap",
            width: "100%",
            id: e.id
        }, [Object(n.b)("thead", null, [Object(n.b)("tr", {
            id: "columns"
        }, e.options.reduce(function(e, t) {
            return e.concat(Object(n.b)("th", null, t.label || ""))
        }, []))]), Object(n.b)("tfoot", null, [Object(n.b)("tr", null, e.options.reduce(function(e) {
            return e.concat(Object(n.b)("th", null, ""))
        }, []))])])])
    }
}, function(module, __webpack_exports__, __webpack_require__) {
    "use strict";
    (function($) {
        var fullcalendar__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(57),
            fullcalendar__WEBPACK_IMPORTED_MODULE_0___default = __webpack_require__.n(fullcalendar__WEBPACK_IMPORTED_MODULE_0__),
            fullcalendar_dist_locale_pt_br__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(146),
            fullcalendar_dist_locale_pt_br__WEBPACK_IMPORTED_MODULE_1___default = __webpack_require__.n(fullcalendar_dist_locale_pt_br__WEBPACK_IMPORTED_MODULE_1__),
            _toast__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(3),
            _loading__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2),
            _template__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(5),
            _functions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(0),
            _ = __webpack_require__(14);
        __webpack_exports__.a = function(cfg, parentElement) {
            var config = $.extend(!0, {
                parent: "main",
                class: "col s12"
            }, cfg);
            config.header || (config.header = {
                left: "prev,next",
                center: "title",
                right: "today"
            }), parentElement ? parentElement.append(Object(_template__WEBPACK_IMPORTED_MODULE_4__.b)(config)) : $(config.parent || "main").append(Object(_functions__WEBPACK_IMPORTED_MODULE_5__.b)("div", {
                class: "container"
            }, [Object(_functions__WEBPACK_IMPORTED_MODULE_5__.b)("div", {
                class: "row"
            }, [Object(_template__WEBPACK_IMPORTED_MODULE_4__.b)(config)])]));
            var loadFns = [];
            config.callbacks && (config.callbacks.select && (loadFns = loadFns.concat(config.callbacks.select)), config.callbacks.dayClick && (loadFns = loadFns.concat(config.callbacks.dayClick))), Object(_functions__WEBPACK_IMPORTED_MODULE_5__.h)(config.urlFunctions, loadFns);
            var calConfig = {
                header: config.header,
                height: config.options.height || "auto",
                slotLabelFormat: config.options.slotLabelFormat || "HH:mm",
                defaultView: config.options.defaultView || "month",
                editable: config.options.editable || !1,
                handleWindowResize: config.options.handleWindowResize || !1,
                minTime: config.options.minTime || "05:00:01",
                maxTime: config.options.maxTime || "22:00:00",
                displayEventTime: config.options.displayEventTime || !1,
                eventLimit: config.options.eventLimit || !1,
                selectable: config.options.selectable || !1,
                viewRender: function(e, t) {
                    fullCalendarRefresh(e.start.format("YYYY-MM-DD"), e.end.format("YYYY-MM-DD"))
                },
                views: {
                    month: {
                        titleFormat: "MMMM YYYY"
                    },
                    agenda: {
                        titleFormat: "DD MMM YYYY"
                    }
                }
            };
            if (config.callbacks && config.callbacks.select) {
                var cbFunc = config.callbacks.select;
                cbFunc && /^[a-zA-Z]+$/.test(cbFunc) && (calConfig.select = eval("(" + cbFunc + ")"))
            }
            if (config.callbacks && config.callbacks.dayClick) {
                var cbFunc$1 = config.callbacks.dayClick;
                cbFunc$1 && /^[a-zA-Z]+$/.test(cbFunc$1) && (calConfig.dayClick = eval("(" + cbFunc$1 + ")"))
            }

            function fullCalendarRefresh(e, t) {
                Object(_loading__WEBPACK_IMPORTED_MODULE_3__.a)();
                var i = (config.urlData || "").trim();
                i && $.ajax({
                    type: "GET",
                    url: i,
                    data: {
                        initialDate: e,
                        finalDate: t
                    },
                    success: function(e) {
                        e.success ? (_[config.id].fullCalendar("removeEvents"), e.events && _[config.id].fullCalendar("renderEvents", e.events)) : Object(_toast__WEBPACK_IMPORTED_MODULE_2__.a)(e.message, "error"), Object(_loading__WEBPACK_IMPORTED_MODULE_3__.b)()
                    }
                })
            }
            _[config.id] = $("#" + config.id).fullCalendar(calConfig), _[config.id].refresh = fullCalendarRefresh
        }
    }).call(this, __webpack_require__(1))
}, function(e, t, i) {
    "use strict";
    (function(e, a) {
        var n = i(3),
            o = i(0),
            s = i(5),
            r = i(14);
        e(document).on("click.card", ".card", function(t) {
            if (e(this).find("> .card-reveal").length) {
                var i = e(t.target).closest(".card");
                void 0 === i.data("initialOverflow") && i.data("initialOverflow", void 0 === i.css("overflow") ? "" : i.css("overflow")), e(t.target).is(e(".card-reveal .card-title")) || e(t.target).is(e(".card-reveal .card-title i")) ? e(this).find(".card-reveal").velocity({
                    translateY: 0
                }, {
                    duration: 225,
                    queue: !1,
                    easing: "easeInOutQuad",
                    complete: function() {
                        e(this).css({
                            display: "none"
                        }), i.css("overflow", i.data("initialOverflow"))
                    }
                }) : (e(t.target).is(e(".card .activator")) || e(t.target).is(e(".card .activator i"))) && (i.css("overflow", "hidden"), e(this).find(".card-reveal").css({
                    display: "block"
                }).velocity("stop", !1).velocity({
                    translateY: "-100%"
                }, {
                    duration: 300,
                    queue: !1,
                    easing: "easeInOutQuad"
                }))
            }
        }), t.a = function(t, i) {
            var c = e.extend(!0, {}, t);
            i ? i.append(Object(s.c)(c)) : e(c.parent || "main").append(Object(o.b)("div", {
                class: "container"
            }, Object(o.b)("div", {
                class: "row"
            }, Object(s.c)(c)))), c.urlData && e("#" + c.id).loading(), r[c.id] = {}, r[c.id].info = function(t) {
                e("#" + c.id).length && (e("#" + c.id + " h5").html(t || ""), e("#" + c.id).loading("stop"))
            }, r[c.id].setColor = function(e) {
                var t = document.getElementById(c.id);
                t && e && (t.classList.remove(t.getAttribute("data-color")), t.classList.add(e), t.setAttribute("data-color", e))
            }, r[c.id].refresh = function() {
                var t;
                c.urlData && (e("#" + c.id).length && (e("#" + c.id).loading(), c.parameters && (t = c.parameters.reduce(function(t, i) {
                    var n = e("#" + i.id).val();
                    return e("#" + i.id).hasClass("datepicker") && (n = a(e("#" + i.id).val(), "DD/MM/YYYY").format("YYYY-MM-DD")), t.concat("&" + i.id + "=" + n)
                }, "").slice(1)), e.ajax({
                    type: "GET",
                    url: c.urlData + (t ? (c.urlData.indexOf("?") > -1 ? "&" : "?") + t : ""),
                    success: function(t) {
                        e("#" + c.id).loading("stop"), t.success ? (r[c.id].info(t.info), r[c.id].setColor(t.color)) : Object(n.a)(t.message, "error")
                    }
                })))
            }, r[c.id].setColor(c.color), e(document).ready(r[c.id].refresh)
        }
    }).call(this, i(1), i(10))
}, function(e, t, i) {
    "use strict";
    (function(e, a) {
        var n = i(16),
            o = i.n(n),
            s = i(3),
            r = i(0),
            c = i(5),
            l = i(14);
        o.a.pluginService.register({
            afterUpdate: function(e) {
                if (e.config.options.elements.center) {
                    var t = o.a.helpers,
                        i = e.config.options.elements.center,
                        a = o.a.defaults.global,
                        n = e.chart.ctx,
                        s = t.getValueOrDefault(i.fontStyle, a.defaultFontStyle),
                        r = t.getValueOrDefault(i.fontFamily, a.defaultFontFamily);
                    if (i.fontSize) var c = i.fontSize;
                    else {
                        n.save();
                        c = t.getValueOrDefault(i.minFontSize, 1);
                        for (var l = t.getValueOrDefault(i.maxFontSize, 256), d = t.getValueOrDefault(i.maxText, i.text);;) {
                            if (n.font = t.fontString(c, s, r), !(n.measureText(d).width < 2 * e.innerRadius && c < l)) {
                                c -= 1;
                                break
                            }
                            c += 1
                        }
                        n.restore()
                    }
                    e.center = {
                        currency: i.currency,
                        font: t.fontString(c, s, r),
                        fillStyle: t.getValueOrDefault(i.fontColor, a.defaultFontColor)
                    }
                }
            },
            afterDraw: function(e) {
                if (e.center) {
                    var t, i, a, n = e.chart.ctx,
                        o = e,
                        s = 0;
                    for (t = 0, i = (o.data.datasets || []).length; i > t; ++t) {
                        var r;
                        for (a = o.getDatasetMeta(t), r = 0; r < a.data.length; r++) a.data[r].hidden || (s += o.data.datasets[t].data[r])
                    }
                    e.center.currency && (s = s.toLocaleString("pt-BR", {
                        minimumFractionDigits: 2,
                        style: "currency",
                        currency: "BRL"
                    })), n.save(), n.font = e.center.font, n.fillStyle = e.center.fillStyle, n.textAlign = "center", n.textBaseline = "middle";
                    var c = (e.chartArea.left + e.chartArea.right) / 2,
                        l = (e.chartArea.top + e.chartArea.bottom) / 2;
                    n.fillText(s, c, l), n.restore()
                }
            }
        });
        var d = {
            tooltips: {
                callbacks: {
                    label: function(t, i) {
                        var a = i.labels[t.index],
                            n = i.datasets[t.datasetIndex].data[t.index];
                        return i.currency && (n = n.toLocaleString("pt-BR", {
                            minimumFractionDigits: 2,
                            style: "currency",
                            currency: "BRL"
                        })), n = ": " + n, e.isArray(a) ? (a = a.slice())[0] += n : a += n, a
                    }
                }
            }
        };
        o.a.defaults._set("doughnut", d), o.a.defaults._set("pie", d), o.a.defaults._set("bar", {
            tooltips: {
                callbacks: {
                    title: function(e, t) {
                        var i = "";
                        return e.length > 0 && (e[0].xLabel ? i = e[0].xLabel : t.labels.length > 0 && e[0].index < t.labels.length && (i = t.labels[e[0].index])), i
                    },
                    label: function(e, t) {
                        var i = t.datasets[e.datasetIndex].label || "";
                        return t.currency && (e.yLabel = e.yLabel.toLocaleString("pt-BR", {
                            minimumFractionDigits: 2,
                            style: "currency",
                            currency: "BRL"
                        })), i + ": " + e.yLabel
                    }
                },
                mode: "index",
                axis: "y"
            }
        }), t.a = function(t, i) {
            var n = e.extend(!0, {
                type: "bar",
                parent: "main",
                class: "col s12"
            }, t);
            i ? i.append(Object(c.d)(n)) : e(n.parent || "main").append(Object(r.b)("div", {
                class: "container"
            }, Object(r.b)("div", {
                class: "row"
            }, Object(c.d)(n)))), n.options.legend || (n.options.legend = {
                display: !1
            }), l[n.id] = new o.a(n.id, {
                type: n.drawType || "bar",
                options: n.options
            }), l[n.id].refresh = function() {
                if (e("#" + n.id).length) {
                    var t = !0;
                    e("#" + n.id).loading(), n.parameters.filter(function(e) {
                        return e.required
                    }).map(function(i) {
                        "" == (e("#" + i.id).val() || "") && (t = !1)
                    });
                    var i = n.parameters.reduce(function(t, i) {
                        var n = e("#" + i.id).val();
                        return e("#" + i.id).hasClass("datepicker") && (n = a(e("#" + i.id).val(), "DD/MM/YYYY").format("YYYY-MM-DD")), t.concat("&" + i.id + "=" + n)
                    }, "").slice(1);
                    t && e.ajax({
                        type: "GET",
                        url: n.urlData + (n.urlData.indexOf("?") > -1 ? "&" : "?") + i,
                        success: function(t) {
                            t.success ? (l[n.id].data = t, !t.currency || n.drawType && "bar" != n.drawType && "line" != n.drawType || (l[n.id].options.scales.yAxes[0].ticks.userCallback = function(e) {
                                return e.toLocaleString("pt-BR", {
                                    minimumFractionDigits: 2,
                                    style: "currency",
                                    currency: "BRL"
                                })
                            }), l[n.id].update()) : Object(s.a)(t.message, "error"), e("#" + n.id).loading("stop")
                        }
                    })
                }
            }, e(document).ready(l[n.id].refresh)
        }
    }).call(this, i(1), i(10))
}, function(module, __webpack_exports__, __webpack_require__) {
    "use strict";
    (function($, moment) {
        var datatables_net__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(19),
            datatables_net__WEBPACK_IMPORTED_MODULE_0___default = __webpack_require__.n(datatables_net__WEBPACK_IMPORTED_MODULE_0__),
            datatables_net_select__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(192),
            datatables_net_select__WEBPACK_IMPORTED_MODULE_1___default = __webpack_require__.n(datatables_net_select__WEBPACK_IMPORTED_MODULE_1__),
            datatables_net_rowgroup__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(193),
            datatables_net_rowgroup__WEBPACK_IMPORTED_MODULE_2___default = __webpack_require__.n(datatables_net_rowgroup__WEBPACK_IMPORTED_MODULE_2__),
            datatables_net_scroller__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(194),
            datatables_net_scroller__WEBPACK_IMPORTED_MODULE_3___default = __webpack_require__.n(datatables_net_scroller__WEBPACK_IMPORTED_MODULE_3__),
            datatables_net_responsive__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(195),
            datatables_net_responsive__WEBPACK_IMPORTED_MODULE_4___default = __webpack_require__.n(datatables_net_responsive__WEBPACK_IMPORTED_MODULE_4__),
            datatables_net_buttons__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(196),
            datatables_net_buttons__WEBPACK_IMPORTED_MODULE_5___default = __webpack_require__.n(datatables_net_buttons__WEBPACK_IMPORTED_MODULE_5__),
            _toast__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(3),
            _functions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(0),
            _template__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(5),
            _ = __webpack_require__(14);
        $.fn.dataTable && ($.extend(!0, $.fn.dataTable.defaults, {
            lengthChange: !1,
            responsive: !0,
            language: {
                sEmptyTable: "Nenhum registro encontrado",
                sInfo: "",
                sInfoEmpty: "",
                sInfoFiltered: "",
                sInfoPostFix: "",
                sInfoThousands: ".",
                sLengthMenu: "_MENU_ resultados por página",
                sLoadingRecords: "Carregando...",
                sProcessing: "",
                sZeroRecords: "Nenhum registro encontrado",
                sSearch: "Pesquisar",
                oPaginate: {
                    sNext: "&#x203A;",
                    sPrevious: "&#8249;",
                    sLast: "&#x00BB",
                    sFirst: "&#x00AB"
                },
                oAria: {
                    sSortAscending: ": Ordenar colunas de forma ascendente",
                    sSortDescending: ": Ordenar colunas de forma descendente"
                }
            }
        }), $.fn.dataTable.ext.errMode = "none", $.fn.dataTableExt.oApi.hideEmptyColumns = function(e) {
            if (null != e && _[e.sTableId]) {
                if (_[e.sTableId].columns().visible(!0), _[e.sTableId].rows().count() > 0) {
                    var t = _[e.sTableId].rows().count();
                    this.api().columns().every(function() {
                        this.nodes().filter(function(e) {
                            return "" === e.innerHTML
                        }).length == t && _[e.sTableId].column(this.index()).visible(!1, !1)
                    })
                }
                _[e.sTableId].columns.adjust()
            }
        }), __webpack_exports__.a = function(cfg, parentElement) {
            var config = $.extend(!0, {
                parent: "main",
                class: "col s12",
                columns: [],
                options: {}
            }, cfg);
            if ($("#" + config.id).DataTable().destroy(), $.fn.dataTableExt.oStdClasses.sWrapper = config.class, config.options && !config.options.withoutRowMenu && !config.options.withoutMenu && config.actions) {
                var idRender = function(data, type, row, meta) {
                    if ("export" === type) return "";
                    var options = [];
                    return config.actions.map(function(curr) {
                        (void 0 !== curr.showIf && eval("(" + curr.showIf + ")") || void 0 === curr.showIf) && options.push(Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("li", null, [Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("a", {
                            href: "javascript:void(0)",
                            onclick: curr.onClickFn + "('" + row.id + "')"
                        }, curr.label)]))
                    }), options.length > 0 && (options = [Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("a", {
                        class: "fly01-dt-menu right dropdown-button",
                        href: "javascript:void(0)",
                        "data-activates": "drop" + config.id + "down" + meta.row
                    }, [Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("i", {
                        class: "material-icons"
                    }, "more_vert")]), Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("ul", {
                        id: "drop" + config.id + "down" + meta.row,
                        class: "dropdown-content"
                    }, options)]), options.unshift(Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("input", {
                        type: "hidden",
                        class: "rowRecordId",
                        value: row.id
                    })), options.map(function(e) {
                        return e.outerHTML
                    }).join("")
                };
                config.columns.push({
                    renderFn: idRender,
                    searchable: !1,
                    orderable: !1,
                    visible: !0,
                    priority: -1e3
                })
            }
            parentElement ? parentElement.append(Object(_template__WEBPACK_IMPORTED_MODULE_8__.e)(config)) : $(config.parent || "main").append(Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("div", {
                class: "container"
            }, [Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("div", {
                class: "row"
            }, [Object(_template__WEBPACK_IMPORTED_MODULE_8__.e)(config)])]));
            var loadFns = [];
            config.actions && config.actions.map(function(e) {
                loadFns.push(e.onClickFn)
            }), config.columns.filter(function(e) {
                return e.renderFn && !("function" == typeof e.renderFn) && !e.renderFn.includes("{")
            }).map(function(e) {
                loadFns.push(e.renderFn)
            }), config.functions && (loadFns = loadFns.concat(config.functions)), Object(_functions__WEBPACK_IMPORTED_MODULE_7__.h)(config.urlFunctions, loadFns);
            var urlGrid = (config.urlGrid || config.urlData || "").trim();
            if (config.parameters && urlGrid) {
                var getParams = config.parameters.reduce(function(e, t) {
                    var i = $("#" + t.id).val();
                    return $("#" + t.id).hasClass("datepicker") && (i = moment($("#" + t.id).val(), "DD/MM/YYYY").format("YYYY-MM-DD")), e.concat("&" + t.id + "=" + i)
                }, "").slice(1);
                urlGrid = urlGrid + (urlGrid.indexOf("?") > -1 ? "&" : "?") + getParams
            }
            var order = [
                    [config.options.orderColumn || 0, config.options.orderDir || "desc"]
                ],
                serialize = function(e, t) {
                    var i, a = [];
                    for (i in e)
                        if (e.hasOwnProperty(i)) {
                            var n = t ? t + "[" + i + "]" : i,
                                o = e[i];
                            a.push(null !== o && "object" == typeof o ? serialize(o, n) : encodeURIComponent(n) + "=" + encodeURIComponent(o))
                        } return a.join("&")
                },
                callExport = function(e) {
                    if (0 === _[config.id].ajax.json().recordsTotal) Object(_toast__WEBPACK_IMPORTED_MODULE_6__.a)("Não existem registros para exportar", "error");
                    else {
                        $("#" + config.id).loading("start");
                        var t = _[config.id].ajax.url() + "&fileType=" + e + "&" + serialize(_[config.id].ajax.params());
                        $.ajax({
                            url: t,
                            type: "GET",
                            xhrFields: {
                                responseType: "blob"
                            },
                            success: function(t) {
                                var i = document.createElement("a"),
                                    a = window.URL.createObjectURL(t);
                                i.href = a, i.download = $(".fly01-main-title").html() + "." + e, i.click(), window.URL.revokeObjectURL(a), Object(_toast__WEBPACK_IMPORTED_MODULE_6__.a)("Arquivo gerado com sucesso", "success"), $("#" + config.id).loading("stop")
                            },
                            error: function() {
                                $("#" + config.id).loading("stop")
                            }
                        })
                    }
                },
                dtConfig = {
                    ordering: config.columns.filter(function(e) {
                        return e.orderable
                    }).length > 0,
                    pagingType: config.options.pagingType || "simple_numbers",
                    pageLength: config.options.pageLength || 10,
                    order: order,
                    deferRender: !0,
                    lengthChange: config.options.lengthChange || !1,
                    dom: "Brtip",
                    buttons: [{
                        text: "PDF",
                        className: "btn-flat",
                        action: function() {
                            callExport("pdf")
                        }
                    }, {
                        text: "XLS",
                        className: "btn-flat",
                        action: function() {
                            callExport("xls")
                        }
                    }, {
                        text: "DOC",
                        className: "btn-flat",
                        action: function() {
                            callExport("doc")
                        }
                    }, {
                        text: "CSV",
                        className: "btn-flat",
                        action: function() {
                            callExport("csv")
                        }
                    }],
                    searching: config.columns.filter(function(e) {
                        return e.searchable
                    }).length > 0,
                    columns: config.columns.reduce(function(prev, curr, idx) {
                        var obj = {
                            data: curr.dataField || "",
                            name: curr.displayName || "",
                            orderable: curr.orderable || !1,
                            searchable: curr.searchable || !1,
                            visible: curr.visible || !1,
                            responsivePriority: 99 - idx
                        };
                        return curr.type && "currency" == curr.type.toLowerCase() && !curr.class && (curr.class = "dt-right"), curr.class && (obj.className = curr.class), "" !== (curr.renderFn || "") && ("function" == typeof curr.renderFn ? obj.render = curr.renderFn : /^[a-zA-Z]+$/.test(curr.renderFn) ? obj.render = eval("(" + curr.renderFn + ")") : obj.render = function(data, type, full, meta) {
                            return eval("(" + curr.renderFn + ")")
                        }), curr.priority && (obj.responsivePriority = curr.priority), curr.width && (obj.width = curr.width), prev.concat(obj)
                    }, [])
                },
                delayTimer;
            if (urlGrid && (dtConfig.processing = !0, dtConfig.serverSide = !0, dtConfig.ajax = {
                    url: urlGrid,
                    cache: !1,
                    async: !0,
                    dataSrc: function(e) {
                        return !1 === e.success && Object(_toast__WEBPACK_IMPORTED_MODULE_6__.a)(e.message, "error"), e.data
                    }
                }), config.rowGroup && (dtConfig.rowGroup = {
                    dataSrc: config.rowGroup
                }), config.callbacks && config.callbacks.footerCallback) {
                var cbFunc = config.callbacks.footerCallback;
                cbFunc && /^[a-zA-Z]+$/.test(cbFunc) && (dtConfig.footerCallback = eval("(" + cbFunc + ")"))
            }
            if (config.callbacks && config.callbacks.preDrawCallback) {
                var cbFunc$1 = config.callbacks.preDrawCallback;
                cbFunc$1 && /^[a-zA-Z]+$/.test(cbFunc$1) && (dtConfig.preDrawCallback = eval("(" + cbFunc$1 + ")"))
            } else dtConfig.preDrawCallback = function() {
                $("#" + config.id).loading("start");
                var e = !0;
                return config.parameters && config.parameters.map(function(t) {
                    t.required && "" == ($("#" + t.id).val() || "") && (e = !1)
                }), e || $("#" + config.id).loading("stop"), e
            };
            if (config.callbacks && config.callbacks.drawCallback) {
                var cbFunc$2 = config.callbacks.drawCallback;
                cbFunc$2 && /^[a-zA-Z]+$/.test(cbFunc$2) && (dtConfig.drawCallback = eval("(" + cbFunc$2 + ")"))
            } else dtConfig.drawCallback = function(e) {
                this.hideEmptyColumns(this), $("#" + config.id + " .fly01-dt-menu.dropdown-button").dropdown({
                    alignment: "right"
                }), $("#" + config.id + " select").material_select(), e._iDisplayLength >= e.fnRecordsDisplay() ? $(e.nTableWrapper).find(".dataTables_paginate").hide() : $(e.nTableWrapper).find(".dataTables_paginate").show(), Object(_functions__WEBPACK_IMPORTED_MODULE_7__.j)(), Object(_functions__WEBPACK_IMPORTED_MODULE_7__.r)(config.id), $("#" + config.id + " tbody tr").each(function() {
                    $(this).find("td").attr("nowrap", "nowrap")
                }), _[config.id] && _[config.id].columns.adjust(), $("#" + config.id).loading("stop")
            };
            if (config.callbacks && config.callbacks.initComplete) {
                var cbFunc$3 = config.callbacks.initComplete;
                cbFunc$3 && /^[a-zA-Z]+$/.test(cbFunc$3) && (dtConfig.initComplete = eval("(" + cbFunc$3 + ")"))
            } else dtConfig.initComplete = function() {
                $("#" + config.id + " tbody tr").each(function() {
                    $(this).find("td").attr("nowrap", "nowrap")
                }), _[config.id] && _[config.id].columns.adjust()
            };
            if (config.callbacks && config.callbacks.rowCallback) {
                var cbFunc$4 = config.callbacks.rowCallback;
                cbFunc$4 && /^[a-zA-Z]+$/.test(cbFunc$4) && (dtConfig.rowCallback = eval("(" + cbFunc$4 + ")"))
            } else dtConfig.rowCallback = function(e, t) {
                return $("#" + config.id + " tbody tr").each(function() {
                    $(this).find("td").attr("nowrap", "nowrap")
                }), $("#" + config.id).DataTable().responsive.recalc(), config.options.select && -1 !== $.inArray(t.id, _[config.id].selected) && $(e).addClass("selected"), e
            };
            config.options && (void 0 !== config.options.scrollLength && (dtConfig.scroller = !0, dtConfig.scrollY = config.options.scrollLength), config.options.select && (dtConfig.select = $.extend(!0, {
                info: !1
            }, config.options.select)), config.options.noExportButtons && (dtConfig.buttons = [])), _[config.id] = $("#" + config.id).on("error.dt", function() {
                return $("#" + config.id).loading("stop")
            }).DataTable(dtConfig), $("#" + config.id + "_wrapper button.dt-button").length > 0 && ($("#" + config.id + " th.dt-controls:last-child").append(Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("a", {
                class: "dt-controls-activator right dropdown-button",
                href: "javascript:void(0)",
                "data-activates": "dt-controls" + config.id
            }, [Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("i", {
                class: "material-icons"
            }, "file_copy")])), $("#" + config.id + " th.dt-controls:last-child").append(Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("ul", {
                id: "dt-controls" + config.id,
                class: "dropdown-content"
            })), $.each($("#" + config.id + "_wrapper button.dt-button"), function(e, t) {
                var i = Object(_functions__WEBPACK_IMPORTED_MODULE_7__.b)("li");
                $(t).appendTo(i), $(i).appendTo($("#dt-controls" + config.id))
            }), $("#" + config.id + " th.dt-controls .dt-controls-activator.dropdown-button").dropdown({
                alignment: "right"
            })), config.rowGroup && _[config.id].column(config.rowGroup).visible(!1);
            var search = function(e, t, i) {
                clearTimeout(delayTimer), delayTimer = setTimeout(function() {
                    _[config.id].column(e).search(t).draw()
                }, i || 300)
            };
            $(_[config.id].table().container()).on("keyup", "th .fly01dt-filter input:not(.date)", function() {
                search($(this).data("index"), this.value, 300)
            }), $(_[config.id].table().container()).on("keyup", "th .fly01dt-filter input.date", function() {
                if (!(this.value.length < 8 && this.value.length > 0)) {
                    var e = this.value.replace("/", "");
                    moment(e, "DDMMYYYY").isValid() && (e = moment(e, "DDMMYYYY").format("YYYY-MM-DD")), search($(this).data("index"), e, 300)
                }
            }), $("th .fly01dt-filter input.date").on("click", function() {
                var e = $(this).attr("id");
                if (void 0 !== $("#dataInicial").val() && void 0 !== $("#dataFinal").val()) {
                    var t = null,
                        i = !1,
                        a = !1,
                        n = !1,
                        o = null,
                        s = 0,
                        r = 0,
                        c = 0;
                    $("#" + e).pickadate({
                        closeOnSelect: !1,
                        onClose: function() {
                            _[config.id].refresh(), $("#" + e).val(""), i || a || $("#" + e).removeAttr("autofocus"), s = 0
                        },
                        onOpen: function() {
                            if ($("#" + e).removeAttr("autofocus"), o = $("#" + e).pickadate("picker"), "" !== $("#dataInicial").val() && "" !== $("#dataFinal").val()) {
                                var i = moment($("#dataInicial").val()).format("YYYY-MM-DD").replace("-", "").replace("-", ""),
                                    a = moment($("#dataFinal").val()).format("YYYY-MM-DD").replace("-", "").replace("-", "");
                                c = i.substring(0, 4), r = i.substring(4, 6);
                                var s = i.substring(6, 8),
                                    l = a.substring(6, 8),
                                    d = 0;
                                if (new Date(c, r, 0).getDate() == l && o.get("max").date != 1 / 0 && t && (d = o.get("max").date - l), o.get("min").date !== -1 / 0 && o.get("max").date !== 1 / 0 && t) {
                                    o.set("min", new Date(c, r - 1, o.get("min").date));
                                    var u = Math.abs(d);
                                    o.set("max", new Date(c, r - 1, l > o.get("max").date ? l - u : o.get("max").date - u))
                                } else o.set("min", new Date(c, r - 1, s)), o.set("max", new Date(c, r - 1, l))
                            }
                            "" === $("#dataInicial").val() && (n = !0)
                        },
                        onSet: function(l) {
                            if (event.stopPropagation(), event.preventDefault(), l.select) i ? (a = !0, s += 1, o.set("max", new Date(l.select))) : (i = !0, s += 1, o.set("min", new Date(l.select)));
                            else if (null === l.clear) {
                                if (i = !1, a = !1, s = 0, o.set("min", !1), o.set("max", !1), $("#" + e).val(""), $("#" + e).removeAttr("autofocus"), n) n = !1, $("#dataInicial").val(""), $("#dataFinal").val("");
                                else {
                                    var d = new Date(c, r, 0);
                                    $("#dataInicial").val(c + "-" + r + "-1"), $("#dataFinal").val(c + "-" + r + "-" + d.getDate())
                                }
                                o.close()
                            }
                            void 0 === $("#" + e).attr("autofocus") && "" !== $("#" + e).val() && $("#" + e).prop("autofocus", "true"), $("#" + e + "Icon").toggleClass("hide", !$("#" + e).prop("autofocus")), (t = i && a) && ($("#dataInicial").val(o.get("min").date !== -1 / 0 ? o.get("min", "yyyy-mm-dd") : ""), $("#dataFinal").val(o.get("max").date !== 1 / 0 ? o.get("max", "yyyy-mm-dd") : ""), $("#" + e + "_table div.picker__day:not(.picker__day--disabled)").addClass("picker__day--selected"), 2 == s && o.close())
                        }
                    })
                }
            }), $(_[config.id].table().container()).on("change", "th .fly01dt-select select", function() {
                search($(this).data("index"), this.value, 0)
            }), Object(_functions__WEBPACK_IMPORTED_MODULE_7__.j)(), $("#" + config.id + " .fly01dt-select > select").material_select(), $("#" + config.id + " .select-dropdown").addClass("truncate"), $("#" + config.id + " th .input-field.fly01dt-filter").click(function(e) {
                return e.stopPropagation(), !1
            }), _[config.id].on("search.dt", function() {
                $("#" + config.id).loading("start")
            }), config.options && config.options.select && (_[config.id].selected = [], _[config.id].on("select", function(e, t, i, a) {
                void 0 !== i && a.forEach(function(e) {
                    _[config.id].row(e).nodes().to$().addClass("selected"), -1 == $.inArray(_[config.id].row(e).data().id, _[config.id].selected) && _[config.id].selected.push(_[config.id].row(e).data().id)
                })
            }), _[config.id].on("deselect", function(e, t, i, a) {
                a.forEach(function(e) {
                    _[config.id].row(e).nodes().to$().removeClass("selected");
                    var t = _[config.id].selected.indexOf(_[config.id].row(e).data().id); - 1 != t && _[config.id].selected.splice(t, 1)
                })
            }), $("#" + config.id + " tbody").on("click", "tr td", function() {
                var e = _[config.id].row(this.parentElement._DT_RowIndex).data().id; - 1 === $.inArray(e, _[config.id].selected) ? _[config.id].row(this.parentElement._DT_RowIndex).select() : 0 == $(this).find(".input-field").length && _[config.id].row(this.parentElement._DT_RowIndex).deselect()
            })), _[config.id].refresh = function() {
                var e = !0,
                    t = (config.urlGrid || config.urlData || "").trim();
                if (t) {
                    if (config.parameters) {
                        config.parameters.filter(function(e) {
                            return e.required
                        }).map(function(t) {
                            "" == ($("#" + t.id).val() || "") && (e = !1)
                        });
                        var i = config.parameters.reduce(function(e, t) {
                            var i = $("#" + t.id).val();
                            return $("#" + t.id).hasClass("datepicker") && (i = moment($("#" + t.id).val(), "DD/MM/YYYY").format("YYYY-MM-DD")), e.concat("&" + t.id + "=" + i)
                        }, "").slice(1);
                        t = t + (t.indexOf("?") > -1 ? "&" : "?") + i
                    }
                    e && (_[config.id].ajax.url(t), 1 == _[config.id].data().count() ? _[config.id].ajax.reload() : _[config.id].ajax.reload(null, !1))
                }
            }
        }
    }).call(this, __webpack_require__(1), __webpack_require__(10))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(0),
            n = i(5);
        t.a = function(t, i) {
            var o = e.extend(!0, {
                parent: "main"
            }, t);
            i ? i.append(Object(n.f)(o)) : e(o.parent || "main").append(Object(a.b)("div", {
                class: "container"
            }, [Object(a.b)("div", {
                class: "row"
            }, Object(n.f)(o))]));
            var s = [];
            o.readyFn && s.push(o.readyFn), o.elements && o.elements.map(function(e) {
                e.domEvents && e.domEvents.map(function(e) {
                    s.push(e.function)
                }), e.onClickFn && s.push(e.onClickFn)
            }), o.functions && (s = s.concat(o.functions)), Object(a.h)(o.urlFunctions, s), Object(a.f)(o), Object(a.r)(o.id)
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        i(35), i(55), i(197);
        var a = i(2),
            n = i(8),
            o = i(3),
            s = i(5),
            r = i(0),
            c = i(14);
        t.a = function(t, i, l) {
            var d, u = e.extend(!0, {
                parent: "main",
                method: "post"
            }, t);
            u.action && (d = i ? u.action.edit || u.action.create : u.action.create || u.action.edit), l ? l.append(Object(s.g)(u, d)) : e(u.parent).append(Object(r.b)("div", {
                class: "container"
            }, [Object(r.b)("div", {
                class: "row"
            }, Object(s.g)(u, d))]));
            var p = [];
            u.readyFn && p.push(u.readyFn), u.afterLoadFn && p.push(u.afterLoadFn), u.elements && u.elements.map(function(e) {
                e.domEvents && e.domEvents.map(function(e) {
                    p.push(e.function)
                }), e.onClickFn && p.push(e.onClickFn)
            }), u.functions && (p = p.concat(u.functions)), Object(r.h)(u.urlFunctions, p), e("form#" + u.id).on("submit", function(t) {
                var i = event && event.target ? event.target.id : "";
                if (Object(a.a)(), t.preventDefault(), !e(this).valid()) return Object(a.b)(), !1;
                var s = e(this).serializeForm(),
                    l = e(this).validate();
                e.ajax({
                    url: e(this).attr("action"),
                    type: "POST",
                    data: s,
                    success: function(e) {
                        if (Object(a.b)(), e.success) {
                            var t = "Cadastrado com sucesso.";
                            "" !== e.message && (t = e.message), "saveNew" == i && u.action.form ? Object(n.a)(u.action.form) : u.action.list ? Object(n.a)(u.action.list) : e.id && c[u.id].refresh(e.id), Object(o.a)(t, "success")
                        } else Object(r.o)(l, e.message)
                    },
                    error: function(e, t) {
                        Object(o.a)(t, "error"), Object(a.b)()
                    }
                })
            }), u.steps && e(".stepper").activateStepper({
                autoFocusInput: !1
            }), c[u.id] = {}, c[u.id].refresh = function(t) {
                Object(a.a)(), t && u.action && u.action.get ? e.ajax({
                    type: "GET",
                    url: u.action.get + t,
                    success: function(t) {
                        !1 === t.success ? (Object(o.a)(t.message, "error", 8e3), u.action.list && Object(n.a)(u.action.list)) : (Object(r.c)(e("#" + u.id), t, u.afterLoadFn), Object(r.f)(u), Object(r.r)(u.id), Object(a.b)())
                    }
                }) : (Object(r.f)(u), Object(r.r)(u.id), Object(a.b)())
            }, c[u.id].refresh(i)
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        i(198);
        var a = i(5),
            n = i(0);
        t.a = function(t, i) {
            var o = e.extend(!0, {
                class: "col s12"
            }, t);
            i ? i.append(Object(a.k)(o)) : e(o.parent || "main").append(Object(n.b)("div", {
                class: "container"
            }, Object(n.b)("div", {
                class: "row"
            }, Object(a.k)(o)))), e(document).ready(function() {
                e("ul#" + o.id + ".tabs").tabs()
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var a = i(2),
            n = i(0),
            o = i(110),
            s = i.n(o),
            r = i(8),
            c = i(3),
            l = "col s10 m8 offset-m2 offset-s1",
            d = function(t) {
                e("form#login").remove(), e("form#platform").attr("action", t.postFormUrl || t.loginUrl);
                var i = function(e, t) {
                        var i = createElem("span", null, e.description);
                        if (t) {
                            var a = new RegExp("(" + t.replace(/[-/\\^$*+?.()|[\]{}]/g, "\\$&").split(" ").join("|") + ")", "gi");
                            i.innerHTML = e.description.replace(a, '<span class="black-text">$1</span>')
                        }
                        return createElem("li", null, [createElem("button", {
                            class: "btn platformGo truncate",
                            "data-platform": e.id
                        }, [i, createElem("i", {
                            class: "material-icons right"
                        }, "send")])])
                    },
                    a = t.platforms.filter(function(e, t, i) {
                        return i.indexOf(e) == t
                    }).sort(function(e, t) {
                        return e.description = (e.description || "Plataforma não tratada").toUpperCase(), t.description = (t.description || "Plataforma não tratada").toUpperCase(), e.description < t.description ? -1 : e.description > t.description ? 1 : 0
                    }),
                    n = [];
                t.platforms.length > 4 && (n.push(createElem("input", {
                    type: "search",
                    id: "platformSelect",
                    autocomplete: "off"
                })), n.push(createElem("i", {
                    class: "material-icons"
                }, "search")), n.push(createElem("label", {
                    class: "truncate",
                    for: "platformSelect"
                }, "Pesquisa o ambiente"))), n.push(createElem("ul", {
                    class: "platform search hide"
                }, a.slice(0, 10).reduce(function(e, t) {
                    return e.concat(i(t))
                }, []))), e("#platformScreen").append(createElem("div", {
                    class: "col s12 m8 offset-m2 input-field visible"
                }, n)), e(".search.platform").removeClass("hide"), t.platforms.length > 4 && (e("input#platformSelect").on("blur.platformSelect", function() {
                    e(".search.platform li.selected").removeClass("selected")
                }), e("input#platformSelect").on("keyup.platformSelect", function(t) {
                    var n = e(t.currentTarget).val().normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase(),
                        o = t.keyCode || t.which;
                    if (40 === o || 38 === o) {
                        var s = e(".search.platform li:visible"),
                            r = s.index(e("li.selected")) + (o - 39); - 1 == r ? r += s.length : r == s.length && (r = 0), e(".search.platform li.selected").removeClass("selected"), e(s[r]).addClass("selected")
                    } else if (13 === o) e(".search.platform li.selected:visible > button").click();
                    else {
                        var c;
                        if (n) {
                            var l = new RegExp("(?=.*" + n.split(/\s+/).join(")(?=.*") + ").*", "i");
                            c = a.filter(function(e) {
                                return l.test("" + e.description.normalize("NFD").replace(/[\u0300-\u036f]/g, "").toLowerCase())
                            }).slice(0, 10)
                        } else c = a;
                        e(".search.platform").html(c.slice(0, 10).reduce(function(e, t) {
                            return e.concat(i(t, n))
                        }, []))
                    }
                })), e("div#platformScreen").show()
            },
            u = function(t) {
                t.success ? 302 !== t.code ? t.url ? (Object(a.a)(), -1 != t.url.indexOf("http") ? window.location.replace(t.url) : Object(r.a)(t.url), e("#loginscreen").remove(), e("body").css("overflow", "")) : t.platforms.length > 0 && d(t) : window.location.replace(t.url) : t.message ? Object(c.b)(t.message) : Object(r.a)(t.url), Object(a.b)()
            },
            p = function(e, t) {
                if ("string" == typeof e) return createElem("img", {
                    src: e,
                    alt: t || ""
                });
                if (Array.isArray(e)) {
                    var i = e.reduce(function(i, a, n) {
                        var o = i.concat(createElem("source", {
                            type: s.a.getType(a),
                            srcset: a
                        }));
                        return n === e.length - 1 && (o = o.concat(createElem("img", {
                            src: a,
                            alt: t || ""
                        }))), o
                    }, []);
                    return createElem("picture", null, i)
                }
            };
        t.a = function(t) {
            if (e("header").html(""), e("main").html(""), e("body").css("overflow", "hidden"), e("body").prepend(function(e) {
                    var t = {};
                    e.forgotPasswordUrl ? (t.href = e.forgotPasswordUrl, t.target = "_blank", t.class = "right") : (t.href = "#", t.class = "right forgotPasswordLink");
                    var i = "";
                    e.forgotPasswordPost && (i = createElem("form", {
                        id: "forgotPassword",
                        action: e.forgotPasswordPost,
                        method: "post",
                        novalidate: "",
                        class: "hide"
                    }, [createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: l + " flow-text"
                    }, [createElem("small", null, "Digite abaixo seu e-mail. Enviaremos instruções para redefinir sua senha.")])]), createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: l + " input-field"
                    }, [createElem("input", {
                        id: "email",
                        class: "validate",
                        name: "email",
                        type: "email",
                        required: "",
                        tabindex: "10",
                        autocomplete: "email"
                    }), createElem("label", {
                        for: "email"
                    }, "Email")])]), createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: "col s4 m3 offset-m2 offset-s1 input-field"
                    }, [createElem("button", {
                        id: "forgotBack",
                        class: "btn btn-flat btn-jumbo",
                        tabindex: "12",
                        type: "button"
                    }, "Voltar")]), createElem("div", {
                        class: "col s6 m5 input-field"
                    }, [createElem("button", {
                        id: "forgotSubmit",
                        class: "btn btn-jumbo",
                        tabindex: "11",
                        type: "submit"
                    }, "Recuperar senha")]), createElem("div", {
                        class: l + " validacao"
                    }, [createElem("label", {
                        id: "lblValidacao"
                    })])])]));
                    var a = [createElem("div", {
                        class: "container"
                    }, [createElem("div", {
                        class: "col s12"
                    }, [createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: "" + l
                    }, [createElem("a", {
                        class: "login-logo",
                        href: "/"
                    }, p(["https://mpn.azureedge.net/img/logo/575/bt.webp", "https://mpn.azureedge.net/img/logo/575/bt.png"], e.title))])]), createElem("form", {
                        id: "login",
                        action: e.loginUrl,
                        method: "post",
                        novalidate: ""
                    }, [createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: l + " flow-text"
                    }, [createElem("span", {
                        class: "boasVindas"
                    }), createElem("span", null, e.welcomeMessage || ", seja bem-vindo!")])]), createElem("div", {
                        class: "row"
                    }, [createElem("input", {
                        id: "returnUrl",
                        name: "returnUrl",
                        type: "hidden",
                        value: "" + (e.returnUrl || "/")
                    }), createElem("input", {
                        id: "clientId",
                        name: "clientId",
                        type: "hidden",
                        value: "" + (e.clientId || "")
                    }), createElem("div", {
                        class: l + " input-field"
                    }, [createElem("input", {
                        id: "username",
                        class: "validate",
                        name: "username",
                        type: "text",
                        required: "",
                        autocomplete: "username",
                        tabindex: "0"
                    }), createElem("label", {
                        for: "username"
                    }, "Usuário")]), createElem("div", {
                        class: l + " input-field"
                    }, [createElem("input", {
                        id: "password",
                        class: "validate",
                        name: "password",
                        type: "password",
                        required: "",
                        autocomplete: "current-password"
                    }), createElem("label", {
                        for: "password"
                    }, "Senha")]), createElem("div", {
                        class: " btn-flat " + l
                    }, [createElem("a", t, "Esqueci minha senha")])]), createElem("div", {
                        class: "row"
                    }, [createElem("div", {
                        class: l + " input-field"
                    }, [createElem("button", {
                        id: "loginBtn",
                        class: "btn btn-jumbo",
                        type: "submit"
                    }, "Login")]), createElem("div", {
                        class: l + " validacao"
                    }, [createElem("label", {
                        id: "lblValidacao"
                    })])])]), i, createElem("div", {
                        id: "platformScreen",
                        class: "row",
                        style: "height:350px;display:none;"
                    }, [createElem("form", {
                        id: "platform",
                        method: "post"
                    }, [createElem("input", {
                        id: "PlatformId",
                        name: "PlatformId",
                        type: "hidden",
                        value: ""
                    })])])])])];
                    return e.carousel ? createElem("div", {
                        class: "row",
                        id: "loginscreen"
                    }, [createElem("div", {
                        class: "col s12 l6 valign-wrapper div-login center-align",
                        style: "position: absolute;min-height: 100%;right: 0;"
                    }, a), createElem("div", {
                        class: "col l6 hide-on-med-and-down white-text black carousel-fade"
                    }, [createElem("div", {
                        class: "image-item"
                    }, e.carousel.reduce(function(e, t, i) {
                        var a = p(t.image, t.text);
                        return 0 == i && a.classList.add(["active"]), e.concat(a)
                    }, []))])]) : createElem("div", {
                        class: "row",
                        id: "loginscreen"
                    }, [createElem("div", {
                        class: l + " l6 offset-l3 valign-wrapper div-login center-align"
                    }, a)])
                }(t)), t.platforms && t.platforms.length > 0 && d(t), e(".div-login").height(e(window).height()), e(".carousel-fade").length > 0) {
                setInterval(function() {
                    var t = ".carousel-fade .image-item",
                        i = t + ">img, " + t + ">picture",
                        a = e(".carousel-fade .image-item>.active").index() + 1;
                    a = e(i).length == a ? 0 : a, e(".carousel-fade .image-item>.active").removeClass("active"), e(i + ":nth-child(" + (a + 1) + ")").addClass("active")
                }, 8e3)
            }
            var i = (new Date).getHours(),
                o = "Boa noite";
            i >= 6 && i < 12 ? o = "Bom dia" : i >= 12 && i < 19 && (o = "Boa tarde"), e(".boasVindas").text(o), Object(n.r)();
            var s = e("form#platform"),
                r = e("form#login"),
                f = e("form#forgotPassword");
            r.on("submit", function(t) {
                if (t.preventDefault(), Object(a.a)(), !r.valid()) return Object(a.b)(), !1;
                e.ajax({
                    url: r.attr("action"),
                    type: "POST",
                    data: r.serialize(),
                    success: u,
                    error: function(e, t) {
                        Object(c.b)(t), Object(a.b)()
                    }
                })
            }), e(document).on("click", ".forgotPasswordLink", function() {
                r.addClass("hide"), f.removeClass("hide")
            }), e(document).on("click", "#forgotBack", function() {
                r.removeClass("hide"), f.addClass("hide")
            }), s.on("submit", function(t) {
                if (t.preventDefault(), Object(a.a)(), "" == e("#PlatformId").val() && "" == e("#ClientId").val() || !s.valid()) return Object(a.b)(), !1;
                e.ajax({
                    url: s.attr("action"),
                    type: "POST",
                    data: s.serialize(),
                    success: u,
                    error: function(e, t) {
                        Object(c.b)(t), Object(a.b)()
                    }
                })
            }), f.on("submit", function(t) {
                if (t.preventDefault(), Object(a.a)(), !f.valid()) return Object(a.b)(), !1;
                e.ajax({
                    url: f.attr("action"),
                    type: "POST",
                    data: f.serialize(),
                    success: function(e) {
                        e.message && (e.success ? Object(c.c)(e.message) : Object(c.b)(e.message)), Object(a.b)()
                    },
                    error: function(e, t) {
                        Object(c.b)(t), Object(a.b)()
                    }
                })
            }), e(document).on("click", ".platformGo", function(i) {
                i.preventDefault();
                var a = e(i.target).hasClass("platformGo") ? e(i.target) : e(i.target).closest(".platformGo");
                e("#PlatformId").val(a.data("platform")), t.platforms && t.platforms.length > 0 ? document.getElementById("platform").submit() : s.submit()
            }), Object(a.b)()
        }
    }).call(this, i(1))
}, , function(e, t, i) {
    "use strict";
    (function(e, a) {
        var n = i(47),
            o = i.n(n),
            s = i(8),
            r = i(2),
            c = i(3);

        function l() {
            document.querySelector("[data-activates=notification-menu] .material-icons").innerHTML = "notifications_" + (document.querySelector(".notification:not(.done)") ? "active" : "none")
        }

        function d(t) {
            if (t.email && e("#user-email").html(t.email), t.platform && !["serie1@totvs.com.br", "fly01@totvs.com.br"].includes(t.email)) {
                var i = e("#platform-id").val();
                e("#platform-name").html(t.platform.name), e("#platform-id").val(t.platform.id), e("#platform-url").val(t.platform.url);
                var a = document.head.querySelector("[href*=mpnui]");
                "blue" !== t.platform.color || document.head.querySelector("[href*=blue]") ? !t.platform.color && document.head.querySelector("[href*=blue]") && (a.href = a.href.replace("mpnui.blue.css", "mpnui.css")) : a.href = a.href.replace("mpnui.css", "mpnui.blue.css"), "" !== i && i !== t.platform.id && (Object(r.a)(), e.ajax({
                    url: e("form#platform").attr("action"),
                    type: "POST",
                    data: "PlatformId=" + t.platform.url,
                    success: function(e) {
                        e.success ? 302 !== e.code ? window.history.state ? Object(s.a)(window.history.state.urlJson, window.history.state.id, !0, !0) : location.reload() : window.location.replace(e.url) : e.message ? (Object(c.b)(e.message), Object(r.b)()) : location.reload()
                    },
                    error: function(e, t) {
                        Object(c.b)(t), Object(r.b)()
                    }
                }))
            }
        }

        function u(e) {
            var t = document.querySelector("[data-id='" + e._id + "']");
            if (void 0 !== t) {
                var i = t.closest(".notification");
                e.readDate ? (t.classList.remove("readNotification"), t.classList.add("unreadNotification"), t.innerHTML = "drafts", i.classList.add("done")) : (t.classList.remove("unreadNotification"), t.classList.add("readNotification"), t.innerHTML = "markunread", i.classList.remove("done"))
            }
            l()
        }

        function p(e) {
            var t = document.getElementById("notification-menu");
            if (t) {
                e.messageType = (e.messageType.toString() || "info").toLowerCase();
                var i = [];
                i.push(createElem("h6", null, e.message)), i.push(createElem("small", {
                    class: ""
                }, a(e.notificationDate).format("DD/MM/YY hh:mm"))), i.push(createElem("i", {
                    class: "notificationType material-icons"
                }, "success" === e.messageType ? "check_circle" : e.messageType)), i.push(createElem("i", {
                    class: "right " + (e.readDate ? "un" : "") + "readNotification material-icons",
                    "data-id": e._id
                }, e.readDate ? "drafts" : "markunread")), t.prepend(createElem("li", {
                    class: e.messageType + " notification " + (e.readDate ? "done" : ""),
                    "data-action": "" + e.actionUrl
                }, i))
            }
            l()
        }
        t.a = function(e, t) {
            e.jwt && e.socketServer && function(e, t) {
                var i = new XMLHttpRequest;
                i.open("GET", e, !0), i.responseType = "json", i.onload = function() {
                    var e = i.status;
                    t(200 === e ? null : e, i.response)
                }, i.send()
            }(e.jwt, function(i, n) {
                if (null !== i) Object(c.b)(" " + i), Object(r.b)();
                else {
                    var l = o()(e.socketServer, {
                        query: "token=" + n.token,
                        forceNew: !0
                    });
                    l.on("error", function(t) {
                        if ("UnauthorizedError" == t.type || "invalid_token" == t.code) try {
                            l = o()(e.socketServer, {
                                query: "token=" + n.token,
                                forceNew: !0
                            })
                        } catch (e) {
                            Object(c.b)("Token expirado!")
                        }
                    }), l.on("receivedMessage_" + e.channel, p), l.on("readMessage_" + e.channel, u), l.on("userChannel_" + t, d), l.emit("login"), document.addEventListener("click", function(e) {
                        if (e.target.closest("#notification-menu")) {
                            var t = e.target.closest("li.notification");
                            if (e.target.classList.contains("readNotification")) e.target.getAttribute("data-id") ? l.emit("readMessage", {
                                _id: e.target.getAttribute("data-id"),
                                readDate: a().format()
                            }) : l.emit("readAllMessage", {
                                readDate: a().format()
                            });
                            else if (e.target.classList.contains("unreadNotification")) e.target.getAttribute("data-id") && l.emit("readMessage", {
                                _id: e.target.getAttribute("data-id")
                            });
                            else if (t) {
                                var i = t.getAttribute("data-action");
                                i && (-1 != i.indexOf("http") ? window.location.replace(i) : Object(s.a)(i))
                            }
                        } else !e.target.closest("[data-activates=notification-menu]") && document.querySelector("#notification-menu.active") && document.querySelector("[data-activates=notification-menu]").click()
                    })
                }
            })
        }
    }).call(this, i(1), i(10))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        i(56);
        var a = i(0);
        t.a = function(t, i, n) {
            var o = "modal" + Math.random().toString(36).substr(2, 7),
                s = Object(a.b)("div", {
                    id: o,
                    class: "modal"
                }, [Object(a.b)("div", {
                    class: "modal-header"
                }, [Object(a.b)("h5", null, t), Object(a.b)("a", {
                    class: "modal-action modal-close"
                }, [Object(a.b)("i", {
                    class: "material-icons"
                }, "close")])]), Object(a.b)("div", {
                    class: "modal-content"
                }, [Object(a.b)("h6", null, i)]), Object(a.b)("div", {
                    class: "modal-footer"
                }, [Object(a.b)("a", {
                    class: "modal-action modal-close btn btn-secondary"
                }, "CANCELAR"), Object(a.b)("a", {
                    class: "modal-action modal-close btn",
                    onclick: n
                }, "CONTINUAR")])]);
            return e("main").append(s), e(".modal").modal(), e("#" + o).modal("open"), {
                Modal: s
            }
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    var a = i(4),
        n = i(0);
    t.a = function(e) {
        return Object(a.a)(e.id, (e.class || "col s12") + " visible", [function(e) {
            var t = [],
                i = {
                    id: e.id,
                    type: e.type,
                    name: e.name || e.id,
                    class: "btn " + e.classBtn || ""
                };
            if (e.disabled && (i.disabled = "disabled"), e.readonly && (i.readonly = "readonly"), e.onClickFn && (i.onclick = e.onClickFn + (e.onClickFn.includes("(") ? "" : "()")), e.value && (i.value = e.value, t.push(e.value)), e.icon) {
                e.iconPosition = e.iconPosition || "left";
                var a = Object(n.b)("i", {
                    class: "material-icons " + e.iconPosition
                }, e.icon);
                "right" === e.iconPosition ? t.push(a) : t.unshift(a)
            }
            return Object(n.b)("button", i, t)
        }(e)])
    }
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(0),
            a = i(8);
        Date.now || (Date.now = function() {
            return (new Date).getTime()
        }), window.onpopstate = function(e) {
            e.state && e.state.urlJson && Object(a.a)(e.state.urlJson, e.state.id, !0)
        };
        for (var n = ["webkit", "moz"], o = 0; o < n.length && !window.requestAnimationFrame; ++o) {
            var s = n[o];
            window.requestAnimationFrame = window[s + "RequestAnimationFrame"], window.cancelAnimationFrame = window[s + "CancelAnimationFrame"] || window[s + "CancelRequestAnimationFrame"]
        }
        if (/iP(ad|hone|od).*OS 6/.test(window.navigator.userAgent) || !window.requestAnimationFrame || !window.cancelAnimationFrame) {
            var r = 0;
            window.requestAnimationFrame = function(e) {
                var t = Date.now(),
                    i = Math.max(r + 16, t);
                return setTimeout(function() {
                    e(r = i)
                }, i - t)
            }, window.cancelAnimationFrame = clearTimeout
        }
        String.prototype.format || (String.prototype.format = function() {
                var e = arguments;
                return this.replace(/{(\d+)}/g, function(t, i) {
                    return void 0 !== e[i] ? e[i] : t
                })
            }), String.prototype.padEnd || (String.prototype.padEnd = function(e, t) {
                return e >>= 0, t = String(void 0 !== t ? t : " "), this.length > e ? String(this) : ((e -= this.length) > t.length && (t += t.repeat(e / t.length)), String(this) + t.slice(0, e))
            }), String.prototype.padStart || (String.prototype.padStart = function(e, t) {
                return e >>= 0, t = String(void 0 !== t ? t : " "), this.length > e ? String(this) : ((e -= this.length) > t.length && (t += t.repeat(e / t.length)), t.slice(0, e) + String(this))
            }), /iphone|ipad/i.test(navigator.userAgent) && e("body").bind("touchstart", function(t) {
                var i = t.timeStamp,
                    a = i - (e(this).data("lastTouch") || i),
                    n = t.originalEvent.touches.length;
                e(this).data("lastTouch", i), !a || a > 500 || n > 1 || (t.preventDefault(), e(this).trigger("click").trigger("click"))
            }), 0 == e("#loader-wrapper").length && e("body").append(Object(t.b)("div", {
                id: "loader-wrapper"
            }, Object(t.b)("div", {
                id: "loader"
            }))),
            function() {
                var e = document.querySelector("link[rel*='icon']") || document.createElement("link");
                e.type = "image/x-icon", e.rel = "shortcut icon", e.href = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAAuJAAALiQE3ycutAAAIf2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDIgNzkuMTYwOTI0LCAyMDE3LzA3LzEzLTAxOjA2OjM5ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIgeG1wOkNyZWF0ZURhdGU9IjIwMTgtMDYtMjhUMTg6MjQ6NTktMDM6MDAiIHhtcDpNZXRhZGF0YURhdGU9IjIwMTgtMDYtMjhUMTg6MjY6MzAtMDM6MDAiIHhtcDpNb2RpZnlEYXRlPSIyMDE4LTA2LTI4VDE4OjI2OjMwLTAzOjAwIiBkYzpmb3JtYXQ9ImltYWdlL3BuZyIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDphMzNkZTg0NC1mMjdmLTcyNDEtYThhYi0xMjcwNTdlYzc0MzQiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDo3MjRhMTdiNy05MTE2LTQzNDItYmFjMy1kNzA3ZjE2Yjk5OTUiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDo1MTIxNDNiYy1mNjRlLWMwNGQtYTU1YS1iOGU3ZmUyNGRhZjIiIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjUxMjE0M2JjLWY2NGUtYzA0ZC1hNTVhLWI4ZTdmZTI0ZGFmMiIgc3RFdnQ6d2hlbj0iMjAxOC0wNi0yOFQxODoyNDo1OS0wMzowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKSIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6Nzc1NzIxNzMtZWUxMi03ODQ5LTg3YmYtYzBjMGE5ZTNlNDRjIiBzdEV2dDp3aGVuPSIyMDE4LTA2LTI4VDE4OjI2OjMwLTAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBQaG90b3Nob3AgQ0MgKFdpbmRvd3MpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjb252ZXJ0ZWQiIHN0RXZ0OnBhcmFtZXRlcnM9ImZyb20gYXBwbGljYXRpb24vdm5kLmFkb2JlLnBob3Rvc2hvcCB0byBpbWFnZS9wbmciLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImRlcml2ZWQiIHN0RXZ0OnBhcmFtZXRlcnM9ImNvbnZlcnRlZCBmcm9tIGFwcGxpY2F0aW9uL3ZuZC5hZG9iZS5waG90b3Nob3AgdG8gaW1hZ2UvcG5nIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJzYXZlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDphMzNkZTg0NC1mMjdmLTcyNDEtYThhYi0xMjcwNTdlYzc0MzQiIHN0RXZ0OndoZW49IjIwMTgtMDYtMjhUMTg6MjY6MzAtMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAoV2luZG93cykiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjc3NTcyMTczLWVlMTItNzg0OS04N2JmLWMwYzBhOWUzZTQ0YyIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo1MTIxNDNiYy1mNjRlLWMwNGQtYTU1YS1iOGU3ZmUyNGRhZjIiIHN0UmVmOm9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDo1MTIxNDNiYy1mNjRlLWMwNGQtYTU1YS1iOGU3ZmUyNGRhZjIiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6TvBCMAAADPklEQVQ4y5VTSUyTURAejQc3XCBhkSjiUqpsQoVWSktbtqKgUgQXZLEoFYEgO0RWWWQppWWxVgTRSqFQEwVikO0oiQeJXjTUi9EDie3FE4kh45uf6EFPHub/X17m+97MN9/A+vo6ICKMW8dg6uUL6NHrFOlpKcOqs6dXkpQxPyjoTHc93ToF5VAuYQgLvwmmpybhSqpqVCE5hcHHjmJoIB8lopMojxSxEOIpQTBGRYQh5VDuHwI6OJ0OSEs+u3T8iA8HyriUas+7nv38Usq5z2di5SgOC2H3AowUCpDn440XVeeWCENY7tPSWGfb7+GKsohwDPH3w47W5sr5mRno79ZCv6GLr+tsUzPCIVWS0h4tjUDPvTux9W69jSNYmHstDg3wQ5lY6GT9flNGS3HMYi6fn30ND/p6wGYdhcXFeejt6oCWujswNmKWXTif+JZ/aD8uzM+Koex2oc3LzQX12o5G+8qnzRWlxUKlQtp3O19T0XmvJfj5hJXp8xIGTSYw9feC3b4CBp22wGPPDiQssH4cIf48bKqvyRk03Qeffe6wa+sWS5w8EuMVEiZa8sf83JzupTdLnsvLy1BRWgTqzCvq8BMByDRyQrxC+jNOIcVcdVZGVVkJ61kHt3LVVhFTPU4u4QSkqVSXF9c+evgA2lubqersmCgxEpYjYIHpF1My6u9UwerqKhj7erp4B70xPCQQaayi0EDUqLMMT4eHoLa6Ai6nJl8lDEdALQgCj2G+JudGt7YDHA4HrK2t7Sotytcw0JQqKeE7aTQxZrGS6rZxC03tWgDPl8bp4ER0370NB0z3az68fwdGgx5mpifBajHDzKtp0Gvbtt9rblQ2NdT15V3PqrSYH4tbmxoGD3i4bYi4MDcrDuAdQjbCrwUadfEtdTa/uqQQRsxDMGp+AgPGfnjGSi+4eaMsiH+YcyO5MizYH8kCnJEYo83L1YVzYUJ0FKadT3x/MyfTWFNZlt7efJc/PGDcVFKYZxAJgjAhRoa+3u7Y1FBr++NEzsqqDSsrJCJOuAimPjmTAOlpqi+s3x9ysRAph2zvdDo3rPz3MlGJ9BKRxMoiub8wNAgFbLlosf5Zpv9aZ/2/6/wL2nnqQvGMPpIAAAAASUVORK5CYII=", document.getElementsByTagName("head")[0].appendChild(e)
            }(),
            function(e) {
                XMLHttpRequest.prototype.open = function() {
                    this.addEventListener("readystatechange", function() {
                        if (4 === this.readyState && 200 == this.status && ("" === this.responseType || "text" === this.responseType)) try {
                            var e = JSON.parse(this.responseText);
                            e && e.urlToRedirect && (top.location.href = e.urlToRedirect)
                        } catch (e) {}
                    }, !1), e.apply(this, arguments)
                }
            }(XMLHttpRequest.prototype.open), e(document).on("click.linkGo", ".linkGo", function(t) {
                var i = e(t.currentTarget);
                i.data("go") && Object(a.a)(i.data("go"))
            }),
            function(e) {
                e.ui.autocomplete.prototype._resizeMenu = function() {
                    this.menu.element.outerWidth(this.element.outerWidth())
                };
                var t = e.fn.val;
                e.fn.val = function(e) {
                    return arguments.length >= 1 ? ("number" == typeof e && (e = e.toLocaleString("pt-BR", {
                        useGrouping: !1
                    })), t.call(this, e)) : t.call(this)
                };
                var i = /\r?\n/g,
                    a = /^(?:submit|button|image|reset|file)$/i,
                    n = /^(?:input|select|textarea|keygen)/i,
                    o = /^(?:checkbox|radio)$/i;
                e.fn.serializeArray = function() {
                    return this.map(function() {
                        var t = e.prop(this, "elements");
                        return t ? e.makeArray(t) : this
                    }).filter(function() {
                        var t = this.type;
                        return this.name && !e(this).is(":disabled") && n.test(this.nodeName) && !a.test(t) && (this.checked || !o.test(t))
                    }).map(function(t, a) {
                        var n = e(this).val();
                        return null == n ? null : Array.isArray(n) ? e.map(n, function(e) {
                            return {
                                name: a.name,
                                value: e.replace(i, "\r\n")
                            }
                        }) : ("number" != typeof n && (n = n.replace(i, "\r\n")), {
                            name: a.name,
                            value: n
                        })
                    }).get()
                }
            }(window.jQuery),
            function() {
                for (var e = !1, t = document.cookie.split(";"), i = 0; i < t.length; i++) {
                    var a = t[i],
                        n = a.indexOf("="),
                        o = n > -1 ? a.substr(0, n) : a;
                    ".FLY01_MANAGER_ASPXAUTH" === o && (e = !0)
                }
                if (e)
                    for (i = 0; i < t.length; i++) o = (n = (a = t[i]).indexOf("=")) > -1 ? a.substr(0, n) : a, document.cookie = o + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT"
            }()
    }).call(this, i(1))
}, , , , , , , , , , , , , , , , , , , function(e, t, i) {
    /*!
     * Date picker for pickadate.js v3.5.0
     * http://amsul.github.io/pickadate.js/date.htm
     */
    e.exports = function(e, t) {
        var i = e._,
            a = function(e, t) {
                var i = this,
                    a = e.$node[0],
                    n = a.value,
                    o = e.$node.data("value"),
                    s = o || n,
                    r = o ? t.formatSubmit : t.format,
                    c = function() {
                        return a.currentStyle ? "rtl" == a.currentStyle.direction : "rtl" == getComputedStyle(e.$root[0]).direction
                    };
                i.settings = t, i.$node = e.$node, i.queue = {
                    min: "measure create",
                    max: "measure create",
                    now: "now create",
                    select: "parse create validate",
                    highlight: "parse navigate create validate",
                    view: "parse create validate viewset",
                    disable: "deactivate",
                    enable: "activate"
                }, i.item = {}, i.item.clear = null, i.item.disable = (t.disable || []).slice(0), i.item.enable = - function(e) {
                    return !0 === e[0] ? e.shift() : -1
                }(i.item.disable), i.set("min", t.min).set("max", t.max).set("now"), s ? i.set("select", s, {
                    format: r
                }) : i.set("select", null).set("highlight", i.item.now), i.key = {
                    40: 7,
                    38: -7,
                    39: function() {
                        return c() ? -1 : 1
                    },
                    37: function() {
                        return c() ? 1 : -1
                    },
                    go: function(e) {
                        var t = i.item.highlight,
                            a = new Date(t.year, t.month, t.date + e);
                        i.set("highlight", a, {
                            interval: e
                        }), this.render()
                    }
                }, e.on("render", function() {
                    e.$root.find("." + t.klass.selectMonth).on("change", function() {
                        var i = this.value;
                        i && (e.set("highlight", [e.get("view").year, i, e.get("highlight").date]), e.$root.find("." + t.klass.selectMonth).trigger("focus"))
                    }), e.$root.find("." + t.klass.selectYear).on("change", function() {
                        var i = this.value;
                        i && (e.set("highlight", [i, e.get("view").month, e.get("highlight").date]), e.$root.find("." + t.klass.selectYear).trigger("focus"))
                    })
                }, 1).on("open", function() {
                    var a = "";
                    i.disabled(i.get("now")) && (a = ":not(." + t.klass.buttonToday + ")"), e.$root.find("button" + a + ", select").attr("disabled", !1)
                }, 1).on("close", function() {
                    e.$root.find("button, select").attr("disabled", !0)
                }, 1)
            };
        a.prototype.set = function(e, t, i) {
            var a = this,
                n = a.item;
            return null === t ? ("clear" == e && (e = "select"), n[e] = t, a) : (n["enable" == e ? "disable" : "flip" == e ? "enable" : e] = a.queue[e].split(" ").map(function(n) {
                return t = a[n](e, t, i)
            }).pop(), "select" == e ? a.set("highlight", n.select, i) : "highlight" == e ? a.set("view", n.highlight, i) : e.match(/^(flip|min|max|disable|enable)$/) && (n.select && a.disabled(n.select) && a.set("select", n.select, i), n.highlight && a.disabled(n.highlight) && a.set("highlight", n.highlight, i)), a)
        }, a.prototype.get = function(e) {
            return this.item[e]
        }, a.prototype.create = function(e, a, n) {
            var o;
            return (a = void 0 === a ? e : a) == -1 / 0 || a == 1 / 0 ? o = a : t.isPlainObject(a) && i.isInteger(a.pick) ? a = a.obj : t.isArray(a) ? (a = new Date(a[0], a[1], a[2]), a = i.isDate(a) ? a : this.create().obj) : a = i.isInteger(a) || i.isDate(a) ? this.normalize(new Date(a), n) : this.now(e, a, n), {
                year: o || a.getFullYear(),
                month: o || a.getMonth(),
                date: o || a.getDate(),
                day: o || a.getDay(),
                obj: o || a,
                pick: o || a.getTime()
            }
        }, a.prototype.createRange = function(e, a) {
            var n = this,
                o = function(e) {
                    return !0 === e || t.isArray(e) || i.isDate(e) ? n.create(e) : e
                };
            return i.isInteger(e) || (e = o(e)), i.isInteger(a) || (a = o(a)), i.isInteger(e) && t.isPlainObject(a) ? e = [a.year, a.month, a.date + e] : i.isInteger(a) && t.isPlainObject(e) && (a = [e.year, e.month, e.date + a]), {
                from: o(e),
                to: o(a)
            }
        }, a.prototype.withinRange = function(e, t) {
            return e = this.createRange(e.from, e.to), t.pick >= e.from.pick && t.pick <= e.to.pick
        }, a.prototype.overlapRanges = function(e, t) {
            return e = this.createRange(e.from, e.to), t = this.createRange(t.from, t.to), this.withinRange(e, t.from) || this.withinRange(e, t.to) || this.withinRange(t, e.from) || this.withinRange(t, e.to)
        }, a.prototype.now = function(e, t, i) {
            return t = new Date, i && i.rel && t.setDate(t.getDate() + i.rel), this.normalize(t, i)
        }, a.prototype.navigate = function(e, i, a) {
            var n, o, s, r, c = t.isArray(i),
                l = t.isPlainObject(i),
                d = this.item.view;
            if (c || l) {
                for (l ? (o = i.year, s = i.month, r = i.date) : (o = +i[0], s = +i[1], r = +i[2]), a && a.nav && d && d.month !== s && (o = d.year, s = d.month), n = new Date(o, s + (a && a.nav ? a.nav : 0), 1), o = n.getFullYear(), s = n.getMonth(); new Date(o, s, r).getMonth() !== s;) r -= 1;
                i = [o, s, r]
            }
            return i
        }, a.prototype.normalize = function(e) {
            return e.setHours(0, 0, 0, 0), e
        }, a.prototype.measure = function(e, t) {
            return t ? "string" == typeof t ? t = this.parse(e, t) : i.isInteger(t) && (t = this.now(e, t, {
                rel: t
            })) : t = "min" == e ? -1 / 0 : 1 / 0, t
        }, a.prototype.viewset = function(e, t) {
            return this.create([t.year, t.month, 1])
        }, a.prototype.validate = function(e, a, n) {
            var o, s, r, c, l = this,
                d = a,
                u = n && n.interval ? n.interval : 1,
                p = -1 === l.item.enable,
                f = l.item.min,
                h = l.item.max,
                m = p && l.item.disable.filter(function(e) {
                    if (t.isArray(e)) {
                        var n = l.create(e).pick;
                        n < a.pick ? o = !0 : n > a.pick && (s = !0)
                    }
                    return i.isInteger(e)
                }).length;
            if ((!n || !n.nav) && (!p && l.disabled(a) || p && l.disabled(a) && (m || o || s) || !p && (a.pick <= f.pick || a.pick >= h.pick)))
                for (p && !m && (!s && u > 0 || !o && u < 0) && (u *= -1); l.disabled(a) && (Math.abs(u) > 1 && (a.month < d.month || a.month > d.month) && (a = d, u = u > 0 ? 1 : -1), a.pick <= f.pick ? (r = !0, u = 1, a = l.create([f.year, f.month, f.date + (a.pick === f.pick ? 0 : -1)])) : a.pick >= h.pick && (c = !0, u = -1, a = l.create([h.year, h.month, h.date + (a.pick === h.pick ? 0 : 1)])), !r || !c);) a = l.create([a.year, a.month, a.date + u]);
            return a
        }, a.prototype.disabled = function(e) {
            var a = this,
                n = a.item.disable.filter(function(n) {
                    return i.isInteger(n) ? e.day === (a.settings.firstDay ? n : n - 1) % 7 : t.isArray(n) || i.isDate(n) ? e.pick === a.create(n).pick : t.isPlainObject(n) ? a.withinRange(n, e) : void 0
                });
            return n = n.length && !n.filter(function(e) {
                return t.isArray(e) && "inverted" == e[3] || t.isPlainObject(e) && e.inverted
            }).length, -1 === a.item.enable ? !n : n || e.pick < a.item.min.pick || e.pick > a.item.max.pick
        }, a.prototype.parse = function(e, t, a) {
            var n = this,
                o = {};
            return t && "string" == typeof t ? (a && a.format || ((a = a || {}).format = n.settings.format), n.formats.toArray(a.format).map(function(e) {
                var a = n.formats[e],
                    s = a ? i.trigger(a, n, [t, o]) : e.replace(/^!/, "").length;
                a && (o[e] = t.substr(0, s)), t = t.substr(s)
            }), [o.yyyy || o.yy, +(o.mm || o.m) - 1, o.dd || o.d]) : t
        }, a.prototype.isDateExact = function(e, a) {
            return i.isInteger(e) && i.isInteger(a) || "boolean" == typeof e && "boolean" == typeof a ? e === a : (i.isDate(e) || t.isArray(e)) && (i.isDate(a) || t.isArray(a)) ? this.create(e).pick === this.create(a).pick : !(!t.isPlainObject(e) || !t.isPlainObject(a)) && this.isDateExact(e.from, a.from) && this.isDateExact(e.to, a.to)
        }, a.prototype.isDateOverlap = function(e, a) {
            var n = this.settings.firstDay ? 1 : 0;
            return i.isInteger(e) && (i.isDate(a) || t.isArray(a)) ? (e = e % 7 + n) === this.create(a).day + 1 : i.isInteger(a) && (i.isDate(e) || t.isArray(e)) ? (a = a % 7 + n) === this.create(e).day + 1 : !(!t.isPlainObject(e) || !t.isPlainObject(a)) && this.overlapRanges(e, a)
        }, a.prototype.flipEnable = function(e) {
            var t = this.item;
            t.enable = e || (-1 == t.enable ? 1 : -1)
        }, a.prototype.deactivate = function(e, a) {
            var n = this,
                o = n.item.disable.slice(0);
            return "flip" == a ? n.flipEnable() : !1 === a ? (n.flipEnable(1), o = []) : !0 === a ? (n.flipEnable(-1), o = []) : a.map(function(e) {
                for (var a, s = 0; s < o.length; s += 1)
                    if (n.isDateExact(e, o[s])) {
                        a = !0;
                        break
                    } a || (i.isInteger(e) || i.isDate(e) || t.isArray(e) || t.isPlainObject(e) && e.from && e.to) && o.push(e)
            }), o
        }, a.prototype.activate = function(e, a) {
            var n = this,
                o = n.item.disable,
                s = o.length;
            return "flip" == a ? n.flipEnable() : !0 === a ? (n.flipEnable(1), o = []) : !1 === a ? (n.flipEnable(-1), o = []) : a.map(function(e) {
                var a, r, c, l;
                for (c = 0; c < s; c += 1) {
                    if (r = o[c], n.isDateExact(r, e)) {
                        a = o[c] = null, l = !0;
                        break
                    }
                    if (n.isDateOverlap(r, e)) {
                        t.isPlainObject(e) ? (e.inverted = !0, a = e) : t.isArray(e) ? (a = e)[3] || a.push("inverted") : i.isDate(e) && (a = [e.getFullYear(), e.getMonth(), e.getDate(), "inverted"]);
                        break
                    }
                }
                if (a)
                    for (c = 0; c < s; c += 1)
                        if (n.isDateExact(o[c], e)) {
                            o[c] = null;
                            break
                        } if (l)
                    for (c = 0; c < s; c += 1)
                        if (n.isDateOverlap(o[c], e)) {
                            o[c] = null;
                            break
                        } a && o.push(a)
            }), o.filter(function(e) {
                return null != e
            })
        }, a.prototype.nodes = function(e) {
            var t = this,
                a = t.settings,
                n = t.item,
                o = n.now,
                s = n.select,
                r = n.highlight,
                c = n.view,
                l = n.disable,
                d = n.min,
                u = n.max,
                p = function(e, t) {
                    return a.firstDay && (e.push(e.shift()), t.push(t.shift())), i.node("thead", i.node("tr", i.group({
                        min: 0,
                        max: 6,
                        i: 1,
                        node: "th",
                        item: function(i) {
                            return [e[i], a.klass.weekdays, 'scope=col title="' + t[i] + '"']
                        }
                    })))
                }((a.showWeekdaysFull ? a.weekdaysFull : a.weekdaysLetter).slice(0), a.weekdaysFull.slice(0)),
                f = function(e) {
                    return i.node("div", " ", a.klass["nav" + (e ? "Next" : "Prev")] + (e && c.year >= u.year && c.month >= u.month || !e && c.year <= d.year && c.month <= d.month ? " " + a.klass.navDisabled : ""), "data-nav=" + (e || -1) + " " + i.ariaAttr({
                        role: "button",
                        controls: t.$node[0].id + "_table"
                    }) + ' title="' + (e ? a.labelMonthNext : a.labelMonthPrev) + '"')
                },
                h = function(n) {
                    var o = a.showMonthsShort ? a.monthsShort : a.monthsFull;
                    return "short_months" == n && (o = a.monthsShort), a.selectMonths && void 0 == n ? i.node("select", i.group({
                        min: 0,
                        max: 11,
                        i: 1,
                        node: "option",
                        item: function(e) {
                            return [o[e], 0, "value=" + e + (c.month == e ? " selected" : "") + (c.year == d.year && e < d.month || c.year == u.year && e > u.month ? " disabled" : "")]
                        }
                    }), a.klass.selectMonth + " browser-default", (e ? "" : "disabled") + " " + i.ariaAttr({
                        controls: t.$node[0].id + "_table"
                    }) + ' title="' + a.labelMonthSelect + '"') : "short_months" == n ? null != s ? o[s.month] : o[c.month] : i.node("div", o[c.month], a.klass.month)
                },
                m = function(n) {
                    var o = c.year,
                        r = !0 === a.selectYears ? 5 : ~~(a.selectYears / 2);
                    if (r) {
                        var l = d.year,
                            p = u.year,
                            f = o - r,
                            h = o + r;
                        if (l > f && (h += l - f, f = l), p < h) {
                            var m = f - l,
                                b = h - p;
                            f -= m > b ? b : m, h = p
                        }
                        if (a.selectYears && void 0 == n) return i.node("select", i.group({
                            min: f,
                            max: h,
                            i: 1,
                            node: "option",
                            item: function(e) {
                                return [e, 0, "value=" + e + (o == e ? " selected" : "")]
                            }
                        }), a.klass.selectYear + " browser-default", (e ? "" : "disabled") + " " + i.ariaAttr({
                            controls: t.$node[0].id + "_table"
                        }) + ' title="' + a.labelYearSelect + '"')
                    }
                    return "raw" === n && null != s ? i.node("div", s.year) : i.node("div", o, a.klass.year)
                };
            return i.node("div", i.node("div", m("raw"), a.klass.year_display) + i.node("span", function() {
                var e;
                return e = null != s ? s.day : o.day, a.weekdaysShort[e]
            }() + ", ", a.klass.weekday_display) + i.node("span", (null != s ? s.date : o.date) + " ", a.klass.day_display) + i.node("span", h("short_months"), a.klass.month_display), a.klass.date_display) + i.node("div", i.node("div", i.node("div", (a.selectYears, h() + m() + f() + f(1)), a.klass.header) + i.node("table", p + i.node("tbody", i.group({
                min: 0,
                max: 5,
                i: 1,
                node: "tr",
                item: function(e) {
                    var n = a.firstDay && 0 === t.create([c.year, c.month, 1]).day ? -7 : 0;
                    return [i.group({
                        min: 7 * e - c.day + n + 1,
                        max: function() {
                            return this.min + 7 - 1
                        },
                        i: 1,
                        node: "td",
                        item: function(e) {
                            e = t.create([c.year, c.month, e + (a.firstDay ? 1 : 0)]);
                            var n = s && s.pick == e.pick,
                                p = r && r.pick == e.pick,
                                f = l && t.disabled(e) || e.pick < d.pick || e.pick > u.pick,
                                h = i.trigger(t.formats.toString, t, [a.format, e]);
                            return [i.node("div", e.date, function(t) {
                                return t.push(c.month == e.month ? a.klass.infocus : a.klass.outfocus), o.pick == e.pick && t.push(a.klass.now), n && t.push(a.klass.selected), p && t.push(a.klass.highlighted), f && t.push(a.klass.disabled), t.join(" ")
                            }([a.klass.day]), "data-pick=" + e.pick + " " + i.ariaAttr({
                                role: "gridcell",
                                label: h,
                                selected: !(!n || t.$node.val() !== h) || null,
                                activedescendant: !!p || null,
                                disabled: !!f || null
                            }) + " " + (f ? "" : 'tabindex="0"')), "", i.ariaAttr({
                                role: "presentation"
                            })]
                        }
                    })]
                }
            })), a.klass.table, 'id="' + t.$node[0].id + '_table" ' + i.ariaAttr({
                role: "grid",
                controls: t.$node[0].id,
                readonly: !0
            })), a.klass.calendar_container) + i.node("div", i.node("button", a.today, a.klass.buttonToday, "type=button data-pick=" + o.pick + (e && !t.disabled(o) ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })) + i.node("button", a.clear, a.klass.buttonClear, "type=button data-clear=1" + (e ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })) + i.node("button", a.close, a.klass.buttonClose, "type=button data-close=true " + (e ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })), a.klass.footer), "picker__container__wrapper")
        }, a.prototype.formats = function() {
            function e(e, t, i) {
                var a = e.match(/\w+/)[0];
                return i.mm || i.m || (i.m = t.indexOf(a) + 1), a.length
            }

            function t(e) {
                return e.match(/\w+/)[0].length
            }
            return {
                d: function(e, t) {
                    return e ? i.digits(e) : t.date
                },
                dd: function(e, t) {
                    return e ? 2 : i.lead(t.date)
                },
                ddd: function(e, i) {
                    return e ? t(e) : this.settings.weekdaysShort[i.day]
                },
                dddd: function(e, i) {
                    return e ? t(e) : this.settings.weekdaysFull[i.day]
                },
                m: function(e, t) {
                    return e ? i.digits(e) : t.month + 1
                },
                mm: function(e, t) {
                    return e ? 2 : i.lead(t.month + 1)
                },
                mmm: function(t, i) {
                    var a = this.settings.monthsShort;
                    return t ? e(t, a, i) : a[i.month]
                },
                mmmm: function(t, i) {
                    var a = this.settings.monthsFull;
                    return t ? e(t, a, i) : a[i.month]
                },
                yy: function(e, t) {
                    return e ? 2 : ("" + t.year).slice(2)
                },
                yyyy: function(e, t) {
                    return e ? 4 : t.year
                },
                toArray: function(e) {
                    return e.split(/(d{1,4}|m{1,4}|y{4}|yy|!.)/g)
                },
                toString: function(e, t) {
                    var a = this;
                    return a.formats.toArray(e).map(function(e) {
                        return i.trigger(a.formats[e], a, [0, t]) || e.replace(/^!/, "")
                    }).join("")
                }
            }
        }(), a.defaults = function(e) {
            return {
                labelMonthNext: "Proximo",
                labelMonthPrev: "Anterior",
                labelMonthSelect: "Escolha um mês",
                labelYearSelect: "Escolha um ano",
                monthsFull: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
                monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
                weekdaysFull: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
                weekdaysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
                weekdaysLetter: ["D", "S", "T", "Q", "Q", "S", "S"],
                today: "Hoje",
                clear: "Limpar",
                close: "Fechar",
                closeOnSelect: !0,
                format: "dd/mm/yyyy",
                klass: {
                    table: e + "table",
                    header: e + "header",
                    date_display: e + "date-display",
                    day_display: e + "day-display",
                    weekday_display: e + "weekday-display",
                    month_display: e + "month-display",
                    year_display: e + "year-display",
                    calendar_container: e + "calendar-container",
                    navPrev: e + "nav--prev",
                    navNext: e + "nav--next",
                    navDisabled: e + "nav--disabled",
                    month: e + "month",
                    year: e + "year",
                    selectMonth: e + "select--month",
                    selectYear: e + "select--year",
                    weekdays: e + "weekday",
                    day: e + "day",
                    disabled: e + "day--disabled",
                    selected: e + "day--selected",
                    highlighted: e + "day--highlighted",
                    now: e + "day--today",
                    infocus: e + "day--infocus",
                    outfocus: e + "day--outfocus",
                    footer: e + "footer",
                    buttonClear: e + "clear btn-flat",
                    buttonToday: e + "today btn-flat",
                    buttonClose: e + "close btn-flat"
                }
            }
        }(e.klasses().picker + "__"), e.extend("pickadate", a)
    }(i(53), i(1))
}, function(e, t, i) {
    (function(e) {
        /*!
         * ClockPicker v0.0.7 (http://weareoutman.github.io/clockpicker/)
         * Copyright 2014 Wang Shenwei.
         * Licensed under MIT (https://github.com/weareoutman/clockpicker/blob/gh-pages/LICENSE)
         *
         * Further modified
         * Copyright 2015 Ching Yaw Hao.
         */
        var t = e(window),
            i = e(document),
            a = "http://www.w3.org/2000/svg",
            n = "SVGAngle" in window && function() {
                var e, t = document.createElement("div");
                return t.innerHTML = "<svg/>", e = (t.firstChild && t.firstChild.namespaceURI) == a, t.innerHTML = "", e
            }(),
            o = function() {
                var e = document.createElement("div").style;
                return "transition" in e || "WebkitTransition" in e || "MozTransition" in e || "msTransition" in e || "OTransition" in e
            }(),
            s = "ontouchstart" in window,
            r = "mousedown" + (s ? " touchstart" : ""),
            c = "mousemove.clockpicker" + (s ? " touchmove.clockpicker" : ""),
            l = "mouseup.clockpicker" + (s ? " touchend.clockpicker" : ""),
            d = navigator.vibrate ? "vibrate" : navigator.webkitVibrate ? "webkitVibrate" : null;

        function u(e) {
            return document.createElementNS(a, e)
        }

        function p(e) {
            return (e < 10 ? "0" : "") + e
        }
        var f = 0;
        var h = 135,
            m = 105,
            b = 20,
            _ = 2 * h,
            v = o ? 350 : 1,
            g = ['<div class="clockpicker picker">', '<div class="picker__holder">', '<div class="picker__frame">', '<div class="picker__wrap">', '<div class="picker__box">', '<div class="picker__date-display">', '<div class="clockpicker-display">', '<div class="clockpicker-display-column">', '<span class="clockpicker-span-hours text-primary"></span>', ":", '<span class="clockpicker-span-minutes"></span>', "</div>", '<div class="clockpicker-display-column clockpicker-display-am-pm">', '<div class="clockpicker-span-am-pm"></div>', "</div>", "</div>", "</div>", '<div class="picker__container__wrapper">', '<div class="picker__calendar-container">', '<div class="clockpicker-plate">', '<div class="clockpicker-canvas"></div>', '<div class="clockpicker-dial clockpicker-hours"></div>', '<div class="clockpicker-dial clockpicker-minutes clockpicker-dial-out"></div>', "</div>", '<div class="clockpicker-am-pm-block">', "</div>", "</div>", '<div class="picker__footer">', "</div>", "</div>", "</div>", "</div>", "</div>", "</div>", "</div>"].join(""),
            y = function(t, a) {
                var o = e(g),
                    s = o.find(".clockpicker-plate"),
                    d = o.find(".picker__holder"),
                    y = o.find(".clockpicker-hours"),
                    w = o.find(".clockpicker-minutes"),
                    O = o.find(".clockpicker-am-pm-block"),
                    j = "INPUT" === t.prop("tagName"),
                    E = j ? t : t.find("input"),
                    C = e("label[for=" + E.attr("id") + "]"),
                    D = this;
                this.id = function(e) {
                    var t = "" + ++f;
                    return e ? e + t : t
                }("cp"), this.element = t, this.holder = d, this.options = a, this.isAppended = !1, this.isShown = !1, this.currentView = "hours", this.isInput = j, this.input = E, this.label = C, this.popover = o, this.plate = s, this.hoursView = y, this.minutesView = w, this.amPmBlock = O, this.spanHours = o.find(".clockpicker-span-hours"), this.spanMinutes = o.find(".clockpicker-span-minutes"), this.spanAmPm = o.find(".clockpicker-span-am-pm"), this.footer = o.find(".picker__footer"), this.amOrPm = "PM", a.twelvehour && (a.ampmclickable ? (this.spanAmPm.empty(), e('<div id="click-am">AM</div>').on("click", function() {
                    D.spanAmPm.children("#click-am").addClass("text-primary"), D.spanAmPm.children("#click-pm").removeClass("text-primary"), D.amOrPm = "AM"
                }).appendTo(this.spanAmPm), e('<div id="click-pm">PM</div>').on("click", function() {
                    D.spanAmPm.children("#click-pm").addClass("text-primary"), D.spanAmPm.children("#click-am").removeClass("text-primary"), D.amOrPm = "PM"
                }).appendTo(this.spanAmPm)) : (this.spanAmPm.empty(), e('<div id="click-am">AM</div>').appendTo(this.spanAmPm), e('<div id="click-pm">PM</div>').appendTo(this.spanAmPm))), e('<button type="button" class="btn-flat picker__clear" tabindex="' + (a.twelvehour ? "3" : "1") + '">' + a.cleartext + "</button>").click(e.proxy(this.clear, this)).appendTo(this.footer), e('<button type="button" class="btn-flat picker__close" tabindex="' + (a.twelvehour ? "3" : "1") + '">' + a.canceltext + "</button>").click(e.proxy(this.hide, this)).appendTo(this.footer), e('<button type="button" class="btn-flat picker__close" tabindex="' + (a.twelvehour ? "3" : "1") + '">' + a.donetext + "</button>").click(e.proxy(this.done, this)).appendTo(this.footer), this.spanHours.click(e.proxy(this.toggleView, this, "hours")), this.spanMinutes.click(e.proxy(this.toggleView, this, "minutes")), E.on("focus.clockpicker click.clockpicker", e.proxy(this.show, this));
                var M, x, P, T, A = e('<div class="clockpicker-tick"></div>');
                if (a.twelvehour)
                    for (M = 1; M < 13; M += 1) x = A.clone(), P = M / 6 * Math.PI, T = m, x.css({
                        left: h + Math.sin(P) * T - b,
                        top: h - Math.cos(P) * T - b
                    }), x.html(0 === M ? "00" : M), y.append(x), x.on(r, $);
                else
                    for (M = 0; M < 24; M += 1) {
                        x = A.clone(), P = M / 6 * Math.PI, T = M > 0 && M < 13 ? 70 : m, x.css({
                            left: h + Math.sin(P) * T - b,
                            top: h - Math.cos(P) * T - b
                        }), x.html(0 === M ? "00" : M), y.append(x), x.on(r, $)
                    }
                for (M = 0; M < 60; M += 5) x = A.clone(), P = M / 30 * Math.PI, x.css({
                    left: h + Math.sin(P) * m - b,
                    top: h - Math.cos(P) * m - b
                }), x.html(p(M)), w.append(x), x.on(r, $);

                function $(e, t) {
                    var n = s.offset(),
                        o = /^touch/.test(e.type),
                        r = n.left + h,
                        d = n.top + h,
                        u = (o ? e.originalEvent.touches[0] : e).pageX - r,
                        p = (o ? e.originalEvent.touches[0] : e).pageY - d,
                        f = Math.sqrt(u * u + p * p),
                        _ = !1;
                    if (!t || !(f < m - b || f > m + b)) {
                        e.preventDefault();
                        var g = setTimeout(function() {
                            D.popover.addClass("clockpicker-moving")
                        }, 200);
                        D.setHand(u, p, !t, !0), i.off(c).on(c, function(e) {
                            e.preventDefault();
                            var t = /^touch/.test(e.type),
                                i = (t ? e.originalEvent.touches[0] : e).pageX - r,
                                a = (t ? e.originalEvent.touches[0] : e).pageY - d;
                            (_ || i !== u || a !== p) && (_ = !0, D.setHand(i, a, !1, !0))
                        }), i.off(l).on(l, function(e) {
                            i.off(l), e.preventDefault();
                            var n = /^touch/.test(e.type),
                                o = (n ? e.originalEvent.changedTouches[0] : e).pageX - r,
                                f = (n ? e.originalEvent.changedTouches[0] : e).pageY - d;
                            (t || _) && o === u && f === p && D.setHand(o, f), "hours" === D.currentView ? D.toggleView("minutes", v / 2) : a.autoclose && (D.minutesView.addClass("clockpicker-dial-out"), setTimeout(function() {
                                D.done()
                            }, v / 2)), s.prepend(I), clearTimeout(g), D.popover.removeClass("clockpicker-moving"), i.off(c)
                        })
                    }
                }
                if (s.on(r, function(t) {
                        0 === e(t.target).closest(".clockpicker-tick").length && $(t, !0)
                    }), n) {
                    var I = o.find(".clockpicker-canvas"),
                        L = u("svg");
                    L.setAttribute("class", "clockpicker-svg"), L.setAttribute("width", _), L.setAttribute("height", _);
                    var R = u("g");
                    R.setAttribute("transform", "translate(" + h + "," + h + ")");
                    var S = u("circle");
                    S.setAttribute("class", "clockpicker-canvas-bearing"), S.setAttribute("cx", 0), S.setAttribute("cy", 0), S.setAttribute("r", 4);
                    var W = u("line");
                    W.setAttribute("x1", 0), W.setAttribute("y1", 0);
                    var F = u("circle");
                    F.setAttribute("class", "clockpicker-canvas-bg"), F.setAttribute("r", b), R.appendChild(W), R.appendChild(F), R.appendChild(S), L.appendChild(R), I.append(L), this.hand = W, this.bg = F, this.bearing = S, this.g = R, this.canvas = I
                }
                k(this.options.init)
            };

        function k(e) {
            e && "function" == typeof e && e()
        }
        y.prototype.toggle = function() {
            this[this.isShown ? "hide" : "show"]()
        }, y.prototype.locate = function() {
            this.popover.show()
        }, y.prototype.show = function() {
            if (!this.isShown) {
                k(this.options.beforeShow), e(":input").each(function() {
                    e(this).attr("tabindex", -1)
                });
                var a = this;
                this.input.blur(), this.popover.addClass("picker--opened"), this.input.addClass("picker__input picker__input--active"), e(document.body).css("overflow", "hidden");
                var n = ("" + (this.input.prop("value") || this.options.default || "")).split(":");
                if (this.options.twelvehour && void 0 !== n[1] && (n[1].indexOf("AM") > 0 ? this.amOrPm = "AM" : this.amOrPm = "PM", n[1] = n[1].replace("AM", "").replace("PM", "")), "now" === n[0]) {
                    var o = new Date(+new Date + this.options.fromnow);
                    n = [o.getHours(), o.getMinutes()], this.options.twelvehour && (this.amOrPm = n[0] >= 12 && n[0] < 24 ? "PM" : "AM")
                }
                if (this.hours = +n[0] || 0, this.minutes = +n[1] || 0, this.spanHours.html(this.hours), this.spanMinutes.html(p(this.minutes)), !this.isAppended) {
                    var s = document.querySelector(this.options.container);
                    this.options.container && s ? s.appendChild(this.popover[0]) : this.popover.insertAfter(this.input), this.options.twelvehour && ("PM" === this.amOrPm ? (this.spanAmPm.children("#click-pm").addClass("text-primary"), this.spanAmPm.children("#click-am").removeClass("text-primary")) : (this.spanAmPm.children("#click-am").addClass("text-primary"), this.spanAmPm.children("#click-pm").removeClass("text-primary"))), t.on("resize.clockpicker" + this.id, function() {
                        a.isShown && a.locate()
                    }), this.isAppended = !0
                }
                this.toggleView("hours"), this.locate(), this.isShown = !0, i.on("click.clockpicker." + this.id + " focusin.clockpicker." + this.id, function(t) {
                    var i = e(t.target);
                    0 === i.closest(a.popover.find(".picker__wrap")).length && 0 === i.closest(a.input).length && a.hide()
                }), i.on("keyup.clockpicker." + this.id, function(e) {
                    27 === e.keyCode && a.hide()
                }), k(this.options.afterShow)
            }
        }, y.prototype.hide = function() {
            k(this.options.beforeHide), this.input.removeClass("picker__input picker__input--active"), this.popover.removeClass("picker--opened"), e(document.body).css("overflow", "visible"), this.isShown = !1, e(":input").each(function(t) {
                e(this).attr("tabindex", t + 1)
            }), i.off("click.clockpicker." + this.id + " focusin.clockpicker." + this.id), i.off("keyup.clockpicker." + this.id), this.popover.hide(), k(this.options.afterHide)
        }, y.prototype.toggleView = function(t, i) {
            var a = !1;
            "minutes" === t && "visible" === e(this.hoursView).css("visibility") && (k(this.options.beforeHourSelect), a = !0);
            var n = "hours" === t,
                o = n ? this.hoursView : this.minutesView,
                s = n ? this.minutesView : this.hoursView;
            this.currentView = t, this.spanHours.toggleClass("text-primary", n), this.spanMinutes.toggleClass("text-primary", !n), s.addClass("clockpicker-dial-out"), o.css("visibility", "visible").removeClass("clockpicker-dial-out"), this.resetClock(i), clearTimeout(this.toggleViewTimer), this.toggleViewTimer = setTimeout(function() {
                s.css("visibility", "hidden")
            }, v), a && k(this.options.afterHourSelect)
        }, y.prototype.resetClock = function(e) {
            var t = this.currentView,
                i = this[t],
                a = "hours" === t,
                o = i * (Math.PI / (a ? 6 : 30)),
                s = a && i > 0 && i < 13 ? 70 : m,
                r = Math.sin(o) * s,
                c = -Math.cos(o) * s,
                l = this;
            n && e ? (l.canvas.addClass("clockpicker-canvas-out"), setTimeout(function() {
                l.canvas.removeClass("clockpicker-canvas-out"), l.setHand(r, c)
            }, e)) : this.setHand(r, c)
        }, y.prototype.setHand = function(t, i, a) {
            var o, s = Math.atan2(t, -i),
                r = "hours" === this.currentView,
                c = Math.PI / (r || a ? 6 : 30),
                l = Math.sqrt(t * t + i * i),
                u = this.options,
                f = r && l < (m + 70) / 2,
                h = f ? 70 : m;
            if (u.twelvehour && (h = m), s < 0 && (s = 2 * Math.PI + s), s = (o = Math.round(s / c)) * c, u.twelvehour ? r ? 0 === o && (o = 12) : (a && (o *= 5), 60 === o && (o = 0)) : r ? (12 === o && (o = 0), o = f ? 0 === o ? 12 : o : 0 === o ? 0 : o + 12) : (a && (o *= 5), 60 === o && (o = 0)), this[this.currentView] !== o && d && this.options.vibrate && (this.vibrateTimer || (navigator[d](10), this.vibrateTimer = setTimeout(e.proxy(function() {
                    this.vibrateTimer = null
                }, this), 100))), this[this.currentView] = o, r ? this.spanHours.html(o) : this.spanMinutes.html(p(o)), n) {
                var _ = Math.sin(s) * (h - b),
                    v = -Math.cos(s) * (h - b),
                    g = Math.sin(s) * h,
                    y = -Math.cos(s) * h;
                this.hand.setAttribute("x2", _), this.hand.setAttribute("y2", v), this.bg.setAttribute("cx", g), this.bg.setAttribute("cy", y)
            } else this[r ? "hoursView" : "minutesView"].find(".clockpicker-tick").each(function() {
                var t = e(this);
                t.toggleClass("active", o === +t.html())
            })
        }, y.prototype.done = function() {
            k(this.options.beforeDone), this.hide(), this.label.addClass("active");
            var e = this.input.prop("value"),
                t = p(this.hours) + ":" + p(this.minutes);
            this.options.twelvehour && (t += this.amOrPm), this.input.prop("value", t), t !== e && (this.input.triggerHandler("change"), this.isInput || this.element.trigger("change")), this.options.autoclose && this.input.trigger("blur"), k(this.options.afterDone)
        }, y.prototype.clear = function() {
            this.hide(), this.label.removeClass("active");
            var e = this.input.prop("value");
            this.input.prop("value", ""), "" !== e && (this.input.triggerHandler("change"), this.isInput || this.element.trigger("change")), this.options.autoclose && this.input.trigger("blur")
        }, y.prototype.remove = function() {
            this.element.removeData("clockpicker"), this.input.off("focus.clockpicker click.clockpicker"), this.isShown && this.hide(), this.isAppended && (t.off("resize.clockpicker" + this.id), this.popover.remove())
        }, y.DEFAULTS = {
            default: "",
            fromnow: 0,
            donetext: "Fechar",
            cleartext: "Limpar",
            canceltext: "Cancelar",
            autoclose: !0,
            ampmclickable: !0,
            darktheme: !1,
            twelvehour: !1,
            vibrate: !0
        }, e.fn.pickatime = function(t) {
            var i = Array.prototype.slice.call(arguments, 1);
            return this.each(function() {
                var a = e(this),
                    n = a.data("clockpicker");
                if (n) "function" == typeof n[t] && n[t].apply(n, i);
                else {
                    var o = e.extend({}, y.DEFAULTS, a.data(), "object" == typeof t && t);
                    a.data("clockpicker", new y(a, o))
                }
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    /*!
     * Date picker for pickadate.js v3.5.0
     * http://amsul.github.io/pickadate.js/date.htm
     */
    e.exports = function(e, t) {
        var i = e._,
            a = function(e, t) {
                var i = this,
                    a = e.$node[0],
                    n = a.value,
                    o = e.$node.data("value"),
                    s = o || n,
                    r = o ? t.formatSubmit : t.format,
                    c = function() {
                        return a.currentStyle ? "rtl" == a.currentStyle.direction : "rtl" == getComputedStyle(e.$root[0]).direction
                    };
                i.settings = t, i.$node = e.$node, i.queue = {
                    min: "measure create",
                    max: "measure create",
                    now: "now create",
                    select: "parse create validate",
                    highlight: "parse navigate create validate",
                    view: "parse create validate viewset",
                    disable: "deactivate",
                    enable: "activate"
                }, i.item = {}, i.item.clear = null, i.item.disable = (t.disable || []).slice(0), i.item.enable = - function(e) {
                    return !0 === e[0] ? e.shift() : -1
                }(i.item.disable), i.set("min", t.min).set("max", t.max).set("now"), s ? i.set("select", s, {
                    format: r
                }) : i.set("select", null).set("highlight", i.item.now), i.key = {
                    40: 7,
                    38: -7,
                    39: function() {
                        return c() ? -1 : 1
                    },
                    37: function() {
                        return c() ? 1 : -1
                    },
                    go: function(e) {
                        var t = i.item.highlight,
                            a = new Date(t.year, t.month, t.date + e);
                        i.set("highlight", a, {
                            interval: e
                        }), this.render()
                    }
                }, e.on("render", function() {
                    e.$root.find("." + t.klass.selectMonth).on("change", function() {
                        var i = this.value;
                        i && (e.set("highlight", [e.get("view").year, i, e.get("highlight").date]), e.$root.find("." + t.klass.selectMonth).trigger("focus"))
                    }), e.$root.find("." + t.klass.selectYear).on("change", function() {
                        var i = this.value;
                        i && (e.set("highlight", [i, e.get("view").month, e.get("highlight").date]), e.$root.find("." + t.klass.selectYear).trigger("focus"))
                    })
                }, 1).on("open", function() {
                    var a = "";
                    i.disabled(i.get("now")) && (a = ":not(." + t.klass.buttonToday + ")"), e.$root.find("button" + a + ", select").attr("disabled", !1)
                }, 1).on("close", function() {
                    e.$root.find("button, select").attr("disabled", !0)
                }, 1)
            };
        a.prototype.set = function(e, t, i) {
            var a = this,
                n = a.item;
            return null === t ? ("clear" == e && (e = "select"), n[e] = t, a) : (n["enable" == e ? "disable" : "flip" == e ? "enable" : e] = a.queue[e].split(" ").map(function(n) {
                return t = a[n](e, t, i)
            }).pop(), "select" == e ? a.set("highlight", n.select, i) : "highlight" == e ? a.set("view", n.highlight, i) : e.match(/^(flip|min|max|disable|enable)$/) && (n.select && a.disabled(n.select) && a.set("select", n.select, i), n.highlight && a.disabled(n.highlight) && a.set("highlight", n.highlight, i)), a)
        }, a.prototype.get = function(e) {
            return this.item[e]
        }, a.prototype.create = function(e, a, n) {
            var o;
            return (a = void 0 === a ? e : a) == -1 / 0 || a == 1 / 0 ? o = a : t.isPlainObject(a) && i.isInteger(a.pick) ? a = a.obj : t.isArray(a) ? (a = new Date(a[0], a[1], a[2]), a = i.isDate(a) ? a : this.create().obj) : a = i.isInteger(a) || i.isDate(a) ? this.normalize(new Date(a), n) : this.now(e, a, n), {
                year: o || a.getFullYear(),
                month: o || a.getMonth(),
                date: o || a.getDate(),
                day: o || a.getDay(),
                obj: o || a,
                pick: o || a.getTime()
            }
        }, a.prototype.createRange = function(e, a) {
            var n = this,
                o = function(e) {
                    return !0 === e || t.isArray(e) || i.isDate(e) ? n.create(e) : e
                };
            return i.isInteger(e) || (e = o(e)), i.isInteger(a) || (a = o(a)), i.isInteger(e) && t.isPlainObject(a) ? e = [a.year, a.month, a.date + e] : i.isInteger(a) && t.isPlainObject(e) && (a = [e.year, e.month, e.date + a]), {
                from: o(e),
                to: o(a)
            }
        }, a.prototype.withinRange = function(e, t) {
            return e = this.createRange(e.from, e.to), t.pick >= e.from.pick && t.pick <= e.to.pick
        }, a.prototype.overlapRanges = function(e, t) {
            return e = this.createRange(e.from, e.to), t = this.createRange(t.from, t.to), this.withinRange(e, t.from) || this.withinRange(e, t.to) || this.withinRange(t, e.from) || this.withinRange(t, e.to)
        }, a.prototype.now = function(e, t, i) {
            var a = new Date;
            return t = new Date(a.getFullYear(), a.getMonth(), 1), i && i.rel && t.setDate(t.getDate() + i.rel), this.normalize(t, i)
        }, a.prototype.navigate = function(e, i, a) {
            var n, o, s, r = t.isArray(i),
                c = t.isPlainObject(i),
                l = this.item.view;
            return (r || c) && (c ? (o = i.year, s = i.month) : (o = +i[0], s = +i[1]), a && a.nav && l && l.month !== s && (o = l.year, s = l.month), n = new Date(o + (a && a.nav ? a.nav : 0), s, 1), o = n.getFullYear(), s = n.getMonth(), i = [o, s, 1]), i
        }, a.prototype.normalize = function(e) {
            return e.setHours(0, 0, 0, 0), e
        }, a.prototype.measure = function(e, t) {
            return t ? "string" == typeof t ? t = this.parse(e, t) : i.isInteger(t) && (t = this.now(e, t, {
                rel: t
            })) : t = "min" == e ? -1 / 0 : 1 / 0, t
        }, a.prototype.viewset = function(e, t) {
            return this.create([t.year, t.month, 1])
        }, a.prototype.validate = function(e, a, n) {
            var o, s, r, c, l = this,
                d = a,
                u = n && n.interval ? n.interval : 1,
                p = -1 === l.item.enable,
                f = l.item.min,
                h = l.item.max,
                m = p && l.item.disable.filter(function(e) {
                    if (t.isArray(e)) {
                        var n = l.create(e).pick;
                        n < a.pick ? o = !0 : n > a.pick && (s = !0)
                    }
                    return i.isInteger(e)
                }).length;
            if ((!n || !n.nav) && (!p && l.disabled(a) || p && l.disabled(a) && (m || o || s) || !p && (a.pick <= f.pick || a.pick >= h.pick)))
                for (p && !m && (!s && u > 0 || !o && u < 0) && (u *= -1); l.disabled(a) && (Math.abs(u) > 1 && (a.month < d.month || a.month > d.month) && (a = d, u = u > 0 ? 1 : -1), a.pick <= f.pick ? (r = !0, u = 1, a = l.create([f.year, f.month, f.date + (a.pick === f.pick ? 0 : -1)])) : a.pick >= h.pick && (c = !0, u = -1, a = l.create([h.year, h.month, h.date + (a.pick === h.pick ? 0 : 1)])), !r || !c);) a = l.create([a.year, a.month, a.date + u]);
            return a
        }, a.prototype.disabled = function(e) {
            var a = this,
                n = a.item.disable.filter(function(n) {
                    return i.isInteger(n) ? e.day === (a.settings.firstDay ? n : n - 1) % 7 : t.isArray(n) || i.isDate(n) ? e.pick === a.create(n).pick : t.isPlainObject(n) ? a.withinRange(n, e) : void 0
                });
            return n = n.length && !n.filter(function(e) {
                return t.isArray(e) && "inverted" == e[3] || t.isPlainObject(e) && e.inverted
            }).length, -1 === a.item.enable ? !n : n || e.pick < a.item.min.pick || e.pick > a.item.max.pick
        }, a.prototype.parse = function(e, t, a) {
            var n = this,
                o = {};
            return t && "string" == typeof t ? (a && a.format || ((a = a || {}).format = n.settings.format), n.formats.toArray(a.format).map(function(e) {
                var a = n.formats[e],
                    s = a ? i.trigger(a, n, [t, o]) : e.replace(/^!/, "").length;
                a && (o[e] = t.substr(0, s)), t = t.substr(s)
            }), [o.yyyy || o.yy, +(o.mm || o.m) - 1, o.dd || o.d]) : t
        }, a.prototype.isDateExact = function(e, a) {
            return i.isInteger(e) && i.isInteger(a) || "boolean" == typeof e && "boolean" == typeof a ? e === a : (i.isDate(e) || t.isArray(e)) && (i.isDate(a) || t.isArray(a)) ? this.create(e).pick === this.create(a).pick : !(!t.isPlainObject(e) || !t.isPlainObject(a)) && this.isDateExact(e.from, a.from) && this.isDateExact(e.to, a.to)
        }, a.prototype.isDateOverlap = function(e, a) {
            var n = this.settings.firstDay ? 1 : 0;
            return i.isInteger(e) && (i.isDate(a) || t.isArray(a)) ? (e = e % 7 + n) === this.create(a).day + 1 : i.isInteger(a) && (i.isDate(e) || t.isArray(e)) ? (a = a % 7 + n) === this.create(e).day + 1 : !(!t.isPlainObject(e) || !t.isPlainObject(a)) && this.overlapRanges(e, a)
        }, a.prototype.flipEnable = function(e) {
            var t = this.item;
            t.enable = e || (-1 == t.enable ? 1 : -1)
        }, a.prototype.deactivate = function(e, a) {
            var n = this,
                o = n.item.disable.slice(0);
            return "flip" == a ? n.flipEnable() : !1 === a ? (n.flipEnable(1), o = []) : !0 === a ? (n.flipEnable(-1), o = []) : a.map(function(e) {
                for (var a, s = 0; s < o.length; s += 1)
                    if (n.isDateExact(e, o[s])) {
                        a = !0;
                        break
                    } a || (i.isInteger(e) || i.isDate(e) || t.isArray(e) || t.isPlainObject(e) && e.from && e.to) && o.push(e)
            }), o
        }, a.prototype.activate = function(e, a) {
            var n = this,
                o = n.item.disable,
                s = o.length;
            return "flip" == a ? n.flipEnable() : !0 === a ? (n.flipEnable(1), o = []) : !1 === a ? (n.flipEnable(-1), o = []) : a.map(function(e) {
                var a, r, c, l;
                for (c = 0; c < s; c += 1) {
                    if (r = o[c], n.isDateExact(r, e)) {
                        a = o[c] = null, l = !0;
                        break
                    }
                    if (n.isDateOverlap(r, e)) {
                        t.isPlainObject(e) ? (e.inverted = !0, a = e) : t.isArray(e) ? (a = e)[3] || a.push("inverted") : i.isDate(e) && (a = [e.getFullYear(), e.getMonth(), e.getDate(), "inverted"]);
                        break
                    }
                }
                if (a)
                    for (c = 0; c < s; c += 1)
                        if (n.isDateExact(o[c], e)) {
                            o[c] = null;
                            break
                        } if (l)
                    for (c = 0; c < s; c += 1)
                        if (n.isDateOverlap(o[c], e)) {
                            o[c] = null;
                            break
                        } a && o.push(a)
            }), o.filter(function(e) {
                return null != e
            })
        }, a.prototype.nodes = function(e) {
            var t = this,
                a = t.settings,
                n = t.item,
                o = n.now,
                s = n.select,
                r = n.highlight,
                c = n.view,
                l = n.disable,
                d = n.min,
                u = n.max,
                p = function(e, t) {
                    return a.firstDay && (e.push(e.shift()), t.push(t.shift())), i.node("thead", i.node("tr", i.group({
                        min: 0,
                        max: 2,
                        i: 1,
                        node: "th",
                        item: function(e) {
                            return [" ", a.klass.weekdays, 'scope=col title="' + t[e] + '"']
                        }
                    })))
                }((a.showWeekdaysFull ? a.weekdaysFull : a.weekdaysLetter).slice(0), a.weekdaysFull.slice(0)),
                f = function(e) {
                    return i.node("div", " ", a.klass["nav" + (e ? "Next" : "Prev")] + (e && c.year >= u.year && c.month >= u.month || !e && c.year <= d.year && c.month <= d.month ? " " + a.klass.navDisabled : ""), "data-nav=" + (e || -1) + " " + i.ariaAttr({
                        role: "button",
                        controls: t.$node[0].id + "_table"
                    }) + ' title="' + (e ? a.labelMonthNext : a.labelMonthPrev) + '"')
                },
                h = function(n) {
                    var o = c.year,
                        r = !0 === a.selectYears ? 5 : ~~(a.selectYears / 2);
                    if (r) {
                        var l = d.year,
                            p = u.year,
                            f = o - r,
                            h = o + r;
                        if (l > f && (h += l - f, f = l), p < h) {
                            var m = f - l,
                                b = h - p;
                            f -= m > b ? b : m, h = p
                        }
                        if (a.selectYears && void 0 == n) return i.node("select", i.group({
                            min: f,
                            max: h,
                            i: 1,
                            node: "option",
                            item: function(e) {
                                return [e, 0, "value=" + e + (o == e ? " selected" : "")]
                            }
                        }), a.klass.selectYear + " browser-default", (e ? "" : "disabled") + " " + i.ariaAttr({
                            controls: t.$node[0].id + "_table"
                        }) + ' title="' + a.labelYearSelect + '"')
                    }
                    return "raw" === n && null != s ? i.node("div", s.year) : i.node("div", o, a.klass.year)
                };
            return i.node("div", i.node("div", h("raw"), a.klass.year_display) + i.node("span", function(n) {
                var o = a.showMonthsShort ? a.monthsShort : a.monthsFull;
                return "full_months" == n && (o = a.monthsFull), a.selectMonths && void 0 == n ? i.node("select", i.group({
                    min: 0,
                    max: 11,
                    i: 1,
                    node: "option",
                    item: function(e) {
                        return [o[e], 0, "value=" + e + (c.month == e ? " selected" : "") + (c.year == d.year && e < d.month || c.year == u.year && e > u.month ? " disabled" : "")]
                    }
                }), a.klass.selectMonth + " ", (e ? "" : "disabled") + " " + i.ariaAttr({
                    controls: t.$node[0].id + "_table"
                }) + ' title="' + a.labelMonthSelect + '"') : "full_months" == n ? null != s ? o[s.month] : o[c.month] : i.node("div", o[c.month], a.klass.month)
            }("full_months") + " ", a.klass.month_display), a.klass.date_display) + i.node("div", i.node("div", i.node("div", (a.selectYears, h() + f() + f(1)), a.klass.header) + i.node("table", p + i.node("tbody", i.group({
                min: 0,
                max: 3,
                i: 1,
                node: "tr",
                item: function(e) {
                    return [i.group({
                        min: 3 * e + 1,
                        max: function() {
                            return this.min + 3 - 1
                        },
                        i: 1,
                        node: "td",
                        item: function(e) {
                            var n = e - 1;
                            e = t.create([c.year, e - 1, 1]);
                            var p = s && s.year == e.year && s.month == e.month,
                                f = r && r.pick == e.pick,
                                h = l && t.disabled(e) || e.pick < d.pick || e.pick > u.pick,
                                m = i.trigger(t.formats.toString, t, [a.format, e]);
                            return [i.node("div", a.monthsShort[n], function(t) {
                                return t.push(c.year == e.year ? a.klass.infocus : a.klass.outfocus), o.pick == e.pick && t.push(a.klass.now), p && t.push(a.klass.selected), f && t.push(a.klass.highlighted), h && t.push(a.klass.disabled), t.join(" ")
                            }([a.klass.month]), "data-pick=" + e.pick + " " + i.ariaAttr({
                                role: "gridcell",
                                label: m,
                                selected: !(!p || t.$node.val() !== m) || null,
                                activedescendant: !!f || null,
                                disabled: !!h || null
                            }) + " " + (h ? "" : 'tabindex="0"')), "", i.ariaAttr({
                                role: "presentation"
                            })]
                        }
                    })]
                }
            })), a.klass.table, 'id="' + t.$node[0].id + '_table" ' + i.ariaAttr({
                role: "grid",
                controls: t.$node[0].id,
                readonly: !0
            })), a.klass.calendar_container) + i.node("div", i.node("button", a.today, a.klass.buttonToday, "type=button data-pick=" + o.pick + (e && !t.disabled(o) ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })) + i.node("button", a.clear, a.klass.buttonClear, "type=button data-clear=1" + (e ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })) + i.node("button", a.close, a.klass.buttonClose, "type=button data-close=true " + (e ? "" : " disabled") + " " + i.ariaAttr({
                controls: t.$node[0].id
            })), a.klass.footer), "picker__container__wrapper")
        }, a.prototype.formats = function() {
            function e(e, t, i) {
                var a = e.match(/\w+/)[0];
                return i.mm || i.m || (i.m = t.indexOf(a) + 1), a.length
            }

            function t(e) {
                return e.match(/\w+/)[0].length
            }
            return {
                d: function(e, t) {
                    return e ? i.digits(e) : t.date
                },
                dd: function(e, t) {
                    return e ? 2 : i.lead(t.date)
                },
                ddd: function(e, i) {
                    return e ? t(e) : this.settings.weekdaysShort[i.day]
                },
                dddd: function(e, i) {
                    return e ? t(e) : this.settings.weekdaysFull[i.day]
                },
                m: function(e, t) {
                    return e ? i.digits(e) : t.month + 1
                },
                mm: function(e, t) {
                    return e ? 2 : i.lead(t.month + 1)
                },
                mmm: function(t, i) {
                    var a = this.settings.monthsShort;
                    return t ? e(t, a, i) : a[i.month]
                },
                mmmm: function(t, i) {
                    var a = this.settings.monthsFull;
                    return t ? e(t, a, i) : a[i.month]
                },
                yy: function(e, t) {
                    return e ? 2 : ("" + t.year).slice(2)
                },
                yyyy: function(e, t) {
                    return e ? 4 : t.year
                },
                toArray: function(e) {
                    return e.split(/(d{1,4}|m{1,4}|y{4}|yy|!.)/g)
                },
                toString: function(e, t) {
                    var a = this;
                    return a.formats.toArray(e).map(function(e) {
                        return i.trigger(a.formats[e], a, [0, t]) || e.replace(/^!/, "")
                    }).join("")
                }
            }
        }(), a.defaults = function(e) {
            return {
                labelMonthNext: "Proximo",
                labelMonthPrev: "Anterior",
                labelMonthSelect: "Escolha um mês",
                labelYearSelect: "Escolha um ano",
                monthsFull: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
                monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
                weekdaysFull: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
                weekdaysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
                weekdaysLetter: ["D", "S", "T", "Q", "Q", "S", "S"],
                today: "Hoje",
                clear: "Limpar",
                close: "Fechar",
                closeOnSelect: !0,
                format: "mmmm, yyyy",
                klass: {
                    table: e + "table",
                    header: e + "header",
                    date_display: e + "date-display",
                    day_display: e + "day-display",
                    month_display: e + "month-display",
                    year_display: e + "year-display",
                    calendar_container: e + "calendar-container",
                    navPrev: e + "nav--prev",
                    navNext: e + "nav--next",
                    navDisabled: e + "nav--disabled",
                    month: e + "month",
                    year: e + "year",
                    selectMonth: e + "select--month",
                    selectYear: e + "select--year",
                    weekdays: e + "weekday",
                    day: e + "day",
                    disabled: e + "day--disabled",
                    selected: e + "day--selected",
                    highlighted: e + "day--highlighted",
                    now: e + "day--today",
                    infocus: e + "day--infocus",
                    outfocus: e + "day--outfocus",
                    footer: e + "footer",
                    buttonClear: e + "clear btn-flat",
                    buttonToday: e + "today btn-flat",
                    buttonClose: e + "close btn-flat"
                }
            }
        }(e.klasses().picker + "__"), e.extend("pickamonth", a)
    }(i(53), i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        i(11);
        var t = function(t, i, a, n) {
            var o = t,
                s = i;
            return o < 0 ? o = 4 : o + a > window.innerWidth && (o -= o + a - window.innerWidth), s < 0 ? s = 4 : s + n > window.innerHeight + e(window).scrollTop && (s -= s + n - window.innerHeight), {
                x: o,
                y: s
            }
        };
        e.fn.tooltip = function(i) {
            return e(".material-tooltip").each(function(t, i) {
                0 == e("[data-tooltip-id=" + e(i).attr("id") + "]").length && e(i).remove()
            }), "remove" === i ? (this.each(function() {
                e("#" + e(this).attr("data-tooltip-id")).remove(), e(this).removeAttr("data-tooltip-id"), e(this).off("mouseenter.tooltip mouseleave.tooltip")
            }), !1) : (i = e.extend({
                delay: 350,
                tooltip: "",
                position: "bottom",
                html: !1
            }, i), this.each(function() {
                var a, n, o, s, r, c, l = Math.random().toString(36).substr(2, 7),
                    d = e(this);
                d.attr("data-tooltip-id") && e("#" + d.attr("data-tooltip-id")).remove(), d.attr("data-tooltip-id", l);
                var u = function() {
                    a = d.attr("data-html") ? "true" === d.attr("data-html") : i.html, n = void 0 === (n = d.attr("data-delay")) || "" === n ? i.delay : n, o = void 0 === (o = d.attr("data-position")) || "" === o ? i.position : o, s = void 0 === (s = d.attr("data-tooltip")) || "" === s ? i.tooltip : s
                };
                u();
                r = function() {
                    var t = e('<div class="material-tooltip"></div>');
                    return s = a ? e("<span></span>").html(s) : e("<span></span>").text(s), t.append(s).appendTo(e("body")).attr("id", l), (c = e('<div class="backdrop"></div>')).appendTo(t), t
                }(), d.off("mouseenter.tooltip mouseleave.tooltip");
                var p, f = !1;
                d.on({
                    "mouseenter.tooltip": function() {
                        p = setTimeout(function() {
                            u(), f = !0, r.velocity("stop"), c.velocity("stop"), r.css({
                                visibility: "visible",
                                left: "0px",
                                top: "0px"
                            });
                            var e, i, a, n, s, l, p = d.outerWidth(),
                                h = d.outerHeight(),
                                m = r.outerHeight(),
                                b = r.outerWidth(),
                                _ = "0px",
                                v = "0px",
                                g = c[0].offsetWidth,
                                y = c[0].offsetHeight;
                            "top" === o ? (n = d.offset().top - m - 5, s = d.offset().left + p / 2 - b / 2, l = t(s, n, b, m), _ = "-10px", c.css({
                                bottom: 0,
                                left: 0,
                                borderRadius: "14px 14px 0 0",
                                transformOrigin: "50% 100%",
                                marginTop: m,
                                marginLeft: b / 2 - g / 2
                            })) : "left" === o ? (n = d.offset().top + h / 2 - m / 2, s = d.offset().left - b - 5, l = t(s, n, b, m), v = "-10px", c.css({
                                top: "-7px",
                                right: 0,
                                width: "14px",
                                height: "14px",
                                borderRadius: "14px 0 0 14px",
                                transformOrigin: "95% 50%",
                                marginTop: m / 2,
                                marginLeft: b
                            })) : "right" === o ? (n = d.offset().top + h / 2 - m / 2, s = d.offset().left + p + 5, l = t(s, n, b, m), v = "+10px", c.css({
                                top: "-7px",
                                left: 0,
                                width: "14px",
                                height: "14px",
                                borderRadius: "0 14px 14px 0",
                                transformOrigin: "5% 50%",
                                marginTop: m / 2,
                                marginLeft: "0px"
                            })) : (n = d.offset().top + d.outerHeight() + 5, s = d.offset().left + p / 2 - b / 2, l = t(s, n, b, m), _ = "+10px", c.css({
                                top: 0,
                                left: 0,
                                marginLeft: b / 2 - g / 2
                            })), r.css({
                                top: l.y,
                                left: l.x
                            }), e = Math.SQRT2 * b / parseInt(g), i = Math.SQRT2 * m / parseInt(y), a = Math.max(e, i), r.velocity({
                                translateY: _,
                                translateX: v
                            }, {
                                duration: 350,
                                queue: !1
                            }).velocity({
                                opacity: 1
                            }, {
                                duration: 300,
                                delay: 50,
                                queue: !1
                            }), c.css({
                                visibility: "visible"
                            }).velocity({
                                opacity: 1
                            }, {
                                duration: 55,
                                delay: 0,
                                queue: !1
                            }).velocity({
                                scaleX: a,
                                scaleY: a
                            }, {
                                duration: 300,
                                delay: 0,
                                queue: !1,
                                easing: "easeInOutQuad"
                            })
                        }, n)
                    },
                    "mouseleave.tooltip": function() {
                        f = !1, clearTimeout(p), setTimeout(function() {
                            !0 !== f && (r.velocity({
                                opacity: 0,
                                translateY: 0,
                                translateX: 0
                            }, {
                                duration: 225,
                                queue: !1
                            }), c.velocity({
                                opacity: 0,
                                scaleX: 1,
                                scaleY: 1
                            }, {
                                duration: 225,
                                queue: !1,
                                complete: function() {
                                    c.css({
                                        visibility: "hidden"
                                    }), r.css({
                                        visibility: "hidden"
                                    }), f = !1
                                }
                            }))
                        }, 225)
                    }
                })
            }))
        }
    }).call(this, i(1))
}, , function(e, t, i) {
    (function(e) {
        ! function(e) {
            e.fn.passwordRules = function(t) {
                var i = {
                    rules: {
                        length: {
                            regex: ".{8,16}",
                            name: "minlength",
                            message: "Entre 8-16 caracteres",
                            enable: !0
                        },
                        lowercase: {
                            regex: "[a-z]{1,}",
                            name: "lowercase",
                            message: "Letra minúscula",
                            enable: !0
                        },
                        uppercase: {
                            regex: "[A-Z]{1,}",
                            name: "uppercase",
                            message: "Letra maiúscula",
                            enable: !0
                        },
                        number: {
                            regex: "[0-9]{1,}",
                            name: "number",
                            message: "Número",
                            enable: !0
                        },
                        specialChar: {
                            regex: "[^a-zA-Z0-9]{1,}",
                            name: "special-char",
                            message: "Caracter especial",
                            enable: !0
                        }
                    },
                    msgRules: "A sua senha precisa ter",
                    container: void 0,
                    containerClass: "",
                    containerId: "checkRulesList",
                    okClass: "valid",
                    koClass: "invalid",
                    onLoad: void 0
                };

                function a(t, i, a) {
                    e.each(n.rules, function(t, o) {
                        o.enable && function(t, i, a, o) {
                            t.test(i) ? e("#" + o + " li." + a).removeClass("ko " + n.koClass).addClass("ok " + n.okClass) : e("#" + o + " li." + a).removeClass("ok " + n.okClass).addClass("ko " + n.koClass)
                        }(new RegExp(o.regex, "g"), i, o.name, a)
                    })
                }
                var n = e.extend(!0, i, t);
                return this.each(function() {
                    e.isFunction(n.onLoad) && n.onLoad(), oRulesBuilder = '<span class="rules">' + n.msgRules + "</span>", oRulesBuilder += '<ul class="rules">', e.each(n.rules, function(e, t) {
                        t.enable && (oRulesBuilder += '<li class="ko ' + n.koClass + " " + t.name + '">' + t.message + "</li>")
                    }), oRulesBuilder += "</ul>", void 0 === n.container ? (e(this).after('<div class="rules-list ' + n.containerClass + '" id="' + n.containerId + '"></div>'), e(oRulesBuilder).appendTo("#" + n.containerId)) : (n.container.addClass("rules-list"), e(oRulesBuilder).appendTo(n.container));
                    var t = void 0 === n.container ? n.containerId : n.container.attr("id");
                    a(0, e(this).val(), t), e(this).keyup(function() {
                        a(0, e(this).val(), t)
                    }), e(this).on("paste", function() {
                        a(0, e(this).val(), t)
                    }), e(this).change(function() {
                        a(0, e(this).val(), t)
                    })
                })
            }
        }(e)
    }).call(this, i(1))
}, , , , , , , function(e, t, i) {
    var a = {
        "./br": 59,
        "./br.js": 59,
        "./en-au": 60,
        "./en-au.js": 60,
        "./en-ca": 61,
        "./en-ca.js": 61,
        "./en-gb": 62,
        "./en-gb.js": 62,
        "./en-ie": 63,
        "./en-ie.js": 63,
        "./en-il": 64,
        "./en-il.js": 64,
        "./en-nz": 65,
        "./en-nz.js": 65,
        "./pt-br": 66,
        "./pt-br.js": 66
    };

    function n(e) {
        var t = o(e);
        return i(t)
    }

    function o(e) {
        var t = a[e];
        if (!(t + 1)) {
            var i = new Error("Cannot find module '" + e + "'");
            throw i.code = "MODULE_NOT_FOUND", i
        }
        return t
    }
    n.keys = function() {
        return Object.keys(a)
    }, n.resolve = o, e.exports = n, n.id = 145
}, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(0),
            a = (i(35), e.isFunction(e.fn.valid) ? 1 : 0);
        e.fn.isValid = function() {
            return !a || this.valid()
        }, e.fn.getActiveStep = function() {
            var t = this.find(".step.active");
            return e(this.children(".step:visible")).index(e(t)) + 1
        }, e.fn.activateStep = function(t) {
            if (!e(this).hasClass("step")) {
                var i = e(this).closest("ul.stepper");
                i.find(">li").removeAttr("data-last"), window.innerWidth < 993 || !i.hasClass("horizontal") ? e(this).addClass("step").stop().slideDown(400, function() {
                    e(this).css({
                        height: "auto",
                        "margin-bottom": "",
                        display: "inherit"
                    }), t && t(), i.find(">li.step").last().attr("data-last", "true")
                }) : (e(this).addClass("step").stop().css({
                    width: "0%",
                    display: "inherit"
                }).css({
                    width: "100%"
                }), e(this).css({
                    height: "auto",
                    "margin-bottom": "",
                    display: "inherit"
                }), t && t(), i.find(">li.step").last().attr("data-last", "true"))
            }
        }, e.fn.deactivateStep = function(t) {
            if (e(this).hasClass("step")) {
                var i = e(this).closest("ul.stepper");
                i.find(">li").removeAttr("data-last"), window.innerWidth < 993 || !i.hasClass("horizontal") ? e(this).stop().css({
                    transition: "none",
                    "-webkit-transition": "margin-bottom none"
                }).slideUp(400, function() {
                    e(this).removeClass("step").css({
                        height: "auto",
                        "margin-bottom": "",
                        transition: "margin-bottom .4s",
                        "-webkit-transition": "margin-bottom .4s"
                    }), t && t(), i.find(">li").removeAttr("data-last"), i.find(">li.step").last().attr("data-last", "true")
                }) : (e(this).stop().css({
                    width: "0%"
                }), e(this).removeClass("step").hide().css({
                    height: "auto",
                    "margin-bottom": "",
                    display: "none",
                    width: ""
                }), t && t(), i.find(">li.step").last().attr("data-last", "true"))
            }
        }, e.fn.showError = function(e) {
            if (a) {
                var t = this.attr("name"),
                    i = this.closest("form"),
                    n = {};
                n[t] = e, i.validate().showErrors(n), this.closest("li").addClass("wrong")
            } else this.removeClass("valid").addClass("invalid"), this.next().attr("data-error", e)
        }, e.fn.activateFeedback = function() {
            this.find(".step.active:not(.feedbacking)").addClass("feedbacking").find(".step-content").prepend('<div class="wait-feedback"> <div class="preloader-wrapper active"> <div class="spinner-layer spinner-blue"> <div class="circle-clipper left"> <div class="circle"></div></div><div class="gap-patch"> <div class="circle"></div></div><div class="circle-clipper right"> <div class="circle"></div></div></div><div class="spinner-layer spinner-red"> <div class="circle-clipper left"> <div class="circle"></div></div><div class="gap-patch"> <div class="circle"></div></div><div class="circle-clipper right"> <div class="circle"></div></div></div><div class="spinner-layer spinner-yellow"> <div class="circle-clipper left"> <div class="circle"></div></div><div class="gap-patch"> <div class="circle"></div></div><div class="circle-clipper right"> <div class="circle"></div></div></div><div class="spinner-layer spinner-green"> <div class="circle-clipper left"> <div class="circle"></div></div><div class="gap-patch"> <div class="circle"></div></div><div class="circle-clipper right"> <div class="circle"></div></div></div></div></div>')
        }, e.fn.destroyFeedback = function() {
            var e = this.find(".step.active.feedbacking");
            return e && (e.removeClass("feedbacking"), e.find(".wait-feedback").remove()), !0
        }, e.fn.resetStepper = function(i) {
            i || (i = 1);
            var a = e(this).closest("form");
            return e(a)[0].reset(), Object(t.r)(), e(this).openStep(i)
        }, e.fn.submitStepper = function() {
            var e = this.closest("form");
            e.isValid() && e.submit()
        }, e.fn.nextStep = function(t, i, a) {
            var n = e(this).data("settings"),
                o = this.closest("form"),
                s = this.find(".step.active"),
                r = e(this.children(".step:visible")).index(e(s)) + 2,
                c = s.find(".next-step").length > 1 ? a ? e(a.target).data("feedback") : void 0 : s.find(".next-step").data("feedback");
            return o.isValid() ? c && i ? (n.showFeedbackLoader && this.activateFeedback(), window[c].call()) : (s.removeClass("wrong").addClass("done"), this.openStep(r, t), this.trigger("nextstep")) : s.removeClass("done").addClass("wrong")
        }, e.fn.prevStep = function(t) {
            var i = this.find(".step.active");
            if (!i.hasClass("feedbacking")) {
                var a = e(this.children(".step:visible")).index(e(i));
                return i.removeClass("wrong"), this.openStep(a, t), this.trigger("prevstep")
            }
        }, e.fn.openStep = function(t, i) {
            var a = e(this).closest("ul.stepper").data("settings"),
                n = this,
                o = t - 1,
                s = this.closest("form");
            if (!(t = this.find(".step:visible:eq(" + o + ")")).hasClass("active")) {
                var r = this.find(".step.active"),
                    c = e(this.children(".step:visible")).index(e(r)),
                    l = o > c ? 1 : 0;
                if (r.hasClass("feedbacking") && n.destroyFeedback(), o > c) {
                    if (!s.isValid()) return r.removeClass("done").addClass("wrong");
                    r.removeClass("wrong").addClass("done")
                }
                r.closeAction(l), t.openAction(l, function() {
                    a.autoFocusInput && t.find("input:enabled:visible:first").focus(), n.trigger("stepchange").trigger("step" + (o + 1)), t.data("event") && n.trigger(t.data("event")), i && i()
                })
            }
        }, e.fn.closeAction = function(e, t) {
            var i = this.removeClass("active").find(".step-content");
            window.innerWidth < 993 || !this.closest("ul").hasClass("horizontal") ? i.stop().slideUp(300, "easeOutQuad", t) : 1 == e ? (i.css({
                left: "-100%"
            }).css({
                display: "none",
                left: "0%"
            }), t && t()) : (i.css({
                left: "100%"
            }).css({
                display: "none",
                left: "0%"
            }), t && t())
        }, e.fn.openAction = function(e, t) {
            var i = this.removeClass("done").addClass("active").find(".step-content");
            window.innerWidth < 993 || !this.closest("ul").hasClass("horizontal") ? i.slideDown(300, "easeOutQuad", t) : 1 == e ? (i.css({
                left: "100%",
                display: "block"
            }).css({
                left: "0%"
            }), t && t()) : (i.css({
                left: "-100%",
                display: "block"
            }).css({
                left: "0%"
            }), t && t())
        }, e.fn.activateStepper = function(t) {
            var i = e.extend({
                linearStepsNavigation: !0,
                autoFocusInput: !0,
                showFeedbackLoader: !0,
                autoFormCreation: !0
            }, t);
            e(document).on("click", function(t) {
                e(t.target).parents(".stepper").length || e(".stepper.focused").removeClass("focused")
            }), e(this).each(function() {
                var t = e(this);
                if (!t.parents("form").length && i.autoFormCreation) {
                    var a = t.data("method"),
                        n = t.data("action");
                    n = n || "?", t.wrap('<form action="' + n + '" method="' + a + '"></form>')
                }
                t.data("settings", {
                    linearStepsNavigation: i.linearStepsNavigation,
                    autoFocusInput: i.autoFocusInput,
                    showFeedbackLoader: i.showFeedbackLoader
                }), t.find("li.step.active").openAction(1), t.find(">li").removeAttr("data-last"), t.find(">li.step").last().attr("data-last", "true"), t.on("click", ".step:not(.active)", function() {
                    var a = e(t.children(".step:visible")).index(e(this));
                    if (t.hasClass("linear")) {
                        if (i.linearStepsNavigation) {
                            var n = t.find(".step.active");
                            e(t.children(".step:visible")).index(e(n)) + 1 == a ? t.nextStep(void 0, !0, void 0) : e(t.children(".step:visible")).index(e(n)) - 1 == a && t.prevStep(void 0)
                        }
                    } else t.openStep(a + 1)
                }).on("click", ".next-step", function(e) {
                    e.preventDefault(), t.nextStep(void 0, !0, e)
                }).on("click", ".previous-step", function(e) {
                    e.preventDefault(), t.prevStep(void 0)
                }).on("click", function() {
                    e(".stepper.focused").removeClass("focused"), e(this).addClass("focused")
                })
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    (function(e) {
        var t = function(e) {
                return e.replace(/(:|\.|\[|\]|,|=)/g, "\\$1")
            },
            i = {
                init: function(i) {
                    var a = {
                        onShow: null,
                        swipeable: !1,
                        responsiveThreshold: 1 / 0
                    };
                    i = e.extend(a, i);
                    var n = function(e) {
                        return ((e.prop("tagName") || "") + (e.attr("id") || "") + (e.attr("class") || "")).replace(/\s/g, "")
                    }(e(this));
                    return this.each(function(a) {
                        var o, s, r, c, l = n + a,
                            d = e(this),
                            u = e(window).width(),
                            p = d.find("li.tab a"),
                            f = d.width(),
                            h = e(),
                            m = Math.max(f, d[0].scrollWidth) / p.length,
                            b = 0,
                            _ = 0,
                            v = !1,
                            g = function(e) {
                                return Math.ceil(f - e.position().left - e[0].getBoundingClientRect().width - d.scrollLeft())
                            },
                            y = function(e) {
                                return Math.floor(e.position().left + d.scrollLeft())
                            },
                            k = function(e) {
                                b - e >= 0 ? (c.velocity({
                                    right: g(o)
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), c.velocity({
                                    left: y(o)
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    delay: 90
                                })) : (c.velocity({
                                    left: y(o)
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), c.velocity({
                                    right: g(o)
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    delay: 90
                                }))
                            };
                        i.swipeable && u > i.responsiveThreshold && (i.swipeable = !1), 0 === (o = e(p.filter('[href="' + location.hash + '"]'))).length && (o = e(this).find("li.tab a.active").first()), 0 === o.length && (o = e(this).find("li.tab a").first()), o.addClass("active"), (b = p.index(o)) < 0 && (b = 0), void 0 !== o[0] && (s = e(o[0].hash)).addClass("active"), d.find(".indicator").length || d.append('<li class="indicator"></li>'), c = d.find(".indicator"), d.append(c), d.is(":visible") && setTimeout(function() {
                            c.css({
                                right: g(o)
                            }), c.css({
                                left: y(o)
                            })
                        }, 0), e(window).off("resize.tabs-" + l).on("resize.tabs-" + l, function() {
                            f = d.width(), m = Math.max(f, d[0].scrollWidth) / p.length, b < 0 && (b = 0), 0 !== m && 0 !== f && (c.css({
                                right: g(o)
                            }), c.css({
                                left: y(o)
                            }))
                        }), i.swipeable ? (p.each(function() {
                            var i = e(t(this.hash));
                            i.addClass("carousel-item"), h = h.add(i)
                        }), r = h.wrapAll('<div class="tabs-content carousel"></div>'), h.css("display", ""), e(".tabs-content.carousel").carousel({
                            fullWidth: !0,
                            noWrap: !0,
                            onCycleTo: function(e) {
                                if (!v) {
                                    var t = b;
                                    b = r.index(e), o.removeClass("active"), (o = p.eq(b)).addClass("active"), k(t), "function" == typeof i.onShow && i.onShow.call(d[0], s)
                                }
                            }
                        })) : p.not(o).each(function() {
                            e(t(this.hash)).hide()
                        }), d.off("click.tabs").on("click.tabs", "a", function(a) {
                            if (e(this).parent().hasClass("disabled")) a.preventDefault();
                            else if (!e(this).attr("target")) {
                                v = !0, f = d.width(), m = Math.max(f, d[0].scrollWidth) / p.length, o.removeClass("active");
                                var n = s;
                                o = e(this), s = e(t(this.hash)), p = d.find("li.tab a"), o.addClass("active"), _ = b, (b = p.index(e(this))) < 0 && (b = 0), i.swipeable ? h.length && h.carousel("set", b, function() {
                                    "function" == typeof i.onShow && i.onShow.call(d[0], s)
                                }) : (void 0 !== s && (s.show(), s.addClass("active"), "function" == typeof i.onShow && i.onShow.call(this, s)), void 0 === n || n.is(s) || (n.hide(), n.removeClass("active"))), k(_), a.preventDefault()
                            }
                        })
                    })
                },
                select_tab: function(e) {
                    this.find('a[href="#' + e + '"]').trigger("click")
                }
            };
        e.fn.tabs = function(t) {
            return i[t] ? i[t].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof t && t ? void e.error("Method " + t + " does not exist on jQuery.tabs") : i.init.apply(this, arguments)
        }
    }).call(this, i(1))
}, , , , function(e, t, i) {
    "use strict";
    (function(e) {
        i(11), i(203);
        var t = i(0),
            a = "header, main, footer, .top-banner",
            n = {
                init: function(i) {
                    i = e.extend({
                        menuWidth: 256,
                        edge: "left",
                        closeOnClick: !0,
                        draggable: !0,
                        onOpen: null,
                        onClose: null
                    }, i), e(this).each(function() {
                        var n = e(this),
                            o = n.attr("data-activates"),
                            s = e("#" + o);
                        s.css("width", i.menuWidth);
                        var r = e('.drag-target[data-sidenav="' + o + '"]');
                        i.draggable ? (r.length && r.remove(), r = e('<div class="drag-target"></div>').attr("data-sidenav", o), e("body").append(r)) : r = e(), "left" === i.edge ? (s.css("transform", "translateX(-" + i.menuWidth + "px)"), r.css({
                            left: 0
                        })) : (s.addClass("right-aligned").css("transform", "translateX(" + i.menuWidth + "px)"), r.css({
                            right: 0
                        }));
                        var c = Object(t.d)("lastMenuState");
                        s.hasClass("fixed") && (window.innerWidth > 992 && (null == c || c) ? (c = !0, s.css("transform", "translateX(0)")) : e(a).css("padding-" + i.edge, "0px"), e(window).resize(function() {
                            l()
                        })), !0 === i.closeOnClick && s.on("click.itemclick", "a:not(.collapsible-header)", function() {
                            window.innerWidth > 992 && s.hasClass("fixed") || d()
                        });
                        var l = function() {
                                s.hasClass("fixed") && (window.innerWidth > 992 ? c && (0 !== e("#sidenav-overlay").length ? d(!0) : (e(a).css("padding-" + i.edge, i.menuWidth + "px"), s.css("transform", "translateX(0)"))) : (c = !1, "left" === i.edge ? s.css("transform", "translateX(-" + i.menuWidth + "px)") : s.css("transform", "translateX(" + i.menuWidth + "px)"), e(a).css("padding-" + i.edge, "0px"))), Object(t.n)("lastMenuState", c, 7)
                            },
                            d = function(a) {
                                c = !1, e("body").css({
                                    overflow: "",
                                    width: ""
                                }), e("#sidenav-overlay").velocity({
                                    opacity: 0
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    complete: function() {
                                        e(this).remove()
                                    }
                                }), "left" === i.edge ? (r.css({
                                    width: "",
                                    right: "",
                                    left: "0"
                                }), s.velocity({
                                    translateX: "-" + i.menuWidth + "px"
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutCubic",
                                    complete: function() {
                                        !0 === a && (s.removeAttr("style"), s.css("width", i.menuWidth), c = !0)
                                    }
                                })) : (r.css({
                                    width: "",
                                    right: "0",
                                    left: ""
                                }), s.velocity({
                                    translateX: i.menuWidth + "px"
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutCubic",
                                    complete: function() {
                                        !0 === a && (s.removeAttr("style"), s.css("width", i.menuWidth))
                                    }
                                })), "function" == typeof i.onClose && i.onClose.call(this, s), Object(t.n)("lastMenuState", c, 7)
                            };
                        i.draggable && (r.on("click", function() {
                            c && (d(), e(a).css("padding-" + i.edge, "0px"))
                        }), r.hammer({
                            prevent_default: !1
                        }).on("pan", function(t) {
                            if ("touch" == t.gesture.pointerType) {
                                var n = t.gesture.center.x,
                                    o = t.gesture.center.y;
                                if (0 === n && 0 === o) return;
                                var r, l = e("body"),
                                    u = e("#sidenav-overlay"),
                                    p = l.innerWidth();
                                if (l.css("overflow", "hidden"), l.width(p), 0 === u.length && window.innerWidth <= 992 && ((u = e('<div id="sidenav-overlay"></div>')).css("opacity", 0).click(function() {
                                        d()
                                    }), "function" == typeof i.onOpen && i.onOpen.call(this, s), e("body").append(u)), "left" === i.edge && (n > i.menuWidth ? n = i.menuWidth : n < 0 && (n = 0)), "left" === i.edge) n < i.menuWidth / 2 ? c = !1 : n >= i.menuWidth / 2 && (c = !0), s.css("transform", "translateX(" + (n - i.menuWidth) + "px)");
                                else {
                                    n < window.innerWidth - i.menuWidth / 2 ? c = !0 : n >= window.innerWidth - i.menuWidth / 2 && (c = !1);
                                    var f = n - i.menuWidth / 2;
                                    f < 0 && (f = 0), s.css("transform", "translateX(" + f + "px)")
                                }
                                if (0 !== u.length) "left" === i.edge ? (r = n / i.menuWidth, u.velocity({
                                    opacity: r
                                }, {
                                    duration: 10,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                })) : (r = Math.abs((n - window.innerWidth) / i.menuWidth), u.velocity({
                                    opacity: r
                                }, {
                                    duration: 10,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }));
                                else e(a).css("padding-" + i.edge, n + "px")
                            }
                        }).on("panend", function(t) {
                            if ("touch" == t.gesture.pointerType) {
                                var a = e("#sidenav-overlay"),
                                    n = t.gesture.velocityX,
                                    o = t.gesture.center.x,
                                    d = o - i.menuWidth,
                                    u = o - i.menuWidth / 2;
                                d > 0 && (d = 0), u < 0 && (u = 0), "left" === i.edge ? c && n <= .3 || n < -.5 ? (0 !== d && s.velocity({
                                    translateX: [0, d]
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), a.velocity({
                                    opacity: 1
                                }, {
                                    duration: 50,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), r.css({
                                    width: window.innerWidth - i.menuWidth + 10 + "px",
                                    right: 0,
                                    left: ""
                                }), c = !0) : (!c || n > .3) && (e("body").css({
                                    overflow: "",
                                    width: ""
                                }), s.velocity({
                                    translateX: [-1 * i.menuWidth - 10, d]
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), a.velocity({
                                    opacity: 0
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    complete: function() {
                                        "function" == typeof i.onClose && i.onClose.call(this, s), e(this).remove()
                                    }
                                }), r.css({
                                    width: "10px",
                                    right: "",
                                    left: 0
                                })) : c && n >= -.3 || n > .5 ? (0 !== u && s.velocity({
                                    translateX: [0, u]
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), a.velocity({
                                    opacity: 1
                                }, {
                                    duration: 50,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), r.css({
                                    width: window.innerWidth - i.menuWidth + 10 + "px",
                                    right: "",
                                    left: 0
                                }), c = !0) : (!c || n < -.3) && (e("body").css({
                                    overflow: "",
                                    width: ""
                                }), s.velocity({
                                    translateX: [i.menuWidth + 10, u]
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                }), a.velocity({
                                    opacity: 0
                                }, {
                                    duration: 200,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    complete: function() {
                                        "function" == typeof i.onClose && i.onClose.call(this, s), e(this).remove()
                                    }
                                }), r.css({
                                    width: "10px",
                                    right: 0,
                                    left: ""
                                })), l()
                            }
                        })), n.off("click.sidenav").on("click.sidenav", function() {
                            if (c) window.innerWidth > 992 && e(a).css("padding-" + i.edge, "0px"), c = !1, d();
                            else if (window.innerWidth > 992) "left" === i.edge ? s.velocity({
                                translateX: [0, -1 * i.menuWidth]
                            }, {
                                duration: 300,
                                queue: !1,
                                easing: "easeOutQuad"
                            }) : s.velocity({
                                translateX: [0, i.menuWidth]
                            }, {
                                duration: 300,
                                queue: !1,
                                easing: "easeOutQuad"
                            }), c = !0, e(a).css("padding-" + i.edge, i.menuWidth);
                            else {
                                var n = e("body"),
                                    o = e('<div id="sidenav-overlay"></div>'),
                                    l = n.innerWidth();
                                n.css("overflow", "hidden"), n.width(l), e("body").append(r), "left" === i.edge ? (r.css({
                                    width: window.innerWidth - i.menuWidth + 10 + "px",
                                    right: 0,
                                    left: ""
                                }), s.velocity({
                                    translateX: [0, -1 * i.menuWidth]
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                })) : (r.css({
                                    width: window.innerWidth - i.menuWidth + 10 + "px",
                                    right: "",
                                    left: 0
                                }), s.velocity({
                                    translateX: [0, i.menuWidth]
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad"
                                })), o.css("opacity", 0).click(function() {
                                    c = !1, d(), o.velocity({
                                        opacity: 0
                                    }, {
                                        duration: 300,
                                        queue: !1,
                                        easing: "easeOutQuad",
                                        complete: function() {
                                            e(this).remove()
                                        }
                                    })
                                }), e("body").append(o), o.velocity({
                                    opacity: 1
                                }, {
                                    duration: 300,
                                    queue: !1,
                                    easing: "easeOutQuad",
                                    complete: function() {
                                        c = !0
                                    }
                                }), "function" == typeof i.onOpen && i.onOpen.call(this, s)
                            }
                            return Object(t.n)("lastMenuState", c, 7), !1
                        }), Object(t.n)("lastMenuState", c, 7)
                    })
                },
                destroy: function() {
                    var t = e("#sidenav-overlay"),
                        i = e('.drag-target[data-sidenav="' + e(this).attr("data-activates") + '"]');
                    t.trigger("click"), i.remove(), e(this).off("click"), t.remove()
                },
                show: function() {
                    this.trigger("click")
                },
                hide: function() {
                    e("#sidenav-overlay").trigger("click")
                }
            };
        e.fn.sideNav = function(t) {
            return n[t] ? n[t].apply(this, Array.prototype.slice.call(arguments, 1)) : "object" != typeof t && t ? void e.error("Method " + t + " does not exist on jQuery.sideNav") : n.init.apply(this, arguments)
        }
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(33),
            a = i.n(t);
        e.fn.hammer = function(t) {
            return this.each(function() {
                var i = e(this);
                i.data("hammer") || i.data("hammer", new a.a(i[0], t))
            })
        }, a.a.Manager.prototype.emit = function(t) {
            return function(i, a) {
                t.call(this, i, a), e(this.element).trigger({
                    type: i,
                    gesture: a
                })
            }
        }(a.a.Manager.prototype.emit)
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(32);
        e.fn.scrollTo = function(t) {
            return e(this).scrollTop(e(this).scrollTop() - e(this).offset().top + e(t).offset().top), this
        }, e.fn.dropdown = function(i) {
            var a = {
                inDuration: 300,
                outDuration: 225,
                constrainWidth: !1,
                hover: !1,
                gutter: 0,
                belowOrigin: !0,
                alignment: "left",
                stopPropagation: !1,
                closeOnClick: !0
            };
            return "open" === i ? (this.each(function() {
                e(this).trigger("open")
            }), !1) : "close" === i ? (this.each(function() {
                e(this).trigger("close")
            }), !1) : void this.each(function() {
                var n = e(this),
                    o = e.extend({}, a, i),
                    s = !1,
                    r = e("#" + n.attr("data-activates"));

                function c() {
                    void 0 !== n.data("induration") && (o.inDuration = n.data("induration")), void 0 !== n.data("outduration") && (o.outDuration = n.data("outduration")), void 0 !== n.data("constrainwidth") && (o.constrainWidth = n.data("constrainwidth")), void 0 !== n.data("hover") && (o.hover = n.data("hover")), void 0 !== n.data("gutter") && (o.gutter = n.data("gutter")), void 0 !== n.data("beloworigin") && (o.belowOrigin = n.data("beloworigin")), void 0 !== n.data("alignment") && (o.alignment = n.data("alignment")), void 0 !== n.data("stoppropagation") && (o.stopPropagation = n.data("stoppropagation")), void 0 !== n.data("closeonclick") && (o.closeOnClick = n.data("closeonclick"))
                }

                function l(i) {
                    "focus" === i && (s = !0), c(), r.addClass("active"), n.addClass("active");
                    var a = n[0].getBoundingClientRect().width;
                    !0 === o.constrainWidth ? r.css("width", a) : r.css("white-space", "nowrap");
                    var l = window.innerHeight,
                        u = n.innerHeight(),
                        p = n.offset().left,
                        f = n.offset().top - e(window).scrollTop(),
                        h = o.alignment,
                        m = 0,
                        b = 0,
                        _ = 0;
                    !0 === o.belowOrigin && (_ = u);
                    var v = 0,
                        g = 0,
                        y = n.parent();
                    if (y.is("body") || (y[0].scrollHeight > y[0].clientHeight && (v = y[0].scrollTop), y[0].scrollWidth > y[0].clientWidth && (g = y[0].scrollLeft)), p + r.innerWidth() > e(window).width() ? h = "right" : p - r.innerWidth() + n.innerWidth() < 0 && (h = "left"), f + r.innerHeight() > l)
                        if (f + u - r.innerHeight() < 0) {
                            var k = l - f - _;
                            r.css("max-height", k)
                        } else _ || (_ += u), _ -= r.innerHeight();
                    if ("left" === h) m = o.gutter, b = n.position().left + m;
                    else if ("right" === h) {
                        r.stop(!0, !0).css({
                            opacity: 0,
                            left: 0
                        }), b = n.position().left + a - r.width() + (m = -o.gutter)
                    }
                    r.css({
                        position: "absolute",
                        top: n.position().top + _ + v,
                        left: b + g
                    }), 0 === e("#" + r.attr("id") + ".ps").length && new t.a("#" + r.attr("id")), r.slideDown({
                        queue: !1,
                        duration: o.inDuration,
                        easing: "easeOutCubic",
                        complete: function() {
                            e(this).css("height", "")
                        }
                    }).animate({
                        opacity: 1
                    }, {
                        queue: !1,
                        duration: o.inDuration,
                        easing: "easeOutSine"
                    }), o.closeOnClick && setTimeout(function() {
                        e(document).on("click." + r.attr("id"), function() {
                            d(), e(document).off("click." + r.attr("id"))
                        })
                    }, 0)
                }

                function d() {
                    s = !1, r.fadeOut(o.outDuration), r.removeClass("active"), n.removeClass("active"), e(document).off("click." + r.attr("id")), setTimeout(function() {
                        r.css("max-height", "")
                    }, o.outDuration)
                }
                if (c(), n.after(r), o.hover) {
                    var u = !1;
                    n.off("click." + n.attr("id")), n.on("mouseenter", function() {
                        !1 === u && (l(), u = !0)
                    }), n.on("mouseleave", function(t) {
                        var i = t.toElement || t.relatedTarget;
                        e(i).closest(".dropdown-content").is(r) || (r.stop(!0, !0), d(), u = !1)
                    }), r.on("mouseleave", function(t) {
                        var i = t.toElement || t.relatedTarget;
                        e(i).closest(".dropdown-button").is(n) || (r.stop(!0, !0), d(), u = !1)
                    })
                } else n.off("click." + n.attr("id")), n.on("click." + n.attr("id"), function(t) {
                    s || (n[0] != t.currentTarget || n.hasClass("active") || 0 !== e(t.target).closest(".dropdown-content").length ? n.hasClass("active") && (d(), e(document).off("click." + r.attr("id"))) : (t.preventDefault(), o.stopPropagation && t.stopPropagation(), l("click")))
                });
                n.on("open", function(e, t) {
                    l(t)
                }), n.on("close", d)
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    (function(e) {
        e.fn.collapsible = function(t, i) {
            var a = {
                    accordion: void 0,
                    onOpen: void 0,
                    onClose: void 0
                },
                n = t;
            return t = e.extend(a, t), this.each(function(a, o) {
                var s = e(o),
                    r = e(o).find("> li > .collapsible-header"),
                    c = s.data("collapsible");

                function l(i, a) {
                    a || i.toggleClass("active"), t.accordion || "accordion" === c || void 0 === c ? function(t) {
                        r = s.find("> li > .collapsible-header"), t.hasClass("active") ? t.parent().addClass("active") : t.parent().removeClass("active"), t.parent().hasClass("active") ? t.siblings(".collapsible-body").stop(!0, !1).slideDown({
                            duration: 350,
                            easing: "easeOutQuart",
                            queue: !1,
                            complete: function() {
                                e(this).css("height", "")
                            }
                        }) : t.siblings(".collapsible-body").stop(!0, !1).slideUp({
                            duration: 350,
                            easing: "easeOutQuart",
                            queue: !1,
                            complete: function() {
                                e(this).css("height", "")
                            }
                        }), r.not(t).removeClass("active").parent().removeClass("active"), r.not(t).parent().children(".collapsible-body").stop(!0, !1).each(function(t, i) {
                            e(i).is(":visible") && e(i).slideUp({
                                duration: 350,
                                easing: "easeOutQuart",
                                queue: !1,
                                complete: function() {
                                    e(this).css("height", ""), d(e(this).siblings(".collapsible-header"))
                                }
                            })
                        })
                    }(i) : function(t) {
                        t.hasClass("active") ? t.parent().addClass("active") : t.parent().removeClass("active"), t.parent().hasClass("active") ? t.siblings(".collapsible-body").stop(!0, !1).slideDown({
                            duration: 350,
                            easing: "easeOutQuart",
                            queue: !1,
                            complete: function() {
                                e(this).css("height", "")
                            }
                        }) : t.siblings(".collapsible-body").stop(!0, !1).slideUp({
                            duration: 350,
                            easing: "easeOutQuart",
                            queue: !1,
                            complete: function() {
                                e(this).css("height", "")
                            }
                        })
                    }(i), d(i)
                }

                function d(e) {
                    e.hasClass("active") ? "function" == typeof t.onOpen && t.onOpen.call(this, e.parent()) : "function" == typeof t.onClose && t.onClose.call(this, e.parent())
                }

                function u(e) {
                    return e.closest("li > .collapsible-header")
                }

                function p() {
                    s.off("click.collapse", "> li > .collapsible-header")
                }
                if ("destroy" !== n)
                    if (i >= 0 && i < r.length) {
                        var f = r.eq(i);
                        f.length && ("open" === n || "close" === n && f.hasClass("active")) && l(f)
                    } else p(), s.on("click.collapse", "> li > .collapsible-header", function(t) {
                        var i = e(t.target);
                        (function(e) {
                            return u(e).length > 0
                        })(i) && (i = u(i)), l(i)
                    }), t.accordion || "accordion" === c || void 0 === c ? l(r.filter(".active").first(), !0) : r.filter(".active").each(function(t, i) {
                        l(e(i), !0)
                    });
                else p()
            })
        }, e(document).ready(function() {
            e(".collapsible").collapsible()
        })
    }).call(this, i(1))
}, , , , , , , , , , , , , , , , , , , , , , function(e, t) {}, , , function(e, t, i) {
    "use strict";
    (function(e) {
        var t = i(2),
            a = function(t, i) {
                this.element = t, this.settings = e.extend({}, a.defaults, i), this.settings.fullPage = this.element.is("body"), this.init(), this.settings.start && this.start()
            };
        a.defaults = {
            overlay: void 0,
            zIndex: void 0,
            message: "<svg class='circular' viewBox='25 25 50 50'><circle class='path' cx='50' cy='50' r='20' fill='none' stroke-width='2' stroke-miterlimit='10'></circle></svg>",
            theme: "light",
            shownClass: "loading-shown",
            hiddenClass: "loading-hidden",
            stoppable: !1,
            start: !0,
            onStart: function(e) {
                e.overlay.fadeIn(150)
            },
            onStop: function(e) {
                e.overlay.fadeOut(150)
            },
            onClick: function() {}
        }, a.setDefaults = function(t) {
            a.defaults = e.extend({}, a.defaults, t)
        }, e.extend(a.prototype, {
            init: function() {
                this.isActive = !1, this.overlay = this.settings.overlay || this.createOverlay(), this.resize(), this.attachMethodsToExternalEvents(), this.attachOptionsHandlers()
            },
            createOverlay: function() {
                var t, i = this.element.attr("id");
                return t = i && e("[id=" + i + "_loading-overlay]").length > 0 ? e("[id=" + i + "_loading-overlay]") : e("<div class='loading-overlay loading-theme-" + this.settings.theme + "'><div class='loading-overlay-content'>" + this.settings.message + "</div></div>").addClass(this.settings.hiddenClass).hide().appendTo("body"), i && t.attr("id", i + "_loading-overlay"), t
            },
            attachMethodsToExternalEvents: function() {
                var t = this;
                t.element.on("loading.start", function() {
                    t.overlay.removeClass(t.settings.hiddenClass).addClass(t.settings.shownClass)
                }), t.element.on("loading.stop", function() {
                    t.overlay.removeClass(t.settings.shownClass).addClass(t.settings.hiddenClass)
                }), t.settings.stoppable && t.overlay.on("click", function() {
                    t.stop()
                }), t.overlay.on("click", function() {
                    t.element.trigger("loading.click", t)
                }), e(window).on("resize", function() {
                    t.resize()
                }), e(function() {
                    t.resize()
                })
            },
            attachOptionsHandlers: function() {
                var e = this;
                e.element.on("loading.start", function(t, i) {
                    e.settings.onStart(i)
                }), e.element.on("loading.stop", function(t, i) {
                    e.settings.onStop(i)
                }), e.element.on("loading.click", function(t, i) {
                    e.settings.onClick(i)
                })
            },
            calcZIndex: function() {
                return void 0 !== this.settings.zIndex ? this.settings.zIndex : (parseInt(this.element.css("z-index")) || 0) + 1 + this.settings.fullPage
            },
            resize: function() {
                var t, i, a, n, o;
                e(this.element).parent().hasClass("input-field") ? (i = (t = e(this.element).parent()).offset().top - 16, e(this.overlay).find(".circular").addClass("right")) : e(this.element).parent().parent().hasClass("input-field") ? (i = (t = e(this.element).parent().parent()).offset().top - 16, e(this.overlay).find(".circular").addClass("right")) : i = (t = this.element).offset().top, a = t.offset().left, this.settings.fullPage ? (o = "100%", n = "100%") : (n = t.outerWidth() + 1, o = t.outerHeight()), this.overlay.css({
                    position: this.settings.fullPage ? "fixed" : "absolute",
                    zIndex: this.calcZIndex(),
                    top: i,
                    left: a,
                    width: n,
                    height: o
                })
            },
            start: function() {
                this.element.is("body") ? Object(t.a)() : (e(this.element).is(":visible") || e(this.element).is("select")) && (this.isActive = !0, this.resize(), this.element.trigger("loading.start", this))
            },
            stop: function() {
                this.element.is("body") ? Object(t.b)() : (this.isActive = !1, this.element.trigger("loading.stop", this))
            },
            active: function() {
                return this.isActive
            },
            toggle: function() {
                this.active() ? this.stop() : this.start()
            },
            destroy: function() {
                this.overlay.remove()
            }
        });
        var n = "jquery-loading";
        e.fn.loading = function(t) {
            return this.each(function() {
                var i = e.data(this, n);
                i ? void 0 === t ? i.start() : "string" == typeof t ? i[t].apply(i) : (i.destroy(), e.data(this, n, new a(e(this), t))) : void 0 !== t && "object" != typeof t && "start" !== t && "toggle" !== t || e.data(this, n, new a(e(this), t))
            })
        }, e.fn.Loading = function(t) {
            var i = e(this).data(n);
            return i && void 0 === t || e(this).data(n, i = new a(e(this), t)), i
        }, e.expr[":"].loading = function(t) {
            var i = e.data(t, n);
            return !!i && i.active()
        }, e.Loading = a, e(document).ready(function() {
            setTimeout(function() {
                e(":loading").each(function(t, i) {
                    e(i).loading("resize")
                })
            }, 200)
        })
    }).call(this, i(1))
}, function(e, t, i) {
    (function(e) {
        e.fn.characterCounter = function() {
            this.each(function() {
                var t = e(this),
                    i = t.parent().find('span[class="character-counter"]');
                if (!i.length && void 0 !== t.attr("data-length")) {
                    if (t.on("input focus", function() {
                            var t = +e(this).attr("data-length"),
                                i = +e(this).val().length,
                                a = i <= t;
                            e(this).parent().find('span[class="character-counter"]').html(i + "/" + t);
                            var n = e(this).hasClass("invalid");
                            a && n ? e(this).removeClass("invalid") : a || n || (e(this).removeClass("valid"), e(this).addClass("invalid"))
                        }), t.on("blur", function() {
                            e(this).parent().find('span[class="character-counter"]').html("")
                        }), i.length) return;
                    i = e("<span/>").addClass("character-counter"), t.parent().append(i)
                }
            })
        }
    }).call(this, i(1))
}, function(e, t, i) {
    (function(e) {
        e(document).on("click.select-activator", ".select-item:not(.disabled) .select-activator, .select-item:not(.disabled).select-activator", function(t) {
            var i = e(this).closest(".select-list");
            i.hasClass("select-multi") ? e(this).closest(".select-item").toggleClass("active") : (i.find(".select-item.active").removeClass("active"), e(this).closest(".select-item").addClass("active"))
        })
    }).call(this, i(1))
}, function(e, t, i) {
    "use strict";
    i.r(t);
    i(114), i(230), i(231), i(232);
    var a = i(2),
        n = i(8),
        o = i(3),
        s = i(22),
        r = i(17),
        c = i(112),
        l = i(31),
        d = i(0),
        u = {
            _: i(14),
            refresh: d.m,
            loading: {
                start: a.a,
                stop: a.b
            },
            go: n.a,
            navigation: l.a,
            toast: o.a,
            modal: function(e, t) {
                "string" == typeof e ? Object(s.a)(r.a, e, t) : Object(r.a)(e, t)
            },
            confirm: c.a,
            fn: {
                updateActiveMenu: d.p,
                updateTextFields: d.r,
                updateHelpers: d.q,
                submitErrorHandler: d.o,
                maskFields: d.j
            },
            version: "0.1.9"
        };
    window.createElem = d.b, window.$$$ = u
}]);
//# sourceMappingURL=mpnui.js.map