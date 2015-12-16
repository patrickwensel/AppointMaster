// JScript source code
/*********************************************************************************/
var count=0;

function signature(){
	var d = new Date();
	return d.getDay()+"."+d.getHours()+"."+d.getMinutes()+"."+d.getSeconds() +"."+d.getMilliseconds()
}

function processHTMLFlow(req) {
    if (req.readyState == 4) {
        if (req.status == 200) {
			//We read the message to determine what to display and Where
			//stream should be on the form :  [elementId |show|hide|focus|css|js] | [content | elementId | URL ] ...
			//   where | content |where | content .....
			//   [hide/show] | where
			var mySplitResult = req.responseText.split("|");
			var where='';
			var content='';
			for (i=0;i<mySplitResult.length;i=i+2){
				
				if (mySplitResult[i]=="show"){
					if (document.getElementById(mySplitResult[i+1])!=null){
						document.getElementById(mySplitResult[i+1]).style.display = '';
					}else
						alert("cannot find elt '+mySplitResult[i+1]");
				}else if (mySplitResult[i]=="hide"){
					if (document.getElementById(mySplitResult[i+1])!=null){
						document.getElementById(mySplitResult[i+1]).style.display = 'none';
					}
				}else if (mySplitResult[i]=="exec"){
					var func=mySplitResult[i+1];
					if (func=="showDrop"){
						showDropDown();
					}else if (func=="hideDrop"){
						hideDropDown();
					}
					
				}else if (mySplitResult[i]=="focus"){
					if (document.getElementById(mySplitResult[i+1])!=null){
						document.getElementById(mySplitResult[i+1]).focus();
					}
				}else if (mySplitResult[i]=="css"){
					if (mySplitResult[i+1]!='')	loadcss(mySplitResult[i+1]);
				}else if (mySplitResult[i]=="js"){
					if (mySplitResult[i+1]!='')	loadJavascript(mySplitResult[i+1]);
				}else {
					if (document.getElementById(mySplitResult[i])){
						var ctrl=document.getElementById(mySplitResult[i]);
						if (ctrl){
							
							//alert(ctrl.nodeName);
							if (ctrl.nodeName=="TEXTAREA"){
								ctrl.value=mySplitResult[i+1].replace(/@CR/g,"\n");
							}else if (ctrl.nodeName=="INPUT"){
								ctrl.value=mySplitResult[i+1].replace(/@CR/g,"\n");
							}else if (ctrl.nodeName=="IMG"){
								ctrl.src=mySplitResult[i+1];
							} else{
								//ctrl.value=mySplitResult[i+1];
								
								ctrl.innerHTML=mySplitResult[i+1];
								//alert(mySplitResult[i]+" type="+ctrl.nodeName+" value="+mySplitResult[i+1]);
								//if (mySplitResult[i]=='DateOfNote') document.getElementById('DateOfNote').innerHTML=mySplitResult[i+1];///Fucking stupid Javascript de merde  DO NOT ASK!!!!!!
							}
						}else
						Alert('Unkonwn ctrl');
					}
				}
			}
        } else {
            alert("There was a problem retrieving the XML data:\n" +req.statusText);
        }
    }
}
function downLoadHTMLFlow(url) {
	var req=false;
	
	if (window.XMLHttpRequest) {
			req = new XMLHttpRequest();
	} else if (window.ActiveXObject) {
			req = new ActiveXObject("Microsoft.XMLHTTP");
	}
    count++;
      if (req) {
			req.onreadystatechange = function() { processHTMLFlow(req); };
            req.open("GET", url +"&count="+signature() , true);
            req.send(null);
      }else{
	  }
}


function loadCampaignDetail(rowNbr,campaignId,fromList){


    var ctrlDiv = document.getElementById("campaignDetail");
    if (ctrlDiv) {
        ctrlDiv.innerHTML = "<div style='background-color:white; font-size:15px;height:400px;width:100%;text-align:center;font-weight:bold;'>Loading Campaign Leads...<br><br><img src='images/ajax-loader.gif'></div>";
        //alert('Loading...');
    }
    
    
    
    var s='dashboard.aspx?campDetail='+campaignId;
    
    downLoadHTMLFlow(s);
    
    var table=document.getElementById('LeadTableTable');
    if (table){
        var rowCount = table.rows.length; 
        for (i=0;i<rowCount;i++){
            var row=document.getElementById('leadtableRow'+i);
            if (row){
                row.style.backgroundColor ='';
                row.style.color='';
                row.style.fontWeight='';
                row.style.fontSize = '';
                row.className = "practiceroi-summary-row";
            }
        } 
        row=document.getElementById('leadtableRow'+rowNbr);
        if (row) {
            row.className = "practiceroi-summary-row row-selected";
            var container = document.getElementById('scrolling-campaign-div');
            if (container) {
                container.scrollTop = row.offsetTop;
            }
        }
    }
    if (fromList){
        var flashMovie = document.getElementById('summary-chart-container');
        if (flashMovie)
            flashMovie.clickSlice(rowNbr);
    }
    
}

function loadApptDetail(apptId){

}




//********************
//**Test
//********************

function processSimpleHTMLFlow(req) {
    if (req.readyState == 4) {
        if (req.status == 200) {
		    var ctrl=document.getElementById('target');
		    if (ctrl){
				    ctrl.innerHTML=req.responseText;
            } else {
                alert("Cannot find target");
            }
        }else {
                alert("There was a problem retrieving the XML data:\n" +req.statusText);
        }
    }
}

function downLoadSimpleHTML(url) {
	var req=false;
	
	if (window.XMLHttpRequest) {
			req = new XMLHttpRequest();
	} else if (window.ActiveXObject) {
			req = new ActiveXObject("Microsoft.XMLHTTP");
	}
  if (req) {
		req.onreadystatechange = function() { processSimpleHTMLFlow(req); };
        req.open("GET", url , true);
        req.send(null);
  }else{
  }
}


//********************
