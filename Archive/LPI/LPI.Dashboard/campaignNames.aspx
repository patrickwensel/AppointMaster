<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="campaignNames.aspx.cs" Inherits="dashboard.campaignNames" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css" media="screen">
        #rbRoiFormat_0
        {
        	float:left;
        }
        #rbRoiFormat_1
        {
        	float:left;
        }
        #cbAutoHide
        {
        	float:left;
        }
        .listTable
        {
        	width:80%;
        	background-color:#eeeeee;
        }
        .listTable th
        {
        	font-weight:bold;
        	padding:5px;
        }
        .listTable td
        {
        	background-color:White;
        	margin:1px;
        }
        .listTable td.line
        {
        	background-color:White;
        	margin:1px;
        	border:solid 1px #eeeeee;
        }
        .listTable tr.invisibleLine
        {
        	color:#AAAAAA;
        	border:solid 1px #eeeeee;
        	background-color:Red;
        	font-style:italic;
        }
        
    </style>
</head>
<body style="font-family:Arial">
    <form id="form1" runat="server">


<asp:Label ID="LabelHeader" runat="server" Text="Label"></asp:Label>
&nbsp;<div style="padding:20px;">

    <h4>Setting Campaigns</h4>
            <br />
        <asp:Label ID="labelmain" runat="server"></asp:Label>
        <asp:Panel ID="panelCampaign" runat="server">
            
            <table cellspacing=10px; cellpadding=20>
                <tr>
                    <th>Id</th>
                    <td><asp:Label ID="ID" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Phone</th>
                    <td><asp:Label ID="Phone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <th>Name</th>
                    <td>
                        <asp:TextBox ID="name" runat="server" Width="308px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>Monthly Budget</th>
                    <td>
                        <asp:TextBox ID="budget" runat="server" Width="103px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th>
                        Visible</th>
                    <td>
                        <asp:CheckBox ID="cbVisible" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th colspan=2>
                        <asp:Button ID="pbsave" runat="server" Text="Save" onclick="pbsave_Click" />
&nbsp;
                        <asp:Button ID="pbCancel" runat="server" Text="Cancel" 
                            onclick="pbCancel_Click" />
                    </th>
                </tr>
            </table>
            
        </asp:Panel>

<asp:Panel ID="PanelList" runat="server">
            <h4>Campaigns List Other Options</h4>
            <table class="listTable" style=" width:80%; margin-left:auto; margin-right:auto">
                <tr>
                    <th>Display ROI</th>
                    <td class="line">
                        <asp:CheckBox ID="cbDisplayRoi" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Campaigns w/o Leads</th>
                    <td class="line">
                        <asp:CheckBox ID="cbAutoHide" runat="server" 
                            Text="Hide campaigns without leads in the selected period" />
                    </td>
                </tr>
                
                
            </table>
            <br /><br />
            <asp:Button ID="pbSaveOptions" runat="server" Text="Save Options" onclick="pbSaveOptions_Click" />
            &nbsp;&nbsp;
<asp:Button ID="pbReturn" runat="server" onclick="pbReturn_Click" Text="Back" />
        
</asp:Panel>        
<asp:Label ID="LabelFooter" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
