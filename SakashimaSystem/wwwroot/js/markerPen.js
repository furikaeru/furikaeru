!function(e){e.extend({markerPen:function(n){function r(e,r,t){t&&(s.beginPath(),"marker"==d?(s.globalCompositeOperation="source-over",s.strokeStyle=n.color,s.lineWidth=n.stroke,s.lineJoin="round",s.moveTo(o,i),s.lineTo(e,r),s.closePath(),s.stroke()):(s.globalCompositeOperation="destination-out",s.arc(o,i,8,0,2*Math.PI,!1),s.fill())),o=e,i=r}function t(){s.setTransform(1,0,0,1,0,0),s.clearRect(0,0,s.canvas.width,s.canvas.height)}var a={color:"yellow",stroke:10,opacity:.7};n=e.extend(a,n),e("body").append("<div id='marker_pen_main'><canvas id='markerPenCanvas'></canvas></div><div id='marker_pen_controls'><a id='hidebtn'>Hide Markings</a><a id='togglebtn'>Disable markerPen</a><a id='clearbtn'>Clear</a><a id='markerbtn'>Marker</a><a id='eraserbtn'>Eraser</a></div><div id='marker_pen_hide_control'><a id='hidecontrols' style='left:1%'><</a></div>"),e("#marker_pen_controls a, #hidecontrols").css({cursor:"pointer",padding:".5%"}),e("#marker_pen_main").css({position:"absolute",top:"0%",left:"0%"}),e("#marker_pen_controls").css({width:"100%",position:"fixed",bottom:"0",padding:"1%","background-color":"rgb(51,51,51)",color:"rgb(255,255,255)","padding-left":"4%",left:0}),e("#marker_pen_hide_control").css({position:"fixed",bottom:"0",padding:"1%","padding-left":"2%","padding-right":"2%","background-color":"rgb(51,51,51)",color:"rgb(255,255,255)",left:0}),e("#marker_pen_controls a").css({"padding-left":"2%","padding-right":"2%","margin-left":"1%"}),e("#markerPenCanvas").css({opacity:n.opacity}),e("#marker_pen_controls a").hover(function(){e(this).css("background-color","rgb(0,0,0)")},function(){e(this).removeAttr("style"),e(this).css({padding:".5%","padding-left":"2%","padding-right":"2%","margin-left":"1%",cursor:"pointer"}),"marker"==d?e("#markerbtn").css({"background-color":"rgb(0,0,0)"}):e("#eraserbtn").css({"background-color":"rgb(0,0,0)"})}),e("#hidebtn").css({opacity:"0.2","pointer-events":"none"}),e("#marker_pen_svg").css({position:"absolute"});var o,i,s,c=!1,d="marker",l=1,m=0,g=0;s=document.getElementById("markerPenCanvas").getContext("2d"),s.canvas.height=e(document).height()+e("#marker_pen_main").height(),s.canvas.width=e(document).width(),e("#markerPenCanvas").mousedown(function(n){c=!0,r(n.pageX-e(this).offset().left,n.pageY-e(this).offset().top,!1),"eraser"==d&&r(n.pageX-e(this).offset().left,n.pageY-e(this).offset().top,!0)}),e("#markerPenCanvas").mousemove(function(n){c&&r(n.pageX-e(this).offset().left,n.pageY-e(this).offset().top,!0)}),e("#markerbtn").css({"background-color":"rgb(0,0,0)"}),e("#markerPenCanvas").mouseup(function(e){c=!1}),e("#markerPenCanvas").mouseleave(function(e){c=!1}),e("#clearbtn").click(function(){t()}),e("#hidecontrols").click(function(){0==g?(e("#marker_pen_controls").animate({left:"-100%"}),document.getElementById("hidecontrols").innerHTML=">",g=1):(e("#marker_pen_controls").animate({left:"0%"}),document.getElementById("hidecontrols").innerHTML="<",g=0)}),e("#markerbtn").click(function(){d="marker",e("#markerbtn").css({"background-color":"rgb(0,0,0)"}),e("#eraserbtn").removeAttr("style"),e("#marker_pen_controls a").css({padding:".5%","padding-left":"2%","padding-right":"2%","margin-left":"1%",cursor:"pointer"})}),e("#eraserbtn").click(function(){d="eraser",e("#eraserbtn").css({"background-color":"rgb(0,0,0)"}),e("#markerbtn").removeAttr("style"),e("#marker_pen_controls a").css({padding:".5%","padding-left":"2%","padding-right":"2%","margin-left":"1%",cursor:"pointer"})}),e("#togglebtn").click(function(){1==l?(l=0,document.getElementById("togglebtn").innerHTML="Enable markerPen",e("#marker_pen_main, #clearbtn, #markerbtn, #eraserbtn").css({"pointer-events":"none"}),e("#clearbtn, #markerbtn, #eraserbtn").css({opacity:.2}),e("#hidebtn").css({opacity:1,"pointer-events":"auto"})):(l=1,document.getElementById("togglebtn").innerHTML="Disable markerPen",e("#marker_pen_main, #clearbtn, #markerbtn, #eraserbtn").css({"pointer-events":"auto"}),e("#clearbtn, #markerbtn, #eraserbtn").css({opacity:1}),e("#hidebtn").css({opacity:.2,"pointer-events":"none"}),m=0,document.getElementById("hidebtn").innerHTML="Hide Markings",e("#marker_pen_main").css({display:"block"}))}),e("#hidebtn").click(function(){1==m?(m=0,document.getElementById("hidebtn").innerHTML="Hide Markings",e("#marker_pen_main").css({display:"block"})):(m=1,document.getElementById("hidebtn").innerHTML="Show Markings",e("#marker_pen_main").css({display:"none"}))})}})}(jQuery);