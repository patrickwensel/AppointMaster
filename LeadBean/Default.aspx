<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LeadBean._Default" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Lead Management Board</title>
    <link rel="stylesheet" href="css/general.css">
    <link rel="stylesheet" href="css/font-awesome.min.css">
    <link rel="stylesheet" href="css/jquery-ui.min.css">
    <script src="scripts/jquery.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="scripts/default.js" type="text/javascript"></script>
</head>
<body>
    <div style="width: 100%">
        <form id="form1" runat="server">
            <h1>
                <asp:Label ID="Title" runat="server" Text="Label"></asp:Label>
            </h1>
            <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableEmbeddedjQuery="false">
                <Scripts>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                    <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
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

            <asp:SqlDataSource ID="Campaign" runat="server"
                SelectCommand="SELECT ID as 'Id', name as 'Name' FROM CAMPAIGN WHERE DB = @DB">
                <SelectParameters>
                    <asp:Parameter Name="DB" />
                </SelectParameters>
            </asp:SqlDataSource>

            <telerik:RadGrid ID="RadGrid2" runat="server" ClientIDMode="Static" AllowFilteringByColumn="True"
                AllowPaging="True" AllowSorting="True" Height="800px" PageSize="150"
                DataSourceID="Leads" OnLoad="RadGrid2_Load"
                AutoGenerateColumns="False" GroupPanelPosition="Top">
                <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>
                <ClientSettings>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" AllowResizeToFit="true" />
                </ClientSettings>
                <MasterTableView DataSourceID="Leads" AutoGenerateColumns="True">
                    <Columns>
                        <telerik:GridClientSelectColumn FilterControlAltText="Filter SelectColumn column" UniqueName="SelectColumn" Resizable="False">
                            <FooterStyle Width="35px" />
                            <HeaderStyle Width="35px" />
                            <ItemStyle Width="35px" />
                        </telerik:GridClientSelectColumn>
                        <telerik:GridHyperLinkColumn AllowSorting="true"
                            FilterControlAltText="Filter column column" DataNavigateUrlFormatString="default.aspx?DELID={0}" DataNavigateUrlFields="ID"
                            HeaderText="Action" DataTextField="ID" DataTextFormatString="&lt;div class='actionButton' title='Edit-{0}'&gt;&lt;i class='fa fa-pencil actionIcon'&gt; &lt;/i&gt;&lt;/div&gt;" UniqueName="Action">
                            <FilterTemplate>
                                <div class="actionControls">
                                    <div class="hiddenActionControls">
                                        <telerik:RadDropDownList ID="ddlCampaign" CssClass="hiddenCampaignList" runat="server" DataSourceID="Campaign" DataTextField="Name" DataValueField="Id" DataTextFormatString='<div class="campaignItem">{0}</div>'></telerik:RadDropDownList>
                                        <asp:Button ID="btnMoveToCampaign" CssClass="btnMoveToCampaign" runat="server" Text="Move to campaign" OnClick="btnMoveToCampaign_Click" />
                                        <asp:Button ID="btnDelete" CssClass="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                                    </div>
                                    <div class="actionButton buttonMoveTo" title="Move selected to">
                                        <div>
                                            <i class="fa fa-folder actionIcon"></i>
                                            <i class="fa fa-caret-down actionIcon"></i>
                                        </div>
                                    </div>
                                    <div class="actionButton buttonDelete" title="Delete selected">
                                        <i class="fa fa-trash-o actionIcon"></i>
                                    </div>

                                    <div class="actionButton buttonRestore" title="Restore selected">
                                        <asp:Button ID="btnRestore" CssClass="btnRestore" runat="server" Text="Restore" OnClick="btnRestore_Click" />
                                    </div>
                                </div>
                            </FilterTemplate>
                        </telerik:GridHyperLinkColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <asp:Label ID="LabelError" runat="server" ForeColor="#996600"></asp:Label>
            <asp:Label ID="usage" runat="server" Visible="False"></asp:Label>
            <br />
            <br />
            <div id="confirmMoveTo" style="margin: 10px auto; text-align: center;">
                <div>Are you sure you want to move the selected record to <span style="font-weight: bold;" class="selectedCampaign"></span>Campaign</div>
            </div>
            <div id="confirmDelete" style="margin: 10px auto; text-align: center;">
                <div>Are you sure you want to delete the selected records</div>
            </div>
        </form>
    </div>
    &nbsp;<asp:HyperLink ID="LinkSwitch" runat="server"
        NavigateUrl="default.aspx?leadbean=1">Trash</asp:HyperLink>
</body>
</html>
