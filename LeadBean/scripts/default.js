$(document).ready(function () {

    if (location.href.indexOf("leadbean=1") != -1) {
        $(".buttonMoveTo").hide()
        $(".buttonDelete").hide()
        $(".buttonRestore").show()
    }
    else {
        $(".buttonMoveTo").show()
        $(".buttonDelete").show()
        $(".buttonRestore").hide()
    }

    $("#confirmMoveTo").dialog({
        autoOpen: false,
        resizable: false,
        width: 400,
        modal: true,
        buttons: {
            "Move": function () {
                $(".btnMoveToCampaign").click();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
    $("#confirmDelete").dialog({
        autoOpen: false,
        resizable: false,
        width: 400,
        modal: true,
        buttons: {
            "Delete": function () {
                $(".btnDelete").click();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $('.buttonMoveTo').bind('click', function (e) {
        $('.hiddenCampaignList').click();
    });

    $('.buttonDelete').bind('click', function (e) {
        $("#confirmDelete").dialog("open");
    });



    $(".campaignItem").bind('click', function (e) {
        $(".selectedCampaign").text($(this).text());
        $("#confirmMoveTo").dialog("open");
    });
});