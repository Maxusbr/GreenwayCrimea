/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

( function() {
	'use strict';

	// Regex by Imme Emosol.
	var validUrlRegex = /^(https?|ftp):\/\/(-\.)?([^\s\/?\.#-]+\.?)+(\/[^\s]*)?[^\s\.,]$/ig,
		doubleQuoteRegex = /"/g;
	CKEDITOR.plugins.add("autolink", {
	    init: function (e) {
	        e.on("instanceReady", function () {
	            var w = document.querySelector('.btn-save');
	            var t = 0;
	            if (CKEDITOR.env.ie || CKEDITOR.env.gecko && CKEDITOR.env.version === 11e4) return;
	            var n = CKEDITOR.env.ie && CKEDITOR.env.version == "6" ? "﻿" : "​";
	            var r = function (e, t) {
	                return e.nodeType == 3 && !e.nodeValue.replace(new RegExp((t ? "^" : "") + n), "").length
	            };
	            var i = function (e) {
	                return e && e.nodeType == 1 && e.tagName.toLowerCase() == "body"
	            };
	            var s = function (e) {
	                return e ? e.replace(/&((g|l|quo)t|amp|#39);/g, function (e) {
	                    return {
	                        "&lt;": "<",
	                        "&": "&",
	                        "&quot;": '"',
	                        "&gt;": ">",
	                        "&#39;": "'"
	                    }[e]
	                }) : ""
	            };
	            var o = function (e) {
	                return Object.prototype.toString.apply(e) == "[object Array]"
	            };
	            var i = function (e) {
	                return e && e.nodeType == 1 && e.tagName.toLowerCase() == "body"
	            };
	            var u = function (e) {
	                if (!e) return {};
	                e = o(e) ? e : e.split(",");
	                for (var t = 0, n, r = {}; n = e[t++];) {
	                    r[n.toUpperCase()] = r[n] = 1
	                }
	                return r
	            };
	            var a = function (e, t, n) {
	                if (e && !i(e)) {
	                    e = n ? e : e.parentNode;
	                    while (e) {
	                        if (!t || t(e) || i(e)) {
	                            return t && !t(e) && i(e) ? null : e
	                        }
	                        e = e.parentNode
	                    }
	                }
	                return null
	            };
	            var f = function (e, t, n, r) {
	                t = u(o(t) ? t : [t]);
	                return a(e, function (e) {
	                    return t[e.tagName] && !(r && r(e))
	                }, n)
	            };
	            
	            e.on('blur', function () {
	                e.autolink();
	            }, null, null, -99999);
	            e.document.on("reset", function () {
	                t = 0
	            });
	            e.autolink = function (t) {
	                var o = e.getSelection().getNative(),
                        u = o.getRangeAt(0).cloneRange(),
                        a, l,
                        innerTextTemp;
	                var c = u.startContainer;
	                while (c.nodeType == 1 && u.startOffset > 0) {
	                    c = u.startContainer.childNodes[u.startOffset - 1];
	                    if (!c) break;
	                    u.setStart(c, c.nodeType == 1 ? c.childNodes.length : c.nodeValue.length);
	                    u.collapse(true);
	                    c = u.startContainer
	                }
	                do {
	                    if (u.startOffset == 0) {
	                        c = u.startContainer.previousSibling;
	                        while (c && c.nodeType == 1) {
	                            if (CKEDITOR.env.gecko && c.firstChild) c = c.firstChild;
	                            else c = c.lastChild
	                        }
	                        if (!c || r(c)) break;
	                        a = c.nodeValue.length
	                    } else {
	                        c = u.startContainer;
	                        a = u.startOffset
	                    }
	                    u.setStart(c, a - 1);
	                    l = u.toString().charCodeAt(0)
	                } while (l != 160 && l != 32);
	                if (u.toString().replace(new RegExp(n, "g"), "").match(/(?:https?:\/\/|ssh:\/\/|ftp:\/\/|file:\/|www\.)/i)) {
	                    while (u.toString().length) {
	                        if (/^(?:https?:\/\/|ssh:\/\/|ftp:\/\/|file:\/|www\.)/i.test(u.toString())) break;
	                        try {
	                            u.setStart(u.startContainer, u.startOffset + 1)
	                        } catch (t) {
	                            var c = u.startContainer;
	                            while (!(next = c.nextSibling)) {
	                                if (i(c)) return;
	                                c = c.parentNode
	                            }
	                            u.setStart(next, 0)
	                        }
	                    }
	                    if (f(u.startContainer, "a", true)) return;
	                    var h = document.createElement("a"),
                            p = document.createTextNode(" "),
                            d;
	                    e.undoManger && e.undoManger.save();
	                    h.appendChild(u.extractContents());
	                    h.href = h.innerHTML = h.innerHTML.replace(/<[^>]+>/g, "");
	                    d = h.getAttribute("href").replace(new RegExp(n, "g"), "");
	                    d = /^(?:https?:\/\/)/ig.test(d) ? d : "http://" + d;
	                    //h.setAttribute("_src", s(d));
	                    h.href = s(d);
	                    u.insertNode(h);
	                    //h.innerText = h.innerText.replace(/(?:((?:(?:\/\/)|^)|\s)www\.)(?=[^/\s$]*?\.)/gi, "$1");
	                    h.parentNode.insertBefore(p, h.nextSibling);
	                    u.setStart(p.nextSibling, 0);
	                    u.collapse(true);
	                    o.removeAllRanges();
	                    o.addRange(u);
	                    e.undoManger && e.undoManger.save()
	                }
	            };
	            if (CKEDITOR.env.webkit) {
	                e.on("key", function (t) {
	                    if (t.data.keyCode === 32 || t.data.keyCode === 13 ) e.autolink(t)
	                })
	            } else {
                    
	                e.document.on("keypress", function (t) {
	                    if (t.data.getKey() === 32 || t.data.getKey() === 13) e.autolink(t)
	                })
	            }
	        })
	    }
	});

} )();
