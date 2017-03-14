/* 
   List of entities more or less taken from CustomFields
   May need to be altered as functionality is expanded
*/
var NoteEntity = {
    0: 'Constituent', 1: 'GeneralLedger', 2: 'AccountsPayable', 3: 'AccountsReceivable', 4 : 'FixedAssets',
    5: 'Inventory', 6: 'CashProcessing', 7: 'CashDisbursements', 8: 'CashReceipting', 9: 'Gifts',
    10: 'NamedFunds', 11: 'CropEvents', 12: 'PlannedGiving', 13: 'Campaigns', 14 : 'Investments',
    15: 'LineOfCredit', 16: 'Loans', 17: 'Portfolio', 18: 'Pools', 19: 'CRM',
    20: 'OfficeIntegration', 21: 'ProcessManagement', 22: 'ProjectManagement', 23: 'JobProcessing', 24: 'HealthPolicy', 25: 'SystemAdministration',
    26: 'Accounting'
};
var notetopicsloadedflag = false;


$(document).ready(function () {

    SetupNotesTab();

});

/* Notes Tab */

function SetupNotesTab() {
    $('#tab-notes-main').click(function (e) {

        LoadNoteDetailsGrid();

        NewNoteDetailsModal();

        SetupNoteTopicSelectModal()

    });
}

function LoadNoteDetailsGrid() {

    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'CreatedOn', caption: 'Created Date', dataType: 'date' },
    { dataField: 'CreatedBy', caption: 'Created By' },
    { dataField: 'Title', caption: 'Title' }
    ];

    LoadGrid('notedetailsgrid',
        'notedetailsgridcontainer',
        columns,
        'notes',
        null,
        EditNoteDetails,
        null);

}

function NewNoteDetailsModal() {

    $('.newnotesdetailmodallink').click(function (e) {

        PopulateDropDown('.nd-Category', 'notecategories/', '', '');

        e.preventDefault();

        modal = $('.notesdetailmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 500,
            resizable: false
        });

        $('.cancelmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

        });

        $('.savenotedetails').unbind('click');

        $('.savenotedetails').click(function () {

            GetNoteTopicsToSave();

            var item = {

                Title: $(modal).find('.nd-Title').val(),
                AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
                AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
                Text: $(modal).find('.nd-Description').val(),
                CategoryId: $(modal).find('.nd-Category').val(),
                ContactDate: $(modal).find('.nd-ContactDate').val(),
                NoteCodeId: $(modal).find('.nd-NoteCode').val(),
                ParentEntityId: currentEntity.Id,
                ParentEntityType: NoteEntity[0]

            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'notes/',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    DisplaySuccessMessage('Success', 'Note Details saved successfully.');

                    CloseModal(modal);

                    LoadNoteDetailsGrid();

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');
                }
            });

        });

    });
}

function EditNoteDetails(id) {

    var modal = $('.notesdetailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    LoadNoteDetails(id);

    $('.hidnoteid').val(id);

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.savenotedetails').unbind('click');

    $('.savenotedetails').click(function () {

        var item = {

            Title: $(modal).find('.nd-Title').val(),
            AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
            AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
            Text: $(modal).find('.nd-Description').val(),
            CategoryId: $(modal).find('.nd-Category').val(),
            ContactDate: $(modal).find('.nd-ContactDate').val(),
            NoteCode: $(modal).find('.nd-NoteCode').val(),
            ParentEntityId: currentEntity.Id,
            ParentEntityType : NoteEntity[0]

        }

        $.ajax({
            type: 'PATCH',
            url: WEB_API_ADDRESS + 'notes/' + id,
            data: item,
            contentType: 'application/x-www-form-urlencoded',
            crossDomain: true,
            success: function () {

                DisplaySuccessMessage('Success', 'Note Details saved successfully.');

                CloseModal(modal);

                LoadNoteDetailsGrid();

            },
            error: function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');
            }
        });

    });
}

