//****************************************************************************
//****  Lead Detail Window
//****************************************************************************
function processopenFCLeadWindow(req) {
    // only if req shows "loaded"
    if (req.readyState == 4) {
        if (req.status == 200) {
			document.getElementById("popupApptDetail").innerHTML = req.responseText; 
			/*var count = req.responseText.match(/<tr>/g); 
            var w = 330;
            var h =300;	
            var vOffset=0;
            if (count>9){
                h=550;		
                vOffset=50;
            }
            var xc = Math.round((document.body.clientWidth/2));
            var yc = Math.round((document.body.clientHeight-h)/2);
            yc=yc-vOffset;

            if (navigator.appName == "Microsoft Internet Explorer")
	            {//alert("You're at " + document.body.scrollTop + " pixels.");
	            yc+=document.body.scrollTop;}
            else
	            {//alert("You're at " + window.pageYOffset + " pixels.");
	            yc+=window.pageYOffset;}
            xc=xc-200;
            document.getElementById("popupApptDetail").style.left = xc + 'px';
            document.getElementById("popupApptDetail").style.top = yc + 'px';*/
			
			document.getElementById("screenBackGround").style.display = ''
            document.getElementById("popupApptDetail").style.display = '';
            document.getElementById("popupApptDetail").focus();
        } else {
            alert("There was a problem retrieving the XML data:\n" +
                req.statusText);
        }
    }
}

function openFCLeadWindow(parameters) {
	if (document.getElementById("popupApptDetail").style.display == 'none'){
        var s='dashboard.aspx?'+parameters+"&count="+signature() ;
		var req=false;
		if (window.XMLHttpRequest) {
				req = new XMLHttpRequest();
		} else if (window.ActiveXObject) {
				req = new ActiveXObject("Microsoft.XMLHTTP");
		}
		count++;
		if (req) {
				req.onreadystatechange = function() { processopenFCLeadWindow(req); };
				req.open("GET", s , true);
				req.send(null);
		}else{
		}
	}
}

//****************************************************************************
//****  Appointment Detail Window
//****************************************************************************
function processopenApptDetailDetail(req) {
    // only if req shows "loaded"
    if (req.readyState == 4) {
        if (req.status == 200) {
			document.getElementById("popupApptDetail").innerHTML = req.responseText; 
			document.getElementById("screenBackGround").style.display = '';
			document.getElementById("popupApptDetail").style.display = '';
			document.getElementById("popupApptDetail").focus();
        } else {
            alert("There was a problem retrieving the XML data:\n" +
                req.statusText);
        }
    }
}

function openApptDetailDetail(ApptDetailId) {
	//if (document.getElementById("popupApptDetail").style.display == 'none'){
        var s='dashboard.aspx?apptDetail='+ApptDetailId+"&count="+signature() ;
		var req=false;
		if (window.XMLHttpRequest) {
				req = new XMLHttpRequest();
		} else if (window.ActiveXObject) {
				req = new ActiveXObject("Microsoft.XMLHTTP");
		}
		count++;
		if (req) {
				req.onreadystatechange = function() { processopenApptDetailDetail(req); };
				req.open("GET", s , true);
				req.send(null);
		}else{
		}
	//}
}
//****************************************************************************

function closeApptDetailDetail() {
    document.getElementById("popupApptDetail").style.display = 'none';
    document.getElementById("screenBackGround").style.display = 'none';	
}

function openSoundWindow(soundURL) {
    var divSound=document.getElementById("popUpSound");
    var SoundFilePlayer=document.getElementById("playerSound");
    var tot =document.getElementById("aa");
    
    //alert(soundURL+' '+divSound+' '+SoundFilePlayer+' '+tot );
    if (SoundFilePlayer) {
        //alert('Debug1');
        if (divSound)  {
            if (divSound.style.display=='none'){
                //alert('Debug2 '+soundURL);
                SoundFilePlayer.innerHTML='<EMBED align=baseline src="'+soundURL+'" border="0" AUTOSTART="true">';
                divSound.style.display = '';
            }
        }
    }
}


