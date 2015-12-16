<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AM.aspx.cs" Inherits="dashboard.AM" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Reminders & Other Services</title>
    <style type="text/css" media="screen">
        #summary-chart-container
        {
            visibility: hidden;
        }
    </style>
    <link href="style.css" rel="stylesheet" type="text/css">
    <link href="css/general.css" rel="stylesheet" type="text/css">

    <script type="text/javascript" src="js/Ajaxcalls.js"></script>

    <script type="text/javascript" src="js/general.js"></script>

    <script type="text/javascript">
function load(){
}
    </script>
</head>

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
        <table width="977px" align="center" cellpadding="0" cellspacing="0" id="Table1" border="0" style="margin:0px">
            <tr>
                <td style="background-color: #C9E4F5; width:488;" valign="top" width="488px">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td width="30%">
                                    <img src="images/logoPROI.gif" style="">
                                </td>
                                <td align="left">
                                    <span style="font-family: Arial Black; font-size: 18px; color: #355010;"><asp:Label ID="AccountName" runat="server" Text=""></asp:Label></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="background-color: #C9E4F5; width:488; padding-right: 1px" align="left" valign="bottom" nowrap=""  width="488px">
                    <div class="animatedtabs">
                        <ul>
                            <li><a href="dashboard.aspx?p=4"><span>Lead Generation</span></a></li>
                            <li><a href="reviews.aspx"><span>Reviews</span></a></li>
                            <li class="selected"><a href="AM.aspx"><span>Reminders</span></a></li>
                            <!-- li><a href="localListings.aspx"><span>Local Listings</span></a></li -->
                            <li><a href="login.aspx?logout=1"><span>Log out</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
            </table>
        <table width="977px" align="center" cellpadding="0" cellspacing="0" id="Table2" border="0" style="margin:0px">
        <tr>
            <td style='background-image: url(images/backGroundPie.gif); background-repeat:repeat-x;' colspan="6" align="center">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr style="background-image: url(images/line_03.jpg);">
                                <td width="50%" align="left" valign="bottom">
                                    <p class="bottom_tab3">
                                        Reminder & Other Services
                                    </p>
                                </td>
                                <td>
                            
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="2" align=center>
                                    
                                    <asp:Label ID="labelMain" runat="server" Text=""></asp:Label>
                                    
                                    
                                    <asp:Panel ID="panelSignup" runat="server" style="background-color:#E0E0FF; width:600px; font-size:12px; padding:12px;">
                                        <h2 style="color:#333355">Signup Now For A 30 Days Free Trial</h2>
                                        <table width=100% cellpadding=5; cellspacing=5; >
                                        <tr>
                                            <th style="text-align:right; color:#333355; font-size:x-small">Please Confirm<br />Your Email</th>
                                            <td>
                                                <asp:TextBox ID="email" runat="server" Width="301px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>&nbsp;</th>
                                            <td>
                                                <asp:Button ID="pbSignup" runat="server" Text="Sign me up" 
                                                    onclick="pbSignup_Click" />
                                                <br />
                                                <asp:Label ID="labelError" runat="server" Font-Bold="True" ForeColor="#996600"></asp:Label>
                                                <br />
                                                <small>
                                                We will contact you soon to setup your reminder account.
                                                </small>
                                            </td>
                                        </tr>
                                        </table>
                                    
                                    </asp:Panel>
                                    
                                    
                                </td>
                            </tr>
                        </tbody>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td width="80%" align="center" valign="top">
        </tr>
        
        <tr background="images/backGroundPie.gif">
            <td colspan="2" align="left">
            </td>
        </tr>
    </table>
    <center>
    <br /><br />
    <img src="images/PowerByMobile.gif" />
    </center>
    <asp:Label ID="DB" runat="server" ForeColor="White"></asp:Label>
    </form>
    
</body>
</html>
