<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="dashboard._Default" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Dash Board</title>
    <link rel="stylesheet" href="css/general.css" type="text/css" media="screen" />
    <script type="text/javascript" src="js/Ajaxcalls.js"></script>
    <script type="text/javascript" src="js/general.js"></script>
<script type="text/javascript">
function load(){
	document.getElementById("popupApptDetail").style.display = 'none';
	initialSelectCampaign();
}
function reloadData(){
    //alert('Reload of graph + Leads results (right side), may have to reload the all page actually!');
    var ctrl=document.getElementById("period");
    var DBctrl=document.getElementById("DB");
    if (ctrl){
        var period=ctrl.selectedIndex;
        var db=DBctrl.innerHTML;
        window.location.href='default.aspx?db='+db+'&p='+period;
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

<body onload="load()">



    <!-- saved from url=(0013)about:internet -->
    <!-- amcharts script-->
    <!-- swf object (version 2.2) is used to detect if flash is installed and include swf in the page -->

    <script type="text/javascript" src="amcharts/flash/swfobject.js"></script>

    <!-- following scripts required for JavaScript version. The order is important! -->

    <script type="text/javascript" src="amcharts/javascript/amcharts.js"></script>

    <script type="text/javascript" src="amcharts/javascript/amfallback.js"></script>

    <script type="text/javascript" src="amcharts/javascript/raphael.js"></script>

<form id="Form1" method="post" runat="server">


    <!-- chart is placed in this div. if you have more than one chart on a page, give unique id for each div -->
    <table width="900px" align="center" cellpadding="0" cellspacing="0" id="Table1" border="0">
        <tr>
            <td style="background-color: #C9E4F5; border-bottom: solid 1px blue;" valign=top width=50%>
                <table width=100%><tr><td width=30%>
                <img src="images/PROIlogo.gif"  style="" /></td><td align=left>
                <span style="font-family:Arial Black; font-size:18px; color:#355010; ">
                    <asp:Label ID="AccountName" runat="server" Text="Label"></asp:Label>
                </td>
                </tr></table>
            </td>
            <td style="background-color: #C9E4F5; padding-right: 10px" align="left" valign="bottom"
                nowrap>
                <span class="menuLinkSelected"><b>Lead Generation</b></span><a href="#" class="menuLink">Reviews</a>
                <a href="#" class="menuLink">Online Reputation</a>
                <a href="#" class="menuLink">Call Tracking</a>
                <a href="#" class="menuLink">Log out</a>
            </td>
        </tr>
        <tr>
            <td style='background-image: url(images/backGroundPie.gif)' colspan="6" align="center">
                <table width="100%">
                    <tr>
                        <td width="80%" align="left">
                            <h3>
                                Leads Generated per Campaign</h3>
                        </td>
                        <td>
                            <asp:DropDownList ID="period" runat="server"  onchange ="reloadData()">
                                <asp:ListItem>Last week</asp:ListItem>
                                <asp:ListItem>Last month</asp:ListItem>
                                <asp:ListItem>Year to date</asp:ListItem>
                                <asp:ListItem>Last year</asp:ListItem>
                                <asp:ListItem>All</asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="chartdiv" style="width: 400px; height: 400px; background-color: #000000">
                </div>
            </td>
            <td style="background-image: url(images/backGroundPie.gif)" width="80%" align="center"                valign="top">
                <br />
                <asp:Label ID="leadTable" runat="server" Text="" class="LeadTable" ></asp:Label>
                
                <br />
                <span style='font-family: Arial Black; font-size: 13px;'>Top Procedures</span>
                
                <asp:Label ID="topProcedures" runat="server" Text=""></asp:Label>
                <!--table class="LeadsDetailTable"  id='Table5' width=100%>
                    <tr>
                        <th>
                            Code
                            </th>
                            <th>
                            Name
                        </th>
                        <th>
                            Date
                        </th>
                        <th>
                            $
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <b>D0011</td><td>Implent Resin
                        </td>
                        <td align='right'>
                            2011/4/2
                        </td>
                        <td align='center'>
                            $120
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>D0011</td><td> Implent Resin
                        </td>
                        <td align='right'>
                            2011/4/2
                        </td>
                        <td align='center'>
                            $120
                        </td>
                    </tr>
                </table-->
                
                
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <span id="campaignDetail"></span>
                
                
                
                
           </td>     
        </tr>
    </table>
    <asp:Label ID="DB" runat="server" ForeColor="White"  ></asp:Label>
    <asp:Label ID="firstCampaign" runat="server" ForeColor="White"  ></asp:Label>

    <div id="popupApptDetail" class="popup" ></div>

    <script type="text/javascript">

			var params = {
		        wmode:"opaque"
			     
            };

		    var flashVars = {
		        path: "amcharts/flash/",
		        settings_file: encodeURIComponent("settings.xml"),
		        data_file: encodeURIComponent("data"+document.getElementById("period").selectedIndex+".xml")
			};

			// change 8 to 80 to test javascript version
            if (swfobject.hasFlashPlayerVersion("8")){
	    		swfobject.embedSWF("amcharts/flash/ampie.swf", "chartdiv", "450", "400", "8.0.0", "amcharts/flash/expressInstall.swf", flashVars, params);
	    	}
			else{
			    // Note, as this example loads external data, JavaScript version might only work on server
				var amFallback = new AmCharts.AmFallback();
    			amFallback.path = "amcharts/flash/";
				amFallback.settingsFile = flashVars.settings_file;
				amFallback.dataFile = flashVars.data_file;				
				amFallback.type = "pie";
				amFallback.write("chartdiv");

			}
    </script>
    
</form>    
</body>
</html>
