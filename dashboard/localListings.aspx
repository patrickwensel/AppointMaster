<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="localListings.aspx.cs" Inherits="dashboard.localListings" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Dash Board</title>
    <style type="text/css" media="screen">
        #chartdiv
        {
            visibility: hidden;
        }
    </style>
    <link href="style.css" rel="stylesheet" type="text/css">
    <link href="css/general.css" rel="stylesheet" type="text/css">

    <script type="text/javascript">
function load(){
}
    </script>


</head>
<body onload="load()">
    <center>
    <form id="Form1" method="post" runat="server">
    <!-- chart is placed in this div. if you have more than one chart on a page, give unique id for each div -->
    <table align="center" cellpadding="0" cellspacing="0" id="Table1" border="0" style="margin:0px;height:100%">
            <tr>
                <td style="background-color: #C9E4F5; width:500px;" valign="top" width="500px">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td width="30%">
                                    <img src="images/logoPROI.gif" style="">
                                </td>
                                <td align="left">
                                    <span style="font-family: Arial Black; font-size: 18px; color: #355010;"><asp:Label ID="AccountName" runat="server" Text="Label"></asp:Label></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
                <td style="background-color: #C9E4F5; width:500px; " align="left" valign="bottom" nowrap=""  width="500px">
                    <div class="animatedtabs">
                        <ul>
                            <li><a href="dashboard.aspx?p=4"><span>Lead Generation</span></a></li>
                            <li><a href="reviews.aspx"><span>Reviews</span></a></li>
                            <li><a href="AM.aspx"><span>Reminders</span></a></li>
                            <!-- li  class="selected"><a href="#"><span>Local Listings</span></a></li -->
                            <li><a href="login.aspx?logout=1"><span>Log out</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        <tr>
            <td style='background-image: url(images/backGroundPie.gif); background-repeat:repeat-x' colspan="2" align="center" style="height:100%">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr style="background-image: url(images/line_03.jpg)">
                                <td width="50%" align="left" valign="bottom">
                                    <p class="bottom_tab3">
                                        Local Listings
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td height="10" colspan="2" background="images/line_06.jpg">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    
            <asp:Label ID="labelIframe" runat="server"></asp:Label>        
            </td>
        </tr>
        <tr>
    </table>
    <asp:Label ID="DB" runat="server" ForeColor="White"></asp:Label>

    </form>
    

    
</body>
</html>
