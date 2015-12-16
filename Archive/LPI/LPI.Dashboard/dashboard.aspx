<%@ Register TagPrefix="eo" Namespace="EO.Web" Assembly="EO.Web" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="dashboard.dashboard" %>
<!DOCTYPE html>
<head>
	<meta charset="utf-8" />
    <title>Dash Board</title>
	<link rel="stylesheet" href="css/base.css">
	<link rel="stylesheet" href="css/popups.css">
	<link rel="stylesheet" href="css/practiceroi.css">
	<link rel="stylesheet" href="css/easybox.css">
    
  <script type="text/javascript" src="sound/jquery.min.js"></script>
  <script type="text/javascript" src="sound/jQuery.jPlayer.2.4.0/jquery.jplayer.min.js"></script>
  <!-- script type="text/javascript" src="sound/jquery.jplayer.min.js"></script -->
  <script type="text/javascript" src="sound/audio.js"></script>
    <script type="text/javascript" src="js/Ajaxcalls.js"></script>
    <script type="text/javascript" src="js/general.js"></script>
    <script type="text/javascript">
function loadAM() {
    closeApptDetailDetail();
	document.getElementById("popUpSound").style.display = 'none';
	initialSelectCampaign();
	createPlayer();
	var ctrl1 = document.getElementById("iFrameIndicatore");
	if (ctrl1) {
	    var ctrl=document.getElementById("center-column");
	    if (ctrl1.innerHTML=="1"){
	        ctrl.className = "center-Iframe";
	        ctrl1.style.display = 'none';
	    }
    }
}

function CustomDatesClick() {
    var ctrl = document.getElementById("period");
    if (ctrl) {
        var period = ctrl.selectedIndex;
        if (period >= 5){
            var ctrl2 = document.getElementById("DivSelectDates");
            if (ctrl2)
                ctrl2.style.display = '';
        }
    }
}
function CloseCustomDates() {
    var ctrl2 = document.getElementById("DivSelectDates");
    if (ctrl2)
        ctrl2.style.display = 'none';
}
function DateChanged() {
    var ctrl = document.getElementById("period");
    if (ctrl) {
        var period = ctrl.selectedIndex;
        if (period < 5)
            reloadData();
        else {
            var ctrl2 = document.getElementById("DivSelectDates");
            if (ctrl2)
                ctrl2.style.display = '';    
        }    
    }
}

function reloadData() {
    //alert('Reload of graph + Leads results (right side), may have to reload the all page actually!');
    var ctrl = document.getElementById("period");
    var DBctrl = document.getElementById("DB");
    var TPCtrl = document.getElementById("displayMode");
    if (ctrl) {
        var period = ctrl.selectedIndex;
        var db = DBctrl.innerHTML;
        var tp = TPCtrl.selectedIndex;
        window.location.href = 'dashboard.aspx?db=' + db + '&p=' + period + "&TP=" + tp;
    }
}

function initialSelectCampaign(){
    var Cmpctrl=document.getElementById("firstCampaign");
    if (Cmpctrl){
        var campId=Cmpctrl.innerHTML;
        loadCampaignDetail(0,campId,false);
    }

}

    </script>


</head>
<body onload="loadAM()">
    <form id="Form1" method="post" runat="server">
    <!-- saved from url=(0013)about:internet -->
    <!-- amcharts script-->
    <!-- swf object (version 2.2) is used to detect if flash is installed and include swf in the page -->

    <script type="text/javascript" src="amcharts/flash/swfobject.js"></script>
    <script type="text/javascript" src="amcharts/javascript/amcharts.js"></script>
    <script type="text/javascript" src="amcharts/javascript/amfallback.js"></script>
    <script type="text/javascript" src="amcharts/javascript/raphael.js"></script>
    
    <!-- chart is placed in this div. if you have more than one chart on a page, give unique id for each div -->


    <div id="screenBackGround">
        <asp:Label ID="iFrameIndicatore" runat="server" Text="Label"></asp:Label>
        
        <asp:Label ID="EntryURL" runat="server" style="display:none"></asp:Label>
        
    </div>


    
    <div id="center-column" class="center-column">
    

<asp:Panel ID="PanelHeader" runat="server">
    <div id="header">
    <div id="header-title">Practice ROI</div>
    <div id="header-practice"><asp:Label ID="AccountName" runat="server" Text="Label"></asp:Label></div>
    </div>
</asp:Panel>

