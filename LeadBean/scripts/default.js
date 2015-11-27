$(document).ready(function () {
    $("#confirmMoveTo").dialog({
        autoOpen:false,
        resizable: false,
        width:400,
        modal: true,
        buttons: {
            "Move": function () {
                $(".btnMoveToCampaign").click();
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $('.btnMoveTo').bind('click', function (e) {       
        $('.hiddenCampaignList').click();
    });

    $(".campaignItem").bind('click', function (e) {
        $(".selectedCampaign").text($(this).text());
        $("#confirmMoveTo").dialog("open");
    });
});