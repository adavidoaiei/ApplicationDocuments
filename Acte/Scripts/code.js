var mesaj = "Nu sunteti logat. Creativa un cont daca nu aveti unul, este gratuit. Pentru a salva documente aveti nevoie de un cont si sa fiti logat. Puteti printa documente folosind \"Printeaza PDF\" fara a avea cont sau a fi logat.\n\nPentru demo logati-va cu:\nEmail: demo\nParola: demo";

$(document).ready(function () {
    //debugger;
    $("#select option:first-child").attr("selected", true);
    $("#selectSearch option:first-child").attr("selected", true);
    changeDocument();
    changeSearch();
    LoadDocuments();

    if ($("#islogged").val() == "true") {
        ShowPreferences();
    }

    $("#value").bind('keypress', function (e) {
        if (e.which == 13) {
            $("#cauta").trigger("click");
        }
    });

    $("#password").bind('keypress', function (e) {
        if (e.which == 13) {
            $("#logeaza").trigger("click");
        }
    });

    $("#name").bind('keypress', function (e) {
        if (e.which == 13) {
            $("#logeaza").trigger("click");
        }
    });

    $("#select").bind('change', function (e) {
        if ($("#select").val() != $("#selectSearch").val()) {
            $("#selectSearch").val($("#select").val());
            changeDocument();
            changeSearch();
            clearListDocuments();
        }
    });

    $("#selectSearch").bind('change', function (e) {
        if ($("#selectSearch").val() != $("#select").val()) {
            $("#select").val($("#selectSearch").val());
            changeDocument();
            changeSearch();
            clearListDocuments();
        }
    });

});

function clearListDocuments() {
    $("#listdoc").html("");
}

function ShowPreferences() {
    $("#meniu").append('<a href=\"#\" onclick=\"OpenPreferencesWindow();\" class=\"item-menu\">Preferinte</a><span style=\"color: black;\">|</span><a href=\"Facade/Logout\" class=\"item-menu\">Logout</a>');
    if ($("#create_template").length == 0)
        $("#meniu").prepend('<a href=\"#\" id="create_template" onclick=\"CreateTemplate();\" class=\"item-menu\">Creeaza Sablon</a><span style=\"color: black;\">|</span>');
}

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

function CloseLoginFailed() {
    $("#loginFailed").dialog("close");
}

function Login() {
    //debugger;
    var name = $("#name").val();
    var password = $("#password").val();
    var user = {};
    user["UserName"] = name;
    user["Password"] = password;

    $.ajax({
        url: 'Facade/Login',
        type: 'POST',
        data: JSON.stringify(user),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        error: function (xhr) {
            //debugger;
            //alert('Error: ' + xhr.statusText);
            if (xhr.status == 200) {
                if (xhr.responseText == "valid") {
                    $(".username").hide();
                    $("#select option:first-child").attr("selected", true);
                    $("#selectSearch option:first-child").attr("selected", true);
                    ShowPreferences();
                    changeDocument();
                    changeSearch();
                    LoadDocuments();
                    $("#islogged").val("true");
                }
                else {
                    $("#loginFailed").dialog({
                        width: 250,
                        modal: true
                    });
                }
            }
        },
        success: function (result) {
            debugger;

        },
        async: true,
        processData: false
    });
    
}

function LoadDocuments() {
    var id = $("#select").val();
    $.ajax({
        // edit to add steve's suggestion.
        //url: "/ControllerName/ActionName",
        url: 'Facade/LoadDocuments/' + id,
        type: 'POST',
        success: function (data) {
            // your data could be a View or Json or what ever you returned in your action method 
            // parse your data here
            //alert(data);
            //debugger;
            $("#listdoc").html(data);           
        },
        error: function (xhr) {
            debugger;
            alert('Error: ' + xhr.statusText);
        },
        statusCode: {
            404: function (content) { alert('cannot find resource'); },
            505: function (content) { alert('internal server error'); }
        },
        async: true,
        processData: false
    });
}

