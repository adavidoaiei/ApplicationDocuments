$(document).ready(function () {
    $.get("Templates/" + $("#select option:selected").val() + "/" + $("#select option:selected").val() + ".html", function (data) {
        $('#document').html(String.format(data, "Templates/" + $("#select option:selected").val() + "/" + $("#select option:selected").val() + ".png"));
    });
    //$("#document").load("Templates/" + $("#select option:selected").val() + "/" + $("#select option:selected").val() + ".html");
});

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

function GeneratePDF() {
    var inputs = $("#document :input");
    
    var requestData = {};
    for (var i = 0; i < inputs.length; i++)
        requestData[inputs[i].id] = inputs[i].value;

    requestData["document"] = $("#select option:selected").val();

    $.ajax({
        url: 'CerereInmatriculare/Completeaza',
        type: 'POST',
        data: JSON.stringify(requestData),
        dataType: 'json',
        //contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
            //debugger;
            //alert('Error: ' + xhr.statusText);
            if (xhr.status == 200)
            {
                var pdfPath = xhr.responseText;
                if (pdfPath.indexOf('<!--') == -1)
                    window.location = pdfPath;
                else
                    window.location = pdfPath.substring(0, pdfPath.indexOf('<!--'));
            }
        },
        success: function (result) {
            //debugger;
                
        },
        async: false,
        processData: false
    });
}

function showPopup(val) {
    $("#optiuni").val(val);
    $("#dialog").dialog({
        width: 550,
        modal: true
    });
}

function SendMesaj() {
    var requestData = {
        Tip: $.trim($("#optiuni option:selected").text()),
        Descriere: $.trim($("#descriere").val()),        
    };

    $.ajax({
        url: 'CerereInmatriculare/SendMesaj',
        type: 'POST',
        data: JSON.stringify(requestData),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
            //debugger;
            //alert('Error: ' + xhr.statusText);
            $("#dialog").dialog("close");
        },
        success: function (result) {
            //debugger;
            $("#dialog").dialog("close");
        },
        async: false,
        processData: false
    });
}

function change() {
    $("#title").text($("#select option:selected").text());
    $.get("Templates/" + $("#select option:selected").val() + "/" + $("#select option:selected").val() + ".html", function (data) {
        $('#document').html(String.format(data, "Templates/" + $("#select option:selected").val() + "/" + $("#select option:selected").val() + ".png"));
    });
}

