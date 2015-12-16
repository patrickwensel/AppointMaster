<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="confirm.aspx.cs" Inherits="LeadBean.confirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Lead Management Board</title>
	<link rel="stylesheet" href="css/general.css">
	<style>
	    table
	    {
	    	background-color:#888888;
	    	width:800px;
	    	border-spacing:1px;
	    }
	    table td
	    {
	    	background-color:White;
	    	padding:3px;
	    	font-weight:bold;
	    	margin:1px;
	    }
	    table th
	    {
	    	margin:1px;
	    	padding:3px;
	    	background-color:#eeeeee;
	    	text-align:left;
	    	font-weight:normal;
	    	width:30%;
	    }
	
	
	</style>
</head>
<body>
    <form id="form1" runat="server">
    <div style=" width:900px; margin-left:auto; margin-right:auto">
    <h1><asp:Label ID="Title" runat="server" Text="Label"></asp:Label>&nbsp;<asp:Label 
            ID="ID" runat="server" Text="Label"></asp:Label>
        </h1>
    
    <br /><br />
    <asp:Label ID="labelMain" runat="server" Text="Label"></asp:Label>
    
        <br />
        <br />
        <asp:Panel ID="PanelAltermativePhone" runat="server">
        <table>
            <tr>
                <th>Alternative Primary Phone</th>
                <td>
                    <asp:TextBox ID="NewPrimaryPhone" runat="server" Width="199px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Alternative Name
                </th>
                <td>
                    <asp:TextBox ID="AlterName" runat="server" Width="299px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>
                    Alternative First Name
                </th>
                <td>
                    <asp:TextBox ID="AlterFirstName" runat="server" Width="299px"></asp:TextBox>
                </td>
            </tr>
        </table>
        
        
&nbsp;<asp:Button ID="pb_save_primary" runat="server" onclick="pb_save_primary_Click" 
            Text="Save" />
        </asp:Panel>
        <br />
        <br />
        <asp:Button ID="pbConfirmDelete" runat="server" Text="Confirm Delete" 
            onclick="pbConfirmDelete_Click" />
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="pbConfirmRestore" runat="server" Text="Confirm Restore" 
            onclick="pbConfirmRestore_Click" />
&nbsp;<asp:Button ID="pbCancel" runat="server" Text="Cancel" style=" float:right" 
            onclick="pbCancel_Click" />
    
        <asp:Label ID="LabelError" runat="server" ForeColor="#996600"></asp:Label>
    
    </div>
    
    </form>
</body>
</html>