function GeneratePDF() {
    //debugger;
    var inputs = $("#document :input");
    
    var requestData = {};
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "text")
        {
            requestData[inputs[i].id] = inputs[i].value;
        }
        else if (inputs[i].type == "checkbox") {
            requestData[inputs[i].id] = "checkbox, " + inputs[i].checked;
        }
    }

    requestData["document"] = $("#select option:selected").text().replace(/ /g, "_").toLowerCase();

    try {
        $.ajax({
            url: 'Facade/Printeaza',
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            error: function (result) {
                //debugger;
                //alert('Error: ' + xhr.statusText);
                if (result.status == 200) {
                    window.location = result.responseText;
                }
            },
            success: function (result) {
                debugger;
                window.location = result.responseText;
            },
            async: true,
            processData: false
        });
    }
    catch (err) {
        txt = "There was an error on this page.\n\n";
        txt += "Error description: " + err.message + "\n\n";
        txt += "Click OK to continue.\n\n";
        alert(txt);
    }
    
}

function GenerateDOC() {
    var inputs = $("#document :input");

    var requestData = {};
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "text") {
            requestData[inputs[i].id] = inputs[i].value;
        }
        else if (inputs[i].type == "checkbox") {
            requestData[inputs[i].id] = "checkbox, " + inputs[i].checked;
        }
    }

    requestData["document"] = $("#select option:selected").text().replace(/ /g, "_").toLowerCase();

    try {
        $.ajax({
            url: 'Facade/PrinteazaDOC',
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            error: function (result) {
                //debugger;
                //alert('Error: ' + xhr.statusText);
                if (result.status == 200) {
                    //window.location = result.responseText;
                    $("#downloadDoc").append("<a href=\"" + result.responseText + "\">Download Doc</a>");
                    $("#downloadDoc").dialog({
                        width: 550,
                        modal: true
                    });
                }
            },
            success: function (result) {
                debugger;
                window.location = result.responseText;
            },
            async: true,
            processData: false
        });
    }
    catch (err) {
        txt = "There was an error on this page.\n\n";
        txt += "Error description: " + err.message + "\n\n";
        txt += "Click OK to continue.\n\n";
        alert(txt);
    }
}

function SalveazaDocument() {
    //debugger;    

    var inputs = $("#document :input");

    var requestData = {};
    for (var i = 0; i < inputs.length; i++)
        requestData[inputs[i].id] = inputs[i].value;

    requestData["document"] = $("#select option:selected").val();
    requestData["name_document"] = $("#name_document").val();

    $.ajax({
        url: 'Facade/Salveaza',
        type: 'POST',
        data: JSON.stringify(requestData),
        dataType: 'json',
        //contentType: 'application/json; charset=utf-8',
        error: function (data) {
            //debugger;
            //alert('Error: ' + xhr.statusText);
            if (data.status == 200) {
                if (data != null)
                    $("#listdoc").html(data.responseText)
            }
        },
        success: function (data) {
            //debugger;
            if (data != null)
                $("#listdoc").html(data.responseText)
        },
        async: true,
        processData: false
    });

    $("#dialogSalveazaDocument").dialog("close");
}

function showPopup(val) {
    $("#optiuni").val(val);
    $("#dialog").dialog({
        width: 550,
        modal: true
    });
}

function SalveazaDocumentWindow() {
    if ($("#islogged").val() == "false") {
        alert(mesaj);
        return;
    }

    $("#name_document").val("");
    $("#dialogSalveazaDocument").dialog({
        width: 550,
        modal: true
    });
}

function CloseSD() {
    $("#dialogSalveazaDocument").dialog("close");
}

