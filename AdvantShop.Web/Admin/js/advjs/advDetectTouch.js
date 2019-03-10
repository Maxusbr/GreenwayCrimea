(function (jQuery) {
    	jQuery.extend({
	    	IsTouchDevice: function (){
				 var ua = navigator.userAgent;
				 if(ua.match(/iPhone|iPod|iPad|BlackBerry|Android/))
					return true;
				 else
					return false;	
			},
			TouchDevice: function (){
				 var ua = navigator.userAgent;
				 ua = ua.match(/iPhone|iPod|iPad|BlackBerry|Android/);	
				 return ua;
			}	 
        });
})(jQuery);



