﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="dashboard.Settings" %>


<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Dash Board</title>
    <style type="text/css" media="screen">
        #chartdiv
        {
            visibility: hidden;
        }
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 316px;
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

<body onload="load()">
    <form id="Form1" method="post" runat="server">
    <!-- chart is placed in this div. if you have more than one chart on a page, give unique id for each div -->
    <table width="977px" align="center" cellpadding="0" cellspacing="0" id="Table1" border="0">
<asp:Panel ID="PanelHeader" runat="server">
            <tr>
                <td style="background-color: #C9E4F5;" valign="top" width="50%">
                                                        
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
                <td style="background-color: #C9E4F5; padding-right: 1px" align="left" valign="bottom" nowrap="">
                    <div class="animatedtabs">
                        <ul>
                            <li>
                                <li>
                                <asp:Label ID="linkLead" runat="server" Text=""></asp:Label> </li>
                            <li><a href="reviews.aspx" onmouseover="window.status='Check your reviews!'; return true;" onmouseout="window.status='';"><span>
                                Reviews            <li><a href="AM.aspx"><span>Reminders</span></a></li>
                            <!-- li><a href="localListings.aspx"><span>Local Listings</span></a></li -->
                            <li><a href="login.aspx?logout=1"><span>Log out</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
</asp:Panel>
        <tr>
            <td style='background-image: url(images/backGroundPie.gif); background-repeat:repeat-x;' colspan="6" align="center">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tbody>
                            <tr style="background-image: url(images/line_03.jpg);">
                                <td width="50%" align="left" valign="bottom">
                                    <p class="bottom_tab3">
                                        Settings
                                    </p>
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td height="10" colspan="2" align=center>
                                    
                                    <br />
                                    <table class="style1">
                                        <tr>
                                            <td class="style2">
                                                Account Notification Email</td>
                                            <td>
                                                <asp:TextBox ID="notificationEmail" runat="server" Width="272px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Display Duplicates</td>
                                            <td>
                                                <asp:CheckBox ID="cbDisplayDuplicates" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Display Budget</td>
                                            <td>
                                                <asp:CheckBox ID="cbDisplayBudget" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Excluded Codes from matching (comma separeted)</td>
                                            <td>
                                                <asp:TextBox ID="excludedCodes" runat="server" Width="550px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                &nbsp;</td>
                                            <td>
                                                <asp:Button ID="pbSave" runat="server" onclick="pbSave_Click" Text="Save" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="pbCancel" runat="server" onclick="pbCancel_Click" 
                                                    Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    
                                    
                                   
                                    
                                    <asp:Label ID="labelMain" runat="server" Text=""></asp:Label>
                                    
                                    
                                    <br />
                                    
                                    
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
    <asp:Label ID="DB" runat="server" ForeColor="White"></asp:Label>
    </form>
</body>
</html>
