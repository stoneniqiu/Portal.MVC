
$(function() {

	$('.etip a').tooltip({ 'placement':'left' });
	$('.wtip a').tooltip({ 'placement':'right' });
	$('.tip a').tooltip();
	
	 
	// NewsUpdate
	$('#news_update').vTicker({ 
		speed: 500,
		pause: 8000,
		animation: 'fade',
		mousePause: true,
		showItems: 3,				
		height:380
	});
	
//Flickr
	jQuery('ul#flickr-badge').jflickrfeed({
		limit: 6,
		qstrings: {
			id: '11821713@N00'
		},
		itemTemplate: '<li><a href="http://www.flickr.com/photos/11821713@N00"><img src="{{image_s}}" alt="{{title}}" /></a></li>'
	});
	
	// zoomOverlay
	$("<span class='zoomOverlay'/><div class='image_highlight'/>").appendTo('.zoom');
	
	// fancybox
	$(".packages-items a.zoom").fancybox();
	
	// slider
	 $('.flexslider').flexslider();
	$('.aslider').flexslider({
		animation: "slide",
		controlNav: true, 
		directionNav: false 
	});

    //$(".carousel_box").flexslider({	    
    //    slideshowSpeed: 15000,
    //});

    //$(".carousel_box").flexslider({
	//	animation: "slide",
	//	controlNav: false, 
	//	directionNav: true, 
	//	slideshowSpeed: 15000,
	//});
	
	// Initialize the gallery
	$('#thumbs a').touchTouch();
	$('#mini_gallery a').touchTouch();
	 
	
	/* theme changer */
	$('.open-close-demo').click(function() {
			  if ($(this).parent().css('left') == '-148px') {
				  $(this).parent().animate({
					  "left": "0"
				  }, 300);
			  } else {
				  $(this).parent().animate({
					  "left": "-148px"
				  }, 300);
			  }
	});
	 
	$(".color-themes").click( function() {
			var colors=$(this).attr('rel');
			chooseStyle(colors, 60);
	 });
	$(".pat").click(function(){
		$("body").removeClass('bg-img');
		if( $('#alpha-style').is(':checked')){
				$('#alpha-style').attr("checked",false);
				$('.bg-alpha').hide();
			}
	});
	$(".drak-theme").click(function(){
									window.location.href='../dark/';
	});
	$(".light-theme").click(function(){
									window.location.href='../light/';
	});
    $("#pat1").click(function(){
        $("body").css({ 'background': 'url(../images/bg.jpg) repeat fixed' });
        return false;
    });
    
    $("#pat2").click(function(){
        $("body").css({ 'background': 'url(../images/switcher/2.jpg) repeat fixed' });
        return false;
    });

    $("#pat3").click(function(){
		 $("body").css({'background':'url(../images/switcher/3.jpg) repeat fixed'});
        return false;
    });
	
    $("#pat4").click(function(){
        $("body").css({ 'background': 'url(../images/switcher/4.jpg) repeat fixed' });
        return false;
    });
    $("#pat5").click(function(){
        $("body").css({ 'background': 'url(../images/switcher/5.jpg) repeat fixed' });
        return false;
    });
    $("#pat6").click(function(){
        $("body").css({ 'background': 'url(../images/switcher/6.jpg) repeat fixed' });
        return false;
    });
    $("#pat7").click(function(){
        $("body").css({ 'background': 'url(../images/switcher/7.jpg) repeat fixed' });
        return false;
    });
 
    $("#backgr-image").click(function(){
        $("body").addClass('bg-img').css({ 'background': 'url(../images/background_1.jpg) no-repeat center top fixed', '-webkit-background-size': 'cover', '-moz-background-size': 'cover', '-o-background-size': 'cover', 'background-size': 'cover' });
		 $('#alpha-style').attr('checked','checked');
		 alpha();
        return false;
    });
    $("#backgr-image2").click(function(){
        $("body").addClass('bg-img').css({ 'background': 'url(../images/background_2.jpg) no-repeat center top fixed', '-webkit-background-size': 'cover', '-moz-background-size': 'cover', '-o-background-size': 'cover', 'background-size': 'cover' })
		 $('#alpha-style').attr('checked','checked');
		 alpha();
        return false;
    });
    $("#backgr-image3").click(function(){
        $("body").addClass('bg-img').css({ 'background': 'url(../images/background_3.jpg) no-repeat center top fixed', '-webkit-background-size': 'cover', '-moz-background-size': 'cover', '-o-background-size': 'cover', 'background-size': 'cover' });
		 $('#alpha-style').attr('checked','checked');
		 alpha();
        return false;
    });
    $("#backgr-image4").click(function(){
        $("body").addClass('bg-img').css({ 'background': 'url(../images/background_4.jpg) no-repeat center top fixed', '-webkit-background-size': 'cover', '-moz-background-size': 'cover', '-o-background-size': 'cover', 'background-size': 'cover' })
		 $('#alpha-style').attr('checked','checked');
		 alpha();
        return false;
    });
	function alpha(){
		  var check=$('#alpha-style').attr('checked');
					if(check){
						$('.bg-blacklight').append('<div class="bg-alpha"></div>');
					}else{
					   $('.bg-alpha').hide();
					}	
	}
	// Check browser fixbug
	var mybrowser=navigator.userAgent;
	if(mybrowser.indexOf('MSIE')>0){$('#scroller  ').css('padding-top', '20px'); $('#imglist li ').css('margin-top', '25px');}
	if(mybrowser.indexOf('Firefox')>0){}	
	if(mybrowser.indexOf('Presto')>0){ $(' .stack  .title').css('margin-top', '135px');}
	if(mybrowser.indexOf('Chrome')>0){}		
	if(mybrowser.indexOf('Safari')>0){}		

});	
	// Contact Submiting form
	function Contact_form(form, options){
		// text on load you change , 0: No Overlay , 1 loading with  Overlay 
		 loading('Loading',1); 
		 var data=form.serialize();		
		$.ajax({
			url: "contact.php",
			data: data,
			success: function(data){	
				  if(data.check==0){ // if Ajax respone data.check =0 or not complete
					  $('#preloader').fadeOut(400,function(){ $(this).remove(); });		
					  			alert("No complete");
					   return false;
				  }
				  if(data.check==1){ // if Ajax respone data.check =1 or Complete
					 $('.notifications').slideDown(function(){
								setTimeout("$('.notifications').slideUp();",7000); 								
							}); // Show  notifications box
					 $('#contactform').get(0).reset();  //  reset form
						unloading();	 //  remove loading
				  }
			},
			cache: false,type: "POST",dataType: 'json'
		});
	}
	// Loading
	  function loading(name,overlay) { 
			$('body').append('<div id="overlay"></div><div id="preloader">'+name+'..</div>');
					if(overlay==1){
					  $('#overlay').css('opacity',0.4).fadeIn(400,function(){  $('#preloader').fadeIn(400);	});
					  return  false;
			   }
			$('#preloader').fadeIn();	  
	   }
	   // Unloading
	  function unloading() { 
			$('#preloader').fadeOut(400,function(){ $('#overlay').fadeOut(); $.fancybox.close(); }).remove();
	   }
$(window).load(function() {
						   
	// BlackAndWhite
	$('.bwWrapper').BlackAndWhite({
		hoverEffect:true,
		webworkerPath: 'components/BlackAndWhite/'
	});
	
});	
