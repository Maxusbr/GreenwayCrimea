/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

( function() {
	'use strict';

	// Regex by Imme Emosol.
	var validUrlRegex = /^(https?|ftp):\/\/(-\.)?([^\s\/?\.#-]+\.?)+(\/[^\s]*)?[^\s\.,]$/ig,
		doubleQuoteRegex = /"/g;

	CKEDITOR.plugins.add("clicklink", {
	    init: function (e) {
	        e.on("instanceReady", function () {
	            $('iframe').contents().click(function (e) {
	                if (typeof e.target.href != 'undefined') {
	                    window.open(e.target.href, 'new' + e.screenX);
	                }
	            });

	        });

	    }
	});
} )();
