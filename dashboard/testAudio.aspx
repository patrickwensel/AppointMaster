<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testAudio.aspx.cs" Inherits="dashboard.testAudio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <script type="text/javascript" src="sound/jquery.min.js"></script>
  <script type="text/javascript" src="sound/jquery.jplayer.min.js"></script>
  <script type="text/javascript" src="sound/audio.js"></script>
  <script type="text/javascript" src="js/AjaxCalls.js"></script>
<script type="text/javascript">
function load(){
	downLoadSimpleHTML('http://localhost/dashboard/testAjax.htm');
	createPlayer();
}
  
</script>
  
</head>
<body onload="load();">
    <form id="form1" runat="server">
    <div>
  <div id="jquery_jplayer_1" class="jp-jplayer"></div>
  <div id="jp_container_1" class="jp-audio">

<span id="target"></span>
    
    </div>
    </form>
</body>
</html>
