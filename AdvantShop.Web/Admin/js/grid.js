function rgb(color) { if (/#/.test(color)) { var rgb = color.replace(/[# ]/g, "").replace(/^(.)(.)(.)$/, '$1$1$2$2$3$3').match(/.{2}/g); for (var i = 0; i < 3; i++) rgb[i] = parseInt(rgb[i], 16); } else if (/rgb/.test(color)) { var rgb = /(\d+), (\d+), (\d+)/.exec(color); for (var i = 0; i < 3; i++) rgb[i] = Number(rgb[i + 1]); rgb.pop(); }; return rgb; }

//var __mouseouttimeout;
//var __sender;
//function mouseover() {
//    if (__sender != undefined) {
//        clearTimeout(__mouseouttimeout);
//        if (__sender.style.backgroundColor == 'rgb(197, 213, 233)' || __sender.style.backgroundColor == '#c5d5e9') { __sender.style.backgroundColor = __sender.originalstyle; }
//    }
//    if (this.style.backgroundColor != 'rgb(244, 122, 64)' && this.style.backgroundColor != '#f47a40') { this.originalstyle = this.style.backgroundColor; this.style.backgroundColor = '#c5d5e9'; }
//}

function mouseout() {
    //__sender = this;
    //__mouseouttimeout = setTimeout("if (__sender.style.backgroundColor == 'rgb(197, 213, 233)' || __sender.style.backgroundColor == '#c5d5e9') { __sender.style.backgroundColor = __sender.originalstyle; }", 10);
    //if (sender.style.backgroundColor == 'rgb(197, 213, 233)' || sender.style.backgroundColor == '#c5d5e9') { sender.style.backgroundColor = sender.originalstyle; }
}

function initgrid() {
    //$("tr.row1, tr.row2").hover(mouseover, mouseout);
    //$("tr.row2, tr.row2").hover(function () { $(this).addClass("blue"); }, function () { $(this).removeClass("blue"); });

    if ($(".txttooltip").length > 0) {
        $(".txttooltip").each(function (i) {
            $(this).tooltip({
                delay: 10,
                showURL: false,
                bodyHandler: function () {
                    return $(this).attr("abbr");
                }
            });
        });
    }

    $("tr.row1").each(function (i) {
        $(this).find("td").not(":first").not(":last").click(function () {
            row_edit($(this).parent());
            this.focus();
            $(this).find("input").focus();
        });
    });
    $("tr.row2").each(function (i) {
        $(this).find("td").not(":first").not(":last").click(function () {
            row_edit($(this).parent());
            this.focus();
            $(this).find("input").focus();
        });
    });
    if ($(".readOnlyRowInInit").val() == "True") {
        $("table.tableview > tbody > tr > td > input[type='text']:not(.add)").attr("readonly", "readonly");
    }
    $("table.tableview > tbody > tr > td  input[type='checkbox']:not(.sel):not(.add):not(span.add > input)").attr("disabled", "disabled");
    $("table.tableview > tbody > tr > td > select:not(.sel):not(.add)").attr("disabled", "disabled");
    $("input.sel").click(chboxselect_click);
    $("td.checkboxcolumn").click(cellselect_click);

    $("input.sel:checked").each(function (idx, item) { doCheck(item); });


    if ($(".readOnlyRowInInit").val() != "True") {
        $("table.tableview > tbody > tr").removeClass("readonlyrow");
    }

    refreshCount();
}

var __OriginalTxtValues = new Array();
var __OriginalCheckBoxValues = new Array();
var __OriginalSelectValues = new Array();

function row_edit(row) {
    if ($(".readOnlyGrid").val().toLowerCase() == "false" && ($(row).find(".nonEditRow").length == 0) || $(row).attr("rowType") == "product") {
        $(row).parent().find("tr:not(tr.readonlyrow)").each(function (i) {
            row_canceledit(this);
        });
        $(row).find(".deletebtn").hide();
        $(row).find(".editbtn").hide();
        $(row).find(".cancelbtn").show();
        $(row).find(".updatebtn").show();
        $(row).find("td:not(:last)").unbind("click");
        var txts = $(row).find("input[type='text']");
        txts.removeAttr("readonly");
        //txts.keypress(function(e) { if (!e) var e = window.event; if (e.keyCode == 13) { $(this).parent().parent().find("input.updatebtn").click(); return false; } if (e.keyCode == 27) { row_canceledit(row); $(this).parent().focus(); } })
        $(row).find("input").keypress(function (e) {
            if (!e) var e = window.event;
            if (e.keyCode == 13) {
                $(this).parent().parent().find("input.updatebtn").click();
                return false;
            }
            if (e.keyCode == 27) {
                row_canceledit(row);
                $(this).parent().focus();
            }
        });

        if ($(".readOnlyRowInInit").val() == "True") {
            $(row).removeClass("readonlyrow");
        }
        txts.each(function (i) { __OriginalTxtValues[i] = txts[i].value; });

        var checkboxes = $(row).find("td input[type='checkbox']:not(.sel):not(.add):not(span.add > input):not([readonly])");
        checkboxes.removeAttr("disabled");
        checkboxes.each(function (i) { __OriginalCheckBoxValues[i] = checkboxes[i].checked; });

        var selects = $(row).find("td select:not(.sel):not(.add)");
        selects.removeAttr("disabled");
        selects.each(function (i) { __OriginalSelectValues[i] = $(selects[i]).val(); });
    }
}

function row_canceledit(row) {
    $(row).find(".cancelbtn").hide();
    $(row).find(".updatebtn").hide();
    $(row).find(".deletebtn").show();
    $(row).find(".editbtn").show();
    $(row).find("td:not(:last)").click(function () {
        row_edit($(this).parent());
        this.focus();
        $(this).find("input").focus();
    });
    if ($(".readOnlyRowInInit").val() == "True") {
        $(row).addClass("readonlyrow");
    }
    var txts = $(row).find("input[type='text']");
    txts.attr("readonly", "readonly");
    if ($(".resetToDefaultValue").val() == "True") {
        txts.each(function (i) {
            if (!$(txts[i]).hasClass('add')) {
                txts[i].value = __OriginalTxtValues[i];
            }
        });
    }
    var checkboxes = $(row).find("td input[type='checkbox']:not(.sel):not(.add):not(span.add > input):not([readonly])");
    checkboxes.attr("disabled", "disabled");
    checkboxes.each(function (i) { checkboxes[i].checked = __OriginalCheckBoxValues[i]; });

    var selects = $(row).find("td select:not(.sel):not(.add)");
    selects.attr("disabled", "disabled");
    selects.each(function (i) { $(selects[i]).val(__OriginalSelectValues[i]); });

    $("body").trigger("row_canceledit");

    return false;
}

function refreshCount() {
    var obj = $("#selectedIdsCount");
    if (obj.length > 0) {
        if ($("#selectedIdsCount").length > 0) {
            if ($.trim($("#SelectedIds").val()) == "-1") {
                var total = 0;
                $("span.foundrecords").each(function () {
                    var count = parseFloat($(this).text());
                    if (!isNaN(count))
                        total += count;
                });
                obj.text(total);
            } else {
                var ids = new Array();
                $.each($("#SelectedIds").val().split(" "), function (idx, item) {
                    if (item && item.length && $.inArray(item, ids) == -1) {
                        ids.push(item);
                    }
                });

                var totalIds = ids.length;
                obj.text(totalIds);
            }
        }

    }
}

function chboxselect_click(e) {

    doCheck(this);

    var e = e || window.event;

    e.cancelBubble = true;
    if (e.stopPropagation) e.stopPropagation();

    //return false;
}

function doCheck(obj) {
    if (obj.checked)
        $(obj).parent().parent().addClass("selectedrow");
    else
        $(obj).parent().parent().removeClass("selectedrow");

    //get id of row
    var parentElements = obj.parentNode.childNodes;
    var id = -1;
    for (var i = 0; i < parentElements.length; i++) {
        if (parentElements[i].type == 'hidden') {
            id = parentElements[i].value;
            break;
        }
    }

    var ids; //array of id's
    var elIds = document.getElementById("SelectedIds"); //hidden field for saving id's
    if (elIds == null) return;
    //getting id's from hidden field
    ids = elIds.value.trim().split(" ");
    if (ids == "") ids = new Array();
	
    if (ids.includes(id) && obj.checked) {
        return;
    }
	
    //if(ids[0]=="-1")

    var theBox = $("span.checkboxcheckall input")[0];

    if ((ids[0] != "-1" && obj.checked == true) || (ids[0] == "-1" && obj.checked != true)) {
        ids.push(id);
    }
    else {
        //search id for delete
        for (var i2 = 0; i2 < ids.length; i2++) {
            if (ids[i2] == id) ids[i2] = "";
        }
        if (theBox != null) {
            theBox.checked = false;
        }
    }
    //saving id's to hidden field
    var count = 0;
    elIds.value = "";
    for (var i1 = 0; i1 < ids.length; i1++)
        if (ids[i1] != "") {
            if (ids[i1] != "-1") count++;
            elIds.value += ids[i1] + " ";
        }
    if ($("#selectedIdsCount").length > 0) {
        if (ids[0] != "-1") {
            $("#selectedIdsCount").text(count);
        } else {
            var prevVal = parseInt($("#selectedIdsCount").text());
            $("#selectedIdsCount").text(obj.checked ? prevVal + 1 : prevVal - 1);
        }
    }

    var ischeckedall = true;

    //    var theBox = (sender.type == "checkbox") ? sender : sender.children.item[0];
    //    elm = theBox.form.elements;    
    if (this.checked) {
        var elements = $("table.tableview tbody tr");
        for (i = 0; i < elements.length; i++) {
            //if (elements[i].firstChild.firstChild.type == 'checkbox' && elements[i].firstChild.firstChild.id != theBox.id && elements[i].firstChild.firstChild.checked != xState) {
            var childs = elements[i].cells[0].childNodes;
            for (j = 0; j < childs.length; j++)
                if (childs[j].type == "checkbox" && childs[j].id != theBox.id && childs[j].checked == false) {
                    ischeckedall = false;
                    break;
                }
        }
    }
    else ischeckedall = false;
    if (theBox != null) {
        theBox.checked = ischeckedall;
    }
}

function cellselect_click() {

    var parentElements = this.childNodes;
    var id = -1;
    for (var i = 0; i < parentElements.length; i++) {
        if (parentElements[i].type == 'hidden') {
            id = parentElements[i].value;
            break;
        }
    }
    var childs = this.childNodes;
    for (i = 0; i < childs.length; i++)
        if (childs[i].type == "checkbox") childs[i].click();
}

function SelectVisible(xState) {
    var theBox = $("span.checkboxcheckall")[0];
    if (theBox != undefined) theBox = theBox.firstChild;
    else return;
    var elements = $("table.tableview tbody  tr");
    for (i = 0; i < elements.length; i++) {
        var childs = elements[i].cells[0].childNodes;
        for (j = 0; j < childs.length; j++)
            if (childs[j].type == "checkbox" && childs[j].id != theBox.id && childs[j].checked != xState) {
                childs[j].click();
                break;
            }
    }
    theBox.checked = xState;
}

function HighlightRows() {

    var theBox = $("span.checkboxcheckall input")[0];
    if (theBox == undefined) return;

    ischeckedall = true;

    var checkboxes = $("td.checkboxcolumn input");
    for (i = 0; i < checkboxes.length; i++)
        if (checkboxes[i].type == "checkbox" && checkboxes[i].id != theBox.id)
            if (checkboxes[i].checked) HighlightRow(checkboxes[i]); else ischeckedall = false;

    theBox.checked = ischeckedall;
}
function SelectAll(xState) {
    SelectVisible(xState);
    if (xState) {
        var total = 0;
        $("span.foundrecords").each(function () {
            var count = parseFloat($(this).text());
            if (!isNaN(count))
                total += count;
        });
        $("#selectedIdsCount").text(total);
        $("#SelectedIds").val('-1');
    } else {
        $("#selectedIdsCount").text("0");
        $("#SelectedIds").val('');
    }

}

function ResetFilter() {
    SelectAll(false);

    document.getElementById('SelectedIds').value = '';
    document.getElementById('selectedIdsCount').innerHTML = '0';

    var txts = $("input.filtertxtbox");
    for (var i = 0; i < txts.length; i++) {
        var defValue = $(txts[i]).attr("data-default-value") || '';
        txts[i].value = defValue;
    }

    var chboxes = $("select.dropdownselect");
    for (var i1 = 0; i1 < chboxes.length; i1++)
        chboxes[i1].selectedIndex = 0;
}

function FilterClick() {
    if ($("select.dropdownselect")[0].selectedIndex == 0) {
        SelectAll(false);
    }
}