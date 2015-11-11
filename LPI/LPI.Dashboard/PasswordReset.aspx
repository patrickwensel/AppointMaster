<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="dashboard.PasswordReset" %>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Dash Board</title>
    <link href="style.css" rel="stylesheet" type="text/css">
    <link href="css/general.css" rel="stylesheet" type="text/css">

    <script type="text/javascript">
function load(){
    var ctrl = document.getElementById("Email");
    if (ctrl) {
        ctrl.focus();
    }
    var ctrl2 = document.getElementById("password1");
    if (ctrl2) {
            ctrl2.focus();
    }
    
}
    </script>


</head>
<body onload="load()">
    <form id="Form1" method="post" runat="server">
    
    <table width="977px" align="center" cellpadding="0" cellspacing="0" id="Table1" border="0">
            <tr>
                <td style="background-color: #C9E4F5;" valign="top" width="50%">
                    <table width="100%">
                        <tbody>
                            <tr>
                                <td width="30%">
                                    <img src="images/logoPROI.gif" style="">
                                </td>
                                <td align="left">
                                    <span style="font-family: Arial Black; font-size: 18px; color: #355010;">
                                    Password Recovery</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        <tr>
            <td style='background-image: url(images/backGroundPie.gif); height:500px; vertical-align:middle' colspan="6" align="center" >
            <table width=100% cellpadding=0 cellspacing=0>
            <tr>
            <td width=33% align=left style="font-family:Arial; padding:10px; color:#443399;">
                <h2><asp:Label ID="mainMessage" runat="server" Text=""></asp:Label></h2><br /><br /><br />
            </td>
            <td>
                <asp:Panel ID="PanelReset" runat="server">
                    
                    <table class="LeadTable" style="background-color: #C9E4F5;" width="400px;">
                        <tr>
                            <td colspan=2 align=right style=" text-align:right; padding:4px; padding-top:15px; padding-bottom:15px; font-size:small;">
                                Please enter you new password
                            </td>
                        </tr>
                        <tr>
                            <th>
                                New Password
                            </th>
                            <td>
                                <asp:TextBox ID="password1" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Re-Enter Password
                            </th>
                            <td>
                                <asp:TextBox ID="password2" runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="pbRest" runat="server" Text="Reset Password" 
                                    onclick="pbRest_Click" />
                                <br />
                                <br />
                                <asp:Label ID="errorReset" runat="server" CssClass="error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan=2>
                            <a href="login.aspx">< Return to Login Page</a>                    
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelForgot" runat="server">
                    <table class="LeadTable" style="background-color: #C9E4F5;" width="400px;">
                        <tr>
                            <td colspan=2 style=" padding:10px">
                                Please provide the email associated with your user.<br />
                                If this email matches a user account, a recovery email will be sent with the relevant instructions to reset your password.
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Email
                            </th>
                            <td>
                                <asp:TextBox ID="Email" runat="server" Width="208px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="pbSend" runat="server" Text="Send Recovery Email" 
                                    onclick="pbSend_Click" />
                                <br />
                                <br />
                                <asp:Label ID="ErrorSend" runat="server" CssClass="error"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan=2>
                            <a href="login.aspx">< Return to Login Page</a>                    
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                
</td>
            </tr>
            
            </table>
        
            </td>
        </tr>
        </table>
        
    <asp:Label ID="DB" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="ID" runat="server" Visible="False"></asp:Label>
        
    </form>
</body>
</html>
