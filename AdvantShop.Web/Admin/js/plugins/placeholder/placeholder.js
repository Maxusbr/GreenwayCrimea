(function ($) {
	var advantshop = Advantshop, 
	scriptsManager = advantshop.ScriptsManager, 
	isNative = !!('placeholder' in document.createElement('input')), 
	counter = 0;
	
	advantshop.NamespaceRequire('Advantshop.ScriptsManager');
	
	var placeholder = function(obj, options){
		this.$obj = advantshop.GetJQueryObject(obj);
		this.placeholderText = this.$obj.attr('placeholder');
		this.options = $.extend({}, this.defaultOptions, options);
		
		return this; 
	};
	
	scriptsManager.Placeholder = placeholder;
	
	placeholder.prototype.Init = function(obj, options){
		var placeholderObj = new placeholder(obj, options);
		
		placeholderObj.GenerateHtml();
		placeholderObj.BindEvent();
		
		return placeholderObj;
	};	
	
	
	$(function(){
		if(isNative != true){
			$('[placeholder]').each(function(){
				placeholder.prototype.Init(this, this.getAttribute('data-placeholder-options') || {});
			});	
		}
	});
	

	placeholder.prototype.GenerateHtml = function(){
		var placeholderObj = this
		, $obj = placeholderObj.$obj
		, pos = $obj.position()
		, $objClone = $obj.clone();
		
		$objClone.attr('id', 'placeholder-' + (counter+= 1));
		
		if($objClone.is(':password') === true){
			$objClone.attr('type', 'text');
		}
		
		$objClone.addClass(placeholderObj.options.cssClass);
		
		$objClone.css({
			top: pos.top,
			left: pos.left	
		});
		
		$objClone.val(placeholderObj.placeholderText);
		
		$obj.after($objClone);
		
		placeholderObj.$objClone = $objClone;
		
		placeholderObj.Show();
		
	};
	
	placeholder.prototype.BindEvent = function(){
		var placeholderObj = this
		, options = placeholderObj.options
		, $obj = placeholderObj.$obj
		, $objClone = placeholderObj.$objClone;
		
		$obj.on(options.eventShow, function(){
			placeholderObj.Show();
		});
		
		$objClone.on(options.eventHide, function(){
			placeholderObj.Hide();
		});
	};
	
	placeholder.prototype.Hide = function(){
		var placeholderObj = this
		placeholderObj.$objClone.hide();
		placeholderObj.$obj.show();
		placeholderObj.$obj.focus();
	};
	
	placeholder.prototype.Show = function(){
		var placeholderObj = this
		
		if(placeholderObj.$obj.val().length === 0){
			placeholderObj.$objClone.show();
			placeholderObj.$obj.hide();
		}else{
			placeholderObj.$objClone.hide();
		}
	};	
	
	placeholder.prototype.defaultOptions ={
		cssClass:'placeholder',
		eventShow: 'blur',
		eventHide: 'click focus'
	};
	
})(jQuery);