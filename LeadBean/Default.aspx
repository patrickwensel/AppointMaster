<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LeadBean._Default" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Lead Management Board</title>
	<link rel="stylesheet" href="css/general.css">
</head>
<body>
    <div style=" width:100%">
    <form id="form1" runat="server">
    <h1>
        <asp:Label ID="Title" runat="server" Text="Label"></asp:Label>
    </h1>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>
        </Scripts>
    </telerik:RadScriptManager>
                    
    <asp:SqlDataSource ID="Leads" runat="server" 
        SelectCommand="select campaignID as 'Camp', inComingNumber as 'Incoming', timeStamp as 'Time Stamp', durationMinutes as 'Duration', firstName, lastName,email,fileURL, birthday as 'DOB',PrimaryPhone as 'Prim. Phone', alterPrimaryPhone as 'alt. Prim. Phone', ID from @TABLE where DB= @DB ">
        <SelectParameters>
            <asp:Parameter Name="DB" />
        </SelectParameters>
        <SelectParameters>
            <asp:Parameter Name="TABLE" />
        </SelectParameters>
    </asp:SqlDataSource>

    <telerik:RadGrid ID="RadGrid2" runat="server" AllowFilteringByColumn="True" 
        AllowPaging="True" AllowSorting="True" CellSpacing="0"   Height="800px" PageSize="150" 
        DataSourceID="Leads" GridLines="None" onload="RadGrid2_Load" 
                    AutoGenerateColumns="False">
        <PagerStyle PageSizeControlType="RadComboBox" />
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
        <ClientSettings>
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
        </ClientSettings>
        <MasterTableView DataSourceID="Leads" AutoGenerateColumns="True">
            <CommandItemSettings ExportToPdfText="Export to PDF" />
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                Visible="True">
                <HeaderStyle Width="20px" />
            </RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                Visible="True">
                <HeaderStyle Width="20px" />
            </ExpandCollapseColumn>
            <EditFormSettings>
                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                </EditColumn>
            </EditFormSettings>
            <PagerStyle PageSizeControlType="RadComboBox" />
            <Columns>
                <telerik:GridHyperLinkColumn AllowSorting="true" 
            FilterControlAltText="Filter column column" DataNavigateUrlFormatString="default.aspx?DELID={0}"  DataNavigateUrlFields="ID"
            HeaderText="Action" DataTextField="ID" DataTextFormatString="<b>Delete-{0}</b>" UniqueName="ID">
        </telerik:GridHyperLinkColumn>
                        
                
            </Columns>
        </MasterTableView>
        
        
        
        
        
        
        
        
        
        
    </telerik:RadGrid>
    


    
    </div>
    <asp:Label ID="LabelError" runat="server" ForeColor="#996600"></asp:Label>
    <asp:Label ID="usage" runat="server" Visible="False"></asp:Label>
    <br />
    <br />
        </form>
    
&nbsp;<asp:HyperLink ID="LinkSwitch" runat="server" 
        NavigateUrl="default.aspx?leadbean=1">Trash</asp:HyperLink>
    
</body>
</html>
