function chkSelect_OnMouseMove(tableRow) {
    //debugger
    var checkBox = tableRow.childNodes[1].childNodes[0];
    if (checkBox.checked == false)
        tableRow.style.backgroundColor = "#ffffcc";
}

function chkSelect_OnMouseOut(tableRow, rowIndex) {
    //debugger
    var checkBox = tableRow.childNodes[1].childNodes[0];
    if (checkBox.checked == false) {
        var bgColor;
        if (rowIndex % 2 == 0)
            bgColor = "#ffffff";
        else
            bgColor = "#f5f5f5";
        tableRow.style.backgroundColor = bgColor;
    }
}
function chkSelect_OnMouseMove1(tableRow) {
    //debugger
    var checkBox = tableRow.childNodes[0].childNodes[0];
    if (checkBox.checked == false)
        tableRow.style.backgroundColor = "#ffffcc";
}

function chkSelect_OnMouseOut1(tableRow, rowIndex) {
    //debugger
    var checkBox = tableRow.childNodes[0].childNodes[0];
    if (checkBox.checked == false) {
        var bgColor;
        if (rowIndex % 2 == 0)
            bgColor = "#ffffff";
        else
            bgColor = "#f5f5f5";
        tableRow.style.backgroundColor = bgColor;
    }
}
function chkSelect_OnClick(checkBox, rowIndex) {
    debugger
    var bgColor;
    re = new RegExp('chkSelect');
    re2 = new RegExp('chkSelectAll');
    var tableRow = checkBox.parentNode.parentNode;

    if (rowIndex % 2 == 0)
        bgColor = "#ffffff";
    else
        bgColor = "#f5f5f5";
    if (checkBox.checked == true) {
        tableRow.style.backgroundColor = "#b0c4de";
        if ($(".chk").length == $(".chk:checked").length) {
            $("#chkSelectAll").attr("checked", "checked");
        }
    }
    else {
        tableRow.style.backgroundColor = bgColor;
        $("#chkSelectAll").removeAttr("checked");
    }
}
function chkSelectAll_OnClick(checkBox) {
    debugger
    re = new RegExp('chkSelect');
    re2 = new RegExp('chkSelectAll');
    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'checkbox') {
            if (re.test(elm.id) && re2.test(elm.id) == false) {
                elm.checked = checkBox.checked;
                var tableId = elm.parentNode.parentNode.id;
                var rowIndex = tableId.substring(tableId.length - 1, tableId.length);
                chkSelect_OnClick(elm, rowIndex);
            }
        }
    }
}
function chkSelectAll_OnClick1(checkBox) {
    debugger
    re = new RegExp('chkSelect');
    re2 = new RegExp('chkSelectAll');
    for (i = 0; i < document.forms[1].elements.length; i++) {
        elm = document.forms[1].elements[i];
        if (elm.type == 'checkbox') {
            if (re.test(elm.id) && re2.test(elm.id) == false) {
                elm.checked = checkBox.checked;
                var tableId = elm.parentNode.parentNode.id;
                var rowIndex = tableId.substring(tableId.length - 1, tableId.length);
                chkSelect_OnClick(elm, rowIndex);
            }
        }
    }
}