function SendMesaj() {
    $("#descriere").val("");
    var requestData = {
        Tip: $.trim($("#optiuni option:selected").text()),
        Descriere: $.trim($("#descriere").val()),        
    };

    $.ajax({
        url: 'Facade/SendMesaj',
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
        async: true,
        processData: false
    });
}

    function edit(id, val) {
        //debugger;
        
        $("#listdoc div a").each(function () {
            //debugger;
            $(this).css("background-color", "#f5f5f5");
        });
        val.style.backgroundColor = "#cacaca";
       
        $.ajax({
            // edit to add steve's suggestion.
            //url: "/ControllerName/ActionName",
            url: 'Facade/Edit/' + id,
            type: 'POST', 
            success: function (data) {
                // your data could be a View or Json or what ever you returned in your action method 
                // parse your data here
                //alert(data);
                if (data != "{}") {
                    json = $.parseJSON(data);                    
                    var inputs = $("#document :input");
                    for (var i = 0; i < inputs.length; i++)
                        inputs[i].value = json[inputs[i].id];
                    $("#edit").val(id);
                }
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                505: function (content) { alert('internal server error'); }
            },
            async: true,
            processData: false
        });
    }

    function EditareHandler(id, val) {
        //debugger;
        //if ($("#edit").val() != "")
        //    SalveazaEditare();
        edit(id, val);        
    }

    function SalveazaEditare() {
        if ($("#islogged").val() == "false") {
            alert(mesaj);
            return;
        }
        var id = $("#edit").val();
        if (id == "")
            SalveazaDocumentWindow();
        else {
            //alert("Are id se salveaza editare")

            var inputs = $("#document :input");

            var requestData = {};
            for (var i = 0; i < inputs.length; i++)
                requestData[inputs[i].id] = inputs[i].value;

            requestData["document"] = $("#select option:selected").val();
            requestData["id"] = id;

            $.ajax({
                url: 'Facade/SaveEdit',
                type: 'POST',
                data: JSON.stringify(requestData),
                dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                error: function (xhr) {
                    //debugger;
                    //alert('Error: ' + xhr.statusText);
                    if (xhr.status == 200) {
                    }
                },
                success: function (result) {
                    //debugger;

                },
                async: true,
                processData: false
            });
        }
    }

    function changeSearch() {
        var id = $("#selectSearch").val();

        $.ajax({
            // edit to add steve's suggestion.
            //url: "/ControllerName/ActionName",
            url: 'Facade/GetFieldsDropdownDocument/' + id,
            type: 'POST',
            success: function (data) {
                // your data could be a View or Json or what ever you returned in your action method 
                // parse your data here
                //alert(data);
                //debugger;
                $("#fields").html(data);
                $("#valueContainer").css("display", "block");
            },
            error: function (xhr) {
                //debugger;
                alert('Error: ' + xhr.statusText);                
            },
            statusCode: {
                404: function (content) { alert('cannot find resource'); },
                505: function (content) { alert('internal server error'); }
            },
            async: true,
            processData: false
        });
    }

    function Cauta() {
        //debugger;

        var requestData = {};
        requestData["id_template"] = $("#selectSearch option:selected").val();
        requestData["field"] = $("#fields option:selected").val();
        requestData["value"] = $("#value").val();

        $.ajax({
            url: 'Facade/SearchDocument',
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            error: function (data) {
                //debugger;
                //alert('Error: ' + xhr.statusText);
                if (data.status == 200) {
                    //$("#listdoc").html("<div><a title=\"Click pe nume pentru editare document\" style=\"width: 150px !important; display:block; background-color: white;\" onclick=\"edit('45', this);\" href=\"#\">cf</a></div>");
                    $("#listdoc").html(data.responseText);
                    $("#listdoc div:first a").trigger("click");
                }
            },
            success: function (data) {
                //debugger;
                //$("#listdoc").html("<div><a title=\"Click pe nume pentru editare document\" style=\"width: 150px !important; display:block; background-color: white;\" onclick=\"edit('45', this);\" href=\"#\">cf</a></div>");
                if (data == null)
                    $("#listdoc").html("");
                else
                    $("#listdoc").html(data.responseText);
            },
            async: true,
            processData: false
        });        
    }

    function CreateAccountWindow() {
        $("#container_createaccount").show();
        $("#mesage_createaccount").hide();
        $("#error").html("")
        $("#nume_firma").val("");
        $("#email").val("");
        $("#parola_create").val("");
        $("#confirmare_parola").val("");
        $("#creare_cont").dialog({
            width: 380,
            modal: true
        });
    }

    function CreateAccount() {
        var mesaj = "";
        var isValid = true;

        if (!validateEmail($("#email").val())) {
            isValid = false;
            mesaj += "Email invalid <br />";
        }

        if ($("#parola_create").val() != $("#confirmare_parola").val()) {
            isValid = false;
            mesaj += "Parola diferita de Confirmare Parola <br />"
        }

        if (isValid == false)
        {
            $("#error").html(mesaj + "<br />");
            return;
        }

        if ($("#parola_create").val() == $("#confirmare_parola").val()) {
            var requestData = {};
            requestData["nume_firma"] = $("#nume_firma").val();
            requestData["email"] = $("#email").val();
            requestData["parola"] = $("#parola_create").val();

            $.ajax({
                url: 'Facade/CreateAccount',
                type: 'POST',
                data: JSON.stringify(requestData),
                dataType: 'json',
                //contentType: 'application/json; charset=utf-8',
                error: function (response) {
                    //debugger;
                    if (response.statusText == "OK") {
                        if (response.responseText == "created") {
                            $("#error").hide();
                            $("#container_createaccount").hide();
                            $("#mesage_createaccount").show(500);
                            $("#mesage_createaccount").width(500);
                        }
                    }
                },
                success: function (response) {
                    debugger;

                    if (response == "created") {
                        $("#error").hide();
                        $("#container_createaccount").hide();
                        $("mesage_createaccount").show(5000);
                    }
                },
                async: true,
                processData: false
            });
        }
    }

