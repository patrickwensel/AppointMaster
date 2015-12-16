var lastIdPlaying = '';
var lastLoadedUrl = '';


function stop(ID) {
    if (ID!=''){
        var ctrlPlay = document.getElementById('play_' + ID);
        var ctrlPause = document.getElementById('pause_' + ID);
        var ctrlRestart = document.getElementById('restart_' + ID);
        if (ctrlPlay) {
            if (ctrlPause) {
                if (ctrlRestart) {
                    $("#jquery_jplayer_1").jPlayer("stop"); //just in case something is playing
                    ctrlPlay.style.display = "";
                    ctrlPause.style.display = "none";
                    ctrlRestart.style.display = "none";
                }
            }
        }
    }
}

function restart() {
    $("#jquery_jplayer_1").jPlayer("setMedia", { mp3: lastLoadedUrl }).jPlayer("play");
}

function pause() {
    if (lastIdPlaying != "") {
        var ctrlPlay = document.getElementById('play_' + lastIdPlaying);
        var ctrlPause = document.getElementById('pause_' + lastIdPlaying);
        var ctrlRestart = document.getElementById('restart_' + lastIdPlaying);
        if (ctrlPlay) {
            if (ctrlPause) {
                if (ctrlRestart) {
                    $("#jquery_jplayer_1").jPlayer("pause");
                    ctrlPlay.style.display = "";
                    ctrlPause.style.display = "none";
                    ctrlRestart.style.display = "";
                }
            }
        }
    }
}

function play(ID, URL) {
    var ctrlPlay = document.getElementById('play_' + ID);
    var ctrlPause = document.getElementById('pause_' + ID);
    var ctrlRestart = document.getElementById('restart_' + ID);
    if (ctrlRestart) {
        if (ctrlPlay) {
            if (ctrlPause) {
                var sameURL = (lastLoadedUrl == URL);
                //We start playing
                if (sameURL) {
                    $("#jquery_jplayer_1").jPlayer("play");
                    //alert('PLAY');
                } else {
                    if (lastIdPlaying != '') {
                        stop(lastIdPlaying);
                    }
                    $("#jquery_jplayer_1").jPlayer("setMedia", { mp3: URL }).jPlayer("play");
                    //alert('PLAY ' + ID + ' ' + URL + ' sameURL=' + sameURL);
                }
                ctrlPlay.style.display = "none";
                ctrlPause.style.display = "";
                ctrlRestart.style.display = "";
                lastIdPlaying = ID;
                lastLoadedUrl = URL;
            } else {
                alert('pause control missing for id ' + ID);
            }
        } else {
            alert('play control missing for id ' + ID);
        }
    } else {
        alert('restart control missing for id ' + ID);
    }
}



function playPause(ID, URL) {
    var ctrlPlay = document.getElementById('play_' + ID);
    var ctrlPause = document.getElementById('pause_' + ID);
    var ctrlRestart = document.getElementById('restart_' + ID);
    if (ctrlRestart) {
        if (ctrlPlay) {
            if (ctrlPause) {
                var sameURL = (lastLoadedUrl == URL);
                if (ctrlPlay.style.display == "none") {
                    //we are playing we need to pauset
                    if (sameURL)
                        $("#jquery_jplayer_1").jPlayer("pause");
                    else
                        $("#jquery_jplayer_1").jPlayer("setMedia", { mp3: URL }).jPlayer("pause");

                    ctrlPlay.style.display = "";
                    ctrlPause.style.display = "none";
                    ctrlRestart.style.display = "none";
                    lastIdPlaying = '';
                } else {
                    //We start playing
                    if (sameURL)
                        $("#jquery_jplayer_1").jPlayer("play");
                    else {
                        if (lastIdPlaying != '') {
                            stop(lastIdPlaying);
                        }
                        $("#jquery_jplayer_1").jPlayer("setMedia", { mp3: URL }).jPlayer("play");
                    }
                    ctrlPlay.style.display = "none";
                    ctrlPause.style.display = "";
                    ctrlRestart.style.display = "";
                    lastIdPlaying = ID;
                }
                lastLoadedUrl = URL;
            } else {
                alert('pause control missing for id ' + ID);
            }
        } else {
            alert('play control missing for id ' + ID);
        }
    } else {
        alert('restart control missing for id ' + ID);
    }
}


   function playURL(URL){
       alert('playing ' + URL);
       $("#jquery_jplayer_1").jPlayer("setMedia", { mp3: URL }).jPlayer("play");
    var ctrl=document.getElementById("soundPlayer");
	if (ctrl){
		ctrl.style.display='';
	}
   }
   function closePlayer(){
	$("#jquery_jplayer_1").jPlayer("stop");
	var ctrl=document.getElementById("soundPlayer");
	if (ctrl){
		ctrl.style.display='none';
	}
   }
	

    /*$(document).ready(function(){
      $("#jquery_jplayer_1").jPlayer({
        ready: function() {
          
	$(this).jPlayer("setMedia", {
            mp3: "http://www.jplayer.org/audio/mp3/Miaow-snip-Stirring-of-a-fool.mp3"
          });
          var click = document.ontouchstart === undefined ? 'click' : 'touchstart';
          var kickoff = function () {
            
            document.documentElement.removeEventListener(click, kickoff, true);
          };
          document.documentElement.addEventListener(click, kickoff, true);
        },
        loop: false,
        swfPath: "sound"
      });

      $("#jquery_jplayer_1").jPlayer("stop");
      loadAM();
    });*/

    function createPlayer(){
          $("#jquery_jplayer_1").jPlayer({
            ready: function() {
              
	    $(this).jPlayer("setMedia", {
                mp3: "http://www.jplayer.org/audio/mp3/Miaow-snip-Stirring-of-a-fool.mp3"
              });
              var click = document.ontouchstart === undefined ? 'click' : 'touchstart';
              var kickoff = function () {
                
                document.documentElement.removeEventListener(click, kickoff, true);
              };
              document.documentElement.addEventListener(click, kickoff, true);
            },
            loop: false,
            swfPath: "Sound"
          });

          $("#jquery_jplayer_1").jPlayer("stop");
    }

