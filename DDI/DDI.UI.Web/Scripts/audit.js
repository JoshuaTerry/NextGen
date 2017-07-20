/* Audit Section */

function ShowAuditData(id) {

    $('.newauditmodal').click(function (e) {

        e.preventDefault();
        var modal = $('.auditmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 800,
            height: 600,
            resizable: true
        });

        LoadAuditTable(id);
    });
    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.queryAudit').unbind('click');

    $('.queryAudit').click(function () {

        $.ajax({
            type: 'GET',
            url: WEB_API_ADDRESS + 'audit/flat/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            headers: GetApiHeaders(),
            success: function (data) {

                LoadAuditTable(id);

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
            }
        });

    });

}

function LoadAuditTable(id) {
    var start = $(modal).find('.na-StartDate').val();
    var end = $(modal).find('.na-EndDate').val();
    var route = 'audit/flat/' + id;
    var columns = [
        { dataField: 'ChangeSetId', groupIndex: 0, sortOrder: 'desc', sortIndex: 0 },
        { dataField: 'Timestamp', caption: 'Date', dataType: 'date', width: '10%' },
        { dataField: 'User' },
        { dataField: 'ChangeType', width: '100px' },
        { dataField: 'Property' },
        { dataField: 'OldValue', caption: 'Old Value' },
        { dataField: 'OldDisplayName', caption: 'Old Display Name' },
        { dataField: 'NewValue', caption: 'New Value' },
        { dataField: 'NewDisplayName', caption: 'New Display Name' }
    ];

    CustomLoadGrid('auditgrid', '.auditgridcontainer', columns, route, '', '', '', null);

}
/* End Audit Section */