// class preferences {

    function OpenPreferencesWindow() {
        LoadPreferences();

        $("#preferences").dialog({
            width: 380,
            modal: true
        });
    }

    function LoadPreferences() {
        $.ajax({
            url: 'Facade/LoadPreferences',
            type: 'POST',
            //data: JSON.stringify(requestData),
            //dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            error: function (data) {
                //debugger;
                //alert(data);
                if (data != "{}") {
                    json = $.parseJSON(data);
                    alert(json);
                }
            },
            success: function (data) {
                //debugger;
                //alert(data);
                if (data != "{}") {
                    json = $.parseJSON(data);
                    
                    var coredocuments = json["CoreDocuments"];
                    var customdocuments = json["CustomDocuments"];

                    if (coredocuments == 'True')
                        $('#coredocuments').prop('checked', true);
                    else
                        $('#coredocuments').prop('checked', false);

                    if (customdocuments == 'True')
                        $('#customdocuments').prop('checked', true);
                    else
                        $('#customdocuments').prop('checked', false);
                }
            },
            async: true,
            processData: false
        });
    }

    function SavePreferences() {
        var requestData = {};

        if ($("#coredocuments").is(':checked'))
            requestData["coredocuments"] = true;
        else
            requestData["coredocuments"] = false;

        if ($("#customdocuments").is(':checked'))
            requestData["customdocuments"] = true;
        else
            requestData["customdocuments"] = false;

        $.ajax({
            url: 'Facade/SavePreferences',
            type: 'POST',
            data: JSON.stringify(requestData),
            dataType: 'json',
            //contentType: 'application/json; charset=utf-8',
            error: function (data) {
                //debugger;
                //alert(data);               
                window.location.reload(true);
            },
            success: function (data) {
                //debugger;
                //alert(data);
                window.location.reload(true);
            },
            async: true,
            processData: false
        });

        $("#preferences").dialog("close");        
    }

// }

    function validateEmail(email) { 
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
                   .toString(16)
                   .substring(1);
    };

    function guid() {
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
               s4() + '-' + s4() + s4() + s4();
    }

    function CreateTemplate() {
        $("#content-middle").html("");
        $.get("Windows/CreateTemplate.html?version=" + guid(), function (data) {
            $('#content-middle').html(data);
            $("#getCode").bind('click', function (e) {
                debugger;
                $("#editor").text($("#document").html());
            });
        });        
    }

    function CreateInput() {
        $("#continut").append("<input type=\"text\" id=\"Nume\" value=\"Nume\" style=\"position: absolute;\">");
        var p = $("#Nume");
        p.val("Nume");
        var position = p.position();
        $("#Nume").keydown(function (event) {
            switch (event.keyCode) {
                case 37:
                    position.left = position.left - 1
                    $("#Nume").css("left", position.left);
                    //alert('left');
                    break;
                case 38:
                    position.top = position.top - 1;
                    $("#Nume").css("top", position.top);
                    //alert('up');
                    break;
                case 39:
                    position.left = position.left + 1;
                    $("#Nume").css("left", position.left);
                    //alert('right');
                    break;
                case 40:
                    position.top = position.top + 1;
                    $("#Nume").css("top", position.top);
                    //alert('down');
                    break;
            }
        });
    }

    function CreateSablon() {
        $("#dialogCreeazaSablonDocument").dialog({
            width: 350,
            modal: true
        });
    }