<asp:Panel ID="PanelIframeHeader" runat="server" Visible=false>
    <div id="center-Iframe">
        </asp:Panel>

    <div id="toolbar">
        <div id="toolbar-buttons">
            <a class="lightbox" title="Campaign Settings" data-width="600" data-height="400" href="campaignNames.aspx">
                <img style="padding: 0 5px 0 0;" src="images/settings.png" /></a> 
                
                <asp:HyperLink ID="LinkLogOut" runat="server" ImageUrl="images/logout.png" 
        NavigateUrl="login.aspx?logout=1">HyperLink</asp:HyperLink>
                                   
                    </div>
        <div style=" margin-top:5px">
        <label>
            LEADS PERIOD
        <asp:DropDownList ID="period" runat="server" onchange="DateChanged()" onclick="CustomDatesClick()" >
            <asp:ListItem>Last week</asp:ListItem>
            <asp:ListItem>Last month</asp:ListItem>
            <asp:ListItem>Year to date</asp:ListItem>
            <asp:ListItem>Last year</asp:ListItem>
            <asp:ListItem>All</asp:ListItem>
            <asp:ListItem>Custom ...</asp:ListItem>
        </asp:DropDownList>
        
        <div id="DivSelectDates" style="background-color:#E0E0FF; z-index:2; border:solid 3px #556688; position:absolute; padding:10px; color:#555555; display:none; margin:20px;border-radius:25px;box-shadow: 10px 10px 5px #888888; ">
            <img src="images/close.png" onclick="CloseCustomDates()" style="border:none; float:right; margin:0px; " border=0/>
        <h4>Custom Leads Date Range</h4><br />
        <table style=" background-color:white">
            <tr>
                <td>From</td>
                <td><eo:DatePicker ID="fromDate" runat="server" ControlSkinID="None" DayCellHeight="16" DayCellWidth="19" DayHeaderFormat="FirstLetter" OtherMonthDayVisible="True" TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" 
                                    TitleRightArrowImageUrl="DefaultSubMenuIcon" VisibleDate="2014-12-01" Width="120">
            <SelectedDayStyle CssText="font-family: tahoma; font-size: 10px; background-color: #fbe694; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
            <dayhoverstyle csstext="font-family: tahoma; font-size: 10px; border-right: #fbe694 1px solid; border-top: #fbe694 1px solid; border-left: #fbe694 1px solid; border-bottom: #fbe694 1px solid" /><titlearrowstyle csstext="cursor:hand" />
            <pickerstyle csstext="font-family:Courier New; padding-left:5px; padding-right: 5px;" />
            <todaystyle csstext="font-family: tahoma; font-size: 12px; border-right: #bb5503 1px solid; border-top: #bb5503 1px solid; border-left: #bb5503 1px solid; border-bottom: #bb5503 1px solid" />
            <monthstyle csstext="font-family: tahoma; font-size: 12px; margin-left: 14px; cursor: hand; margin-right: 14px" />
            <disableddaystyle csstext="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
            <DayHeaderStyle CssText="font-family: tahoma; font-size: 12px; border-bottom: #aca899 1px solid" />
            <calendarstyle csstext="background-color: white; border-right: #7f9db9 1px solid; padding-right: 4px; border-top: #7f9db9 1px solid; padding-left: 4px; font-size: 9px; padding-bottom: 4px; border-left: #7f9db9 1px solid; padding-top: 4px; border-bottom: #7f9db9 1px solid; font-family: tahoma" />
            <OtherMonthDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
            <DayStyle CssText="font-family: tahoma; font-size: 12px; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
            <TitleStyle CssText="background-color:#9ebef5;font-family:Tahoma;font-size:12px;padding-bottom:2px;padding-left:6px;padding-right:6px;padding-top:2px;" />
                                    </eo:DatePicker>
        </td>
                <td>To</td>
                <td><eo:DatePicker ID="toDate" runat="server" ControlSkinID="None" 
                                    DayCellHeight="16" DayCellWidth="19" DayHeaderFormat="FirstLetter" 
                                    OtherMonthDayVisible="True" TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" 
                                    TitleRightArrowImageUrl="DefaultSubMenuIcon" VisibleDate="2014-12-01" Width="120"><SelectedDayStyle CssText="font-family: tahoma; font-size: 12px; background-color: #fbe694; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" /><dayhoverstyle csstext="font-family: tahoma; font-size: 12px; border-right: #fbe694 1px solid; border-top: #fbe694 1px solid; border-left: #fbe694 1px solid; border-bottom: #fbe694 1px solid" /><titlearrowstyle csstext="cursor:hand" /><pickerstyle csstext="font-family:Courier New; padding-left:5px; padding-right: 5px;" /><todaystyle csstext="font-family: tahoma; font-size: 12px; border-right: #bb5503 1px solid; border-top: #bb5503 1px solid; border-left: #bb5503 1px solid; border-bottom: #bb5503 1px solid" /><monthstyle csstext="font-family: tahoma; font-size: 12px; margin-left: 14px; cursor: hand; margin-right: 14px" /><disableddaystyle csstext="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" /><DayHeaderStyle CssText="font-family: tahoma; font-size: 12px; border-bottom: #aca899 1px solid" /><calendarstyle csstext="background-color: white; border-right: #7f9db9 1px solid; padding-right: 4px; border-top: #7f9db9 1px solid; padding-left: 4px; font-size: 9px; padding-bottom: 4px; border-left: #7f9db9 1px solid; padding-top: 4px; border-bottom: #7f9db9 1px solid; font-family: tahoma" /><OtherMonthDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" /><DayStyle CssText="font-family: tahoma; font-size: 12px; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" /><TitleStyle CssText="background-color:#9ebef5;font-family:Tahoma;font-size:12px;padding-bottom:2px;padding-left:6px;padding-right:6px;padding-top:2px;" /></eo:DatePicker></td>
            </tr>
        </table>
                                
                                        <center><br />
                                        <asp:Button ID="pbPeriodSelec" runat="server" Text="Set Date Range" onclick="pbPeriodSelec_Click" />
                                        </center>
        </div>
        
        
        <label>
            AMOUNTS DISPLAYED</label>
        <asp:DropDownList ID="displayMode" runat="server" onchange="reloadData()">
            <asp:ListItem>Production Revenue (PROD.)</asp:ListItem>
            <asp:ListItem>Planned Revenue (PLAN.)</asp:ListItem>
            <asp:ListItem>Tracked Revenue</asp:ListItem>
        </asp:DropDownList>
        </div>
    </div>

    

     <asp:Label ID="leadTable" runat="server" Text="" class="LeadTable"></asp:Label>
                
