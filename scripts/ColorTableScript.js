$(document).ready(function () {
    loadColors();
    $("#colorTable tbody").sortable({
        update: function (event, ui) {
            updateDisplayOrder();
        }
    });
});

function loadColors() {
    $.ajax({
        url: 'ColorTable.aspx/GetColors',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        success: function (response) {
            var colors = response.d;
            var tbody = $('#colorTable tbody');
            tbody.empty();

            colors.forEach(function (color) {
                tbody.append(createRow(color));
            });
        }
    });
}

function createRow(color) {
    return `<tr data-id="${color.Id}">
        <td>${color.ColorName}</td>
        <td>${color.Price}</td>
        <td>${color.DisplayOrder}</td>
        <td>${color.InStock ? 'Yes' : 'No'}</td>
        <td>
            <button onclick="editColor(${color.Id})">Edit</button>
            <button onclick="deleteColor(${color.Id})">Delete</button>
        </td>
    </tr>`;
}

function updateDisplayOrder() {
    var updatedColors = [];
    $('#colorTable tbody tr').each(function (index, row) {
        var colorId = $(row).data('id');
        updatedColors.push({
            Id: colorId,
            DisplayOrder: index + 1
        });
    });

    $.ajax({
        url: 'ColorTable.aspx/UpdateDisplayOrder',
        type: 'POST',
        data: JSON.stringify({ colors: updatedColors }),
        contentType: 'application/json',
        dataType: 'json',
        success: function () {
            console.log('Display order updated successfully');
            loadColors();
        },
        error: function () {
            alert('Error updating display order');
        }
    });
}

function addColor() {
    var color = {
        ColorName: $('#colorName').val(),
        Price: parseFloat($('#price').val()),
        DisplayOrder: parseInt($('#displayOrder').val()),
        InStock: $('#inStock').is(':checked')
    };

    $.ajax({
        url: 'ColorTable.aspx/AddColor',
        type: 'POST',
        data: JSON.stringify({ color: color }),
        contentType: 'application/json',
        dataType: 'json',
        success: function () {
            loadColors();
            clearForm();
        }
    });
}

function editColor(id) {
    var row = $(`tr[data-id="${id}"]`);
    var cells = row.find('td');

    row.addClass('edit-mode');
    cells.eq(0).html(`<input type="text" value="${cells.eq(0).text()}">`);
    cells.eq(1).html(`<input type="number" value="${cells.eq(1).text()}">`);
    cells.eq(2).html(`<input type="number" value="${cells.eq(2).text()}">`);
    cells.eq(3).html(`<input type="checkbox" ${cells.eq(3).text() === 'Yes' ? 'checked' : ''}>`);
    cells.eq(4).html(`
        <button onclick="saveEdit(${id})">Save</button>
        <button onclick="cancelEdit(${id})">Cancel</button>
    `);
}

function saveEdit(id) {
    var row = $(`tr[data-id="${id}"]`);
    var color = {
        Id: id,
        ColorName: row.find('input').eq(0).val(),
        Price: parseFloat(row.find('input').eq(1).val()),
        DisplayOrder: parseInt(row.find('input').eq(2).val()),
        InStock: row.find('input').eq(3).is(':checked')
    };

    $.ajax({
        url: 'ColorTable.aspx/UpdateColor',
        type: 'POST',
        data: JSON.stringify({ color: color }),
        contentType: 'application/json',
        dataType: 'json',
        success: function () {
            loadColors();
        }
    });
}

function deleteColor(id) {
    if (confirm('Are you sure you want to delete this color?')) {
        $.ajax({
            url: 'ColorTable.aspx/DeleteColor',
            type: 'POST',
            data: JSON.stringify({ id: id }),
            contentType: 'application/json',
            dataType: 'json',
            success: function () {
                loadColors();
            }
        });
    }
}

function clearForm() {
    $('#colorName').val('');
    $('#price').val('');
    $('#displayOrder').val('');
    $('#inStock').prop('checked', false);
}
