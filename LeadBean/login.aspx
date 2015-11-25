<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="LeadBean.login" %>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>Lead Management Board</title>
	<link rel="stylesheet" href="css/general.css">

    <script type="text/javascript">
        function load() {
            var ctrl = document.getElementById("userId");
            if (ctrl) {
                ctrl.focus();
            }
            var ctrl = document.getElementById("labelURL");
            if (ctrl) {
                ctrl.style.display="none";
            }

            
            
        }
    </script>


</head>
<body onload="load();">
    <form id="Form1" method="post" runat="server">
    

<div class="center-column" >
<img src="images/logo.png"   style=" margin:20px;" />    

<h2 style=" margin:20px; float:right;color: rgb(47,87,122)">Lead Management</h2>
    
    <table style=" border:solid 1px #dddddd; width:600px; height:400px; position:absolute; left:50%;top:50%; margin: -200px  0 0 -300px;   ">
            <td align=left style="font-family:Arial; padding:10px; color:#443399;" colspan=2>
                <asp:Label ID="mainMessage" runat="server" Text=""></asp:Label>
            </td>
    <tr>
        <th style ="vertical-align:middle">
            User Name
        </th>
        <td style ="vertical-align:middle">
            <asp:TextBox ID="userId" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th style ="vertical-align:middle">
            User Password
        </th>
        <td style ="vertical-align:middle">
            <asp:TextBox ID="password" runat="server" TextMode="password"></asp:TextBox>
        </td>
    </tr>
    <tr style=" padding:30px;">
        <td colspan=2>
            <asp:Button ID="Button1" runat="server" Text="Login" onclick="PbLogin_Click" />
        </td>
    </tr>
    <tr style=" padding:30px;">
        <td colspan=2>
            <asp:Label ID="error" runat="server" Text="" ForeColor="Red"></asp:Label>
        </td>
     </tr>   
                    <tr style=" padding:20px;">
                        <td colspan=2><a href="PasswordReset.aspx">Forgot your password?</a>
                        <asp:Label ID="labelURL" runat="server" style="display:none"></asp:Label>
                        
                        </td>
                        
                    </tr>
</table>
    </div>

    
    
    
    
    
    
</div>    
    
    </form>
</body>
</html>