<span id="campaignDetail"></span>                
                
    <asp:Label ID="DB" runat="server" ForeColor="White"></asp:Label>
    <asp:Label ID="GenerationNumber" runat="server" ForeColor="Red"></asp:Label>
        <asp:Label ID="superLink" runat="server"></asp:Label>
    <asp:Label ID="firstCampaign" runat="server" ForeColor="White"></asp:Label>
    
    


        <div id="popupApptDetail" class="popup">
        </div>



<!-- data_file: encodeURIComponent("data_"+document.getElementById("DB").innerHTML+"_"+document.getElementById("period").selectedIndex+".xml") -->
<!-- data_file: encodeURIComponent("data_test3.xml") -->
    
    <script type="text/javascript">
		    var ctrlgenNumber=document.getElementById("GenerationNumber");
		    if (ctrlgenNumber)
		            ctrlgenNumber.style.display='none';
		    var genNumber=document.getElementById("GenerationNumber").innerHTML;
		    var Db=document.getElementById("DB").innerHTML;
			var params = {
		        wmode:"opaque"
			     
            };
            
		    var flashVars = {
		        path: "amcharts/flash/",
		        settings_file: encodeURIComponent("settings.xml"),
		        
		        data_file: encodeURIComponent("data_"+Db+"_"+genNumber+".xml") 
			};

			// change 8 to 80 to test javascript version
            if (swfobject.hasFlashPlayerVersion("8")){
                swfobject.embedSWF("amcharts/flash/ampie.swf", "summary-chart-container", "300", "300", "8.0.0", "amcharts/flash/expressInstall.swf", flashVars, params);
	    	}
			else{
			    // Note, as this example loads external data, JavaScript version might only work on server
				var amFallback = new AmCharts.AmFallback();
    			amFallback.path = "amcharts/flash/";
				amFallback.settingsFile = flashVars.settings_file;
				amFallback.dataFile = flashVars.data_file;				
				amFallback.type = "pie";
				amFallback.write("summary-chart-container");

			}
			
			
			
			
    </script>

</center>    
    <div id="popUpSound" class="popUpSound">
        <span id="playerSound"></span>
        <INPUT type=button value=Close id=pbSave onclick="document.getElementById('popUpSound').style.display = 'none';" NAME='pbCloseSound' style="text-align:center">
    </div>



  <div id="jquery_jplayer_1" class="jp-jplayer"></div>
  <div id="jp_container_1" class="jp-audio">
    
    <asp:Label ID="labelError" runat="server" ForeColor="#CC6600"></asp:Label>

    </form>

<span id="target"></span>

    
</body>
</html>
