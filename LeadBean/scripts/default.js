$(document).ready(function () {
    var headerTable = $($('.rgMasterTable').get()[0]);
    headerTable.css("table-layout", "fixed");
    headerTable.css("width", "1850px");

    var headerTableCols = headerTable.find('col').get();
    $(headerTableCols[0]).css("width", "30px");
    $(headerTableCols[1]).css("width", "70px");
    $(headerTableCols[2]).css("width", "180px");
    $(headerTableCols[3]).css("width", "110px");
    $(headerTableCols[4]).css("width", "170px");
    $(headerTableCols[5]).css("width", "110px");
    $(headerTableCols[6]).css("width", "120px");
    $(headerTableCols[7]).css("width", "120px");
    $(headerTableCols[8]).css("width", "200px");
    $(headerTableCols[9]).css("width", "200px");
    $(headerTableCols[10]).css("width", "170px");
    $(headerTableCols[11]).css("width", "120px");
    $(headerTableCols[12]).css("width", "120px");
    $(headerTableCols[13]).css("width", "100px");


    var contentTable = $($('.rgMasterTable').get()[1]);
    contentTable.css("table-layout", "fixed");
    contentTable.css("width", headerTable.width());

    var contentTableCols = contentTable.find('col').get();

    for (var i = 0; i < contentTableCols.length; i++) {
        $(contentTableCols[i]).css("width", $(headerTableCols[i]).css("width"));
    }

    //alignment of filter rows
    var filterRows = $('.rgFilterRow td').get();
    for (var i = 0; i < filterRows.length; i++) {
        var input = $($(filterRows[i]).find('span'));
        if (input.length > 0) {
            input.css('width', $(filterRows[i]).width() - 24);
        }
        else {
            input = $($(filterRows[i]).find('input'));
            input.css('width', $(filterRows[i]).width() - 24);
        }
    }

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
    $("#confirmDeleteLeads").dialog({
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

    $("#confirmDeleteCampaign").dialog({
        autoOpen: false,
        resizable: false,
        width: 400,
        modal: true,
        buttons: {
            "Delete": function () {
                if ($("#ddlDeleteCampaign_ClientState").val() && JSON.parse($("#ddlDeleteCampaign_ClientState").val()).selectedValue) {
                    var value = JSON.parse($("#ddlDeleteCampaign_ClientState").val()).selectedValue;
                    console.log(value);
                    __doPostBack('btnDeleteCampaign', value);
                }
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
        $("#confirmDeleteLeads").dialog("open");
    });

    $(".moveCampaignItem").bind('click', function (e) {
        $(".selectedCampaign").text($(this).text());
        $("#confirmMoveTo").dialog("open");
    });

    $(".btnDeleteCampaign").bind('click', function (e) {
        if ($("#ddlDeleteCampaign_ClientState").val() && JSON.parse($("#ddlDeleteCampaign_ClientState").val()).selectedValue) {
            $(".selectedCampaign").text($("#ddlDeleteCampaign").find(".rddlFakeInput").text());
            $("#confirmDeleteCampaign").dialog("open");
        }
        else {
            $("#ddlDeleteCampaign").click()
        }
        return false;
    });
});