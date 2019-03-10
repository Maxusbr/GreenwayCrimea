(function (jQuery) {
    jQuery.fn.advMoveCaret = function (pos) { 		
		this.each(function(){
			if(pos)
			{
				switch(pos)
				{
					case "start":
						moveCaretToStart.call(this);
					break;
					case "end":
						moveCaretToEnd.call(this);
					break;
					case parseInt(pos):
						setCaretPosition.call(this, parseInt(pos))
					break;
					default:
						moveCaretToStart.call(this);	
				}
			}
		})
		
		function moveCaretToStart()
		{
			var inputObject = this;
			if($.browser.msie)
			{
				if(inputObject.createTextRange)
				{
				   var r = inputObject.createTextRange();
				   r.collapse(true);
				   r.select();
				}
			}else{
				 if(inputObject.selectionStart >= 0)
				 {
				  inputObject.setSelectionRange(0,0);
				  inputObject.focus();
				 }
			}
		}
		
		function moveCaretToEnd(inputObject)
		{
			var inputObject = this;
			if($.browser.msie)
			{
				  if (inputObject.createTextRange)
				  {
				   var r = inputObject.createTextRange();
				   r.collapse(false);
				   r.select();
				  }
			}else{
				 if (inputObject.selectionStart >= 0)
				 {
				  var end = inputObject.value.length;
				  inputObject.setSelectionRange(end,end);
				  inputObject.focus();
				 }
			} 
		}
		function setCaretPosition(position) 
		{ 
			var inputObject = this;
			if (position > inputObject.value.length - 1 || position < 0) return false;
		    if (inputObject.createTextRange) 
		    { 
		        // IE 
		        var range = inputObject.createTextRange(); 
		        range.collapse(true); 
		        range.moveEnd('character', position); 
		        range.moveStart('character', position); 
		        range.select(); 
		    } 
		    else if (inputObject.selectionEnd  >= 0) 
		    { 
		        // Gecko 
				  inputObject.setSelectionRange(position,position);
				  inputObject.focus();
		    } 
		} 
		
      };
})(jQuery);

