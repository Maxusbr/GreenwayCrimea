/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.image_prefillDimensions = false;

    config.allowedContent = true;

    config.baseHref = CKEDITOR_BASEPATH.replace('vendors/ckeditor/', '');

    //#region filemanager
    config.filebrowserBrowseUrl = CKEDITOR_BASEPATH + 'plugins/fileman/index.html?rnd=0';
    config.filebrowserImageBrowseUrl = CKEDITOR_BASEPATH +  'plugins/fileman/index.html?type=image&rnd=0';
    config.removeDialogTabs = 'link:upload;image:upload';
    //#endregion

    CKEDITOR.dtd.$removeEmpty.span = 0;

    config.extraPlugins = 'codemirror,lineheight,autolink';
	
	config.font_names = 'Arial/Arial, Helvetica, sans-serif;' +
	//'Comic Sans MS/Comic Sans MS, cursive;' + // not supported in IOS
	'Courier New/Courier New, Courier, monospace;' +
	'Georgia/Georgia, serif;' +
	'Lucida Sans Unicode/Lucida Sans Unicode, Lucida Grande, sans-serif;' +
	'Tahoma/Tahoma, Geneva, sans-serif;' +
	'Times New Roman/Times New Roman, Times, serif;' +
	'Trebuchet MS/Trebuchet MS, Helvetica, sans-serif;' +
	'Verdana/Verdana, Geneva, sans-serif';

	config.templates_files = [CKEDITOR.getUrl('plugins/templates/templates/advantshop.js')];
	config.templates = 'advantshop';
    config.templates_replaceContent = false;
};
