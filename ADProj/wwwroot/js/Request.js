﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $("#btn_Add").click(function (event) {
        //Extracting all selected fields
        var txtCategroy = $("#selectOne option:selected").text().toString();
        var txtDescription = $("#selectTwo option:selected").text().toString();
        var txtItemCode = $("#demo2").val().toString();
        var qty = $("#requestQty").val();

        //validations for quantity, category,item
        if (qty <= 0) {
            alert("Please enter a positive quantity");
            return;
        }

        if (qty > 99) {
            alert("Unable make a request for more than 99 items");
            return;
        }
        if (txtDescription == "" || txtDescription == "Select Item") {
            alert("Please choose an item");
            return;
        }


        var txtQty = qty.toString().toString();

        var tableRef = document.getElementById('tblRequestsDetails').getElementsByTagName('tbody')[0];
        var newRow = tableRef.insertRow();
        var cell1 = newRow.insertCell(0);
        var cell2 = newRow.insertCell(1);
        var cell3 = newRow.insertCell(2);
        var cell4 = newRow.insertCell(3);
        var cell5 = newRow.insertCell(4);

        var cell1Text = document.createTextNode(txtCategroy);
        cell1.appendChild(cell1Text);

        var cell2Text = document.createTextNode(txtDescription);
        cell2.appendChild(cell2Text);

        var cell3Text = document.createTextNode(txtItemCode);
        cell3.appendChild(cell3Text);

        var cell4Text = document.createTextNode(txtQty);
        cell4.appendChild(cell4Text);

        cell5.innerHTML = '<button class="btn_Delete" type="button">'
            + 'Delete</button>'
        $("#selectOne").val('');
        $("#selectTwo").val('');
        $("#requestQty").val('');
        $("#demo2").val('');
        $("#demo").val('');

        $('td:nth-child(3),th:nth-child(3)').hide();

    })
    $(".btn_Delete").click(function (event) {
        var currentTr = $(this).closest('tr');
        currentTr.remove();
    });
    $("#btnSubmit").click(function (event) {

        //valid if submitting an empty table
        var tbody = $("#tblRequestsDetails tbody");
        if (tbody.children().length == 0) {
            alert("Unable to submit an empty request form");
            return;
        }

        var requestDetails = new Array();
        $("#tblRequestsDetails TBODY TR").each(function () {
            var row = $(this);
            var rd = {};
            rd.Category = row.find("TD").eq(0).html();
            rd.Description = row.find("TD").eq(1).html();
            rd.ItemId = row.find("TD").eq(2).html();
            rd.Qty = row.find("TD").eq(3).html();
            requestDetails.push(rd);
        });
        alert(JSON.stringify(requestDetails));

        $.ajax({
            type: "POST",
            url: "/Request/InsertRequests",
            data: JSON.stringify(requestDetails),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
            }

        });
        $("#tblRequestsDetails tbody tr").remove();

    });

    $(document).on('click', '.btn_Delete', function () {
        var currentTr = $(this).closest('tr');
        currentTr.remove();
    });

    $("#selectOne").change(function () {
        if ($(this).data('options') === undefined) {
            /*Taking an array of all options-2 and kind of embedding it on the select1*/
            $(this).data('options', $('#selectTwo option').clone());
        }
        var id = $(this).val();
        var options = $(this).data('options').filter('[value=' + id + ']');
        $('#selectTwo').html(options);

        var id = $("#selectTwo").find(':selected').data('id')
        var uom = $("#selectTwo").find(':selected').data('uom')
        $('#demo').val(uom);
        $('#demo2').val(id);

    });
    $("#selectTwo").change(function () {
        var id = $(this).find(':selected').data('id')
        var uom = $(this).find(':selected').data('uom')
        $('#demo').val(uom);
        $('#demo2').val(id);

    });
});