function LoadNoteDetails(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notes/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            if (!notetopicsloadedflag) {

                LoadSelectedNoteTopics(id);

                notetopicsloadedflag = true;
            }

            PopulateDropDown('.nd-Category', 'notecategories', '', '', data.Data.CategoryId);
            PopulateDropDown('.nd-NoteCode', 'notecodes', '', '', data.Data.NoteCodeId);

            $('.nd-Title').val(data.Data.Title),
            $('.nd-Description').val(data.Data.Text),
            $('.nd-AlertStartDate').val(data.Data.AlertStartDate),
            $('.nd-AlertEndDate').val(data.Data.AlertEndDate),
            $('.nd-ContactDate').val(data.Data.ContactDate),
            $('.nd-ContactDate').val(data.Data.NoteCode),
            $('.nd-CreatedBy').text(data.Data.CreatedBy),
            $('.nd-UpdatedBy').text(data.Data.LastModifiedBy),
            $('.nd-CreatedOn').text(data.Data.CreatedOn),
            $('.nd-UpdatedOn').text(data.Data.LastModifiedOn)


        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the address.');
        }
    });
}

/* End Notes Tab */

/* NoteTopics */

function SetupNoteTopicSelectModal() {
    $('.noteTopicSelectImage').click(function (e) {

        LoadNoteTopicsTagBox();

    });
}

function LoadNoteTopicsTagBox() {

    var id = $('.hidnoteid').val();

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notetopics',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            CreateMultiSelectTags(data.Data, '.tagdropdowncontainer');

            $(modal).find('.tagdropdowncontainer').show();

            $('.savenotetopics').unbind('click');

            $('savenotetopics').click(function () {

                var item = []
                $('.tagdropdowncontainer').find('input').each(function (index, value) {

                    if ($(value).prop('checked')) {
                        item.push($(value).val());
                    }

                });

                $.ajax({
                    type: 'POST',
                    url: WEB_API_ADDRESS + 'notes/' + id + '/notetopics',
                    data: item,
                    contentType: 'application/x-www-form-urlencoded',
                    crossDomain: true,
                    success: function () {

                        DisplaySuccessMessage('Success', 'Note Topics saved successfully.');

                        CloseModal(modal);

                        LoadNoteDetailsGrid();

                    },
                    error: function (xhr, status, err) {
                        DisplayErrorMessage('Error', 'An error occurred during saving the Note Topics.');
                    }
                });

            });

            $('.cancelnotetopics').click(function (e) {

                e.preventDefault();

                $(modal).find('.tagdropdowncontainer').hide();

            })
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the note topics.');
        }
    });

}

function LoadSelectedNoteTopics(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + '/notetopics/' + id + '/notes/', 
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (topic) {

                var t = $('<div>').addClass('dx-tag-content').attr('id', topic.Id).appendTo($('.noteTopicSelect')); 
                $('<span>').text(topic.Name).appendTo($(t));
                $('<div>').addClass('dx-tag-remove-button')
                    .click(function () { 
                        $.ajax({
                            url: WEB_API_ADDRESS + '/notes/' + id + '/notetopics/' + topic.Id,
                            method: 'DELETE',
                            headers: GetApiHeaders(),
                            contentType: 'application/json; charset-utf-8',
                            dataType: 'json',
                            crossDomain: true,
                            success: function (data) {

                                DisplaySelectedTags();

                            },
                            error: function (xhr, status, err) {
                                DisplayErrorMessage('Error', 'An error occurred during saving the note topics.');
                            }
                        });
                    })
                    .appendTo($(t));
            });
           
        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during loading the Doing Business As.');
        }
    });

} 

function GetNoteTopicsToSave() {
    var topicid = [];

    var divs = $('.noteTopicSelect').children('div');

    $.map(divs, function (divid) { topicid.push(divid.id) });

    return topicid;

}

function CreateMultiSelectTags(topics, container) {

    var ul = $('<ul>');

    $.map(topics, function (topic) {

        var li = $('<li>');

        $('<input>').attr('type', 'checkbox').val(topic.Id).appendTo($(li));
        $('<span>').text(topic.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));
    });

    $(ul).appendTo($(container));
}

/* End NoteTopics*/

/* Note Alerts Modal */

function SetupNoteAlertsmodal() {

    // make ajax call, if data...

    // create modal and set events
}




/* End Note Alerts Modal */