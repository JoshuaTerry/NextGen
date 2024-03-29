﻿/* 
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

$(document).ready(function () {

    SetupNotesTab();

});

/* Notes Tab */

function SetupNotesTab() {
    $('#tab-notes-main').click(function (e) {

        LoadNoteDetailsGrid();

        NewNoteDetailsModal();

        GetNoteAlerts();

    });
}

function LoadNoteDetailsGrid() {

    var columns = [
    { dataField: 'Id', width: '0px', },
    { dataField: 'CreatedOn', caption: 'Created Date', dataType: 'date' },
    { dataField: 'CreatedBy', caption: 'Created By' },
    { dataField: 'Title', caption: 'Title' }
    ];

    CustomLoadGrid('notedetailsgrid', '.notedetailsgridcontainer', columns, currentEntity.Id + '/notes/', null, EditNoteDetails, null, null);

    PopulateDropDown('.nd-Category', 'notecategories', '', '', ''); 
    PopulateDropDown('.nd-NoteCode', 'notecodes', '', '');
    PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '');

}

function NewNoteDetailsModal() {

    $('.newnotesdetailmodallink').click(function (e) {

    PopulateDropDown('.nd-Category', 'notecategories/', '', '');
    PopulateDropDown('.nd-NoteCode', 'notecodes/', '', '');
    PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '');

    e.preventDefault();

    modal = $('.notesdetailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.noteTopicSelectImage').unbind('click');

    $('.noteTopicSelectImage').click(function (e) {

        SaveNewNoteTopics(modal);

    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        ClearNoteTopicTagBox(modal);

    });

    $('.savenotedetails').unbind('click');

    $('.savenotedetails').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetNoteDetailsToSave();

        MakeServiceCall('POST', 'notes', item, function (data) {

            MakeServiceCall('POST', 'notes/' + data.Data.Id + '/notetopics/', JSON.stringify(topicsavelist), function () {

                DisplaySuccessMessage('Success', 'Note topics saved successfully.');

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');

            });

            DisplaySuccessMessage('Success', 'Note Details saved successfully.');

            CloseModal(modal);

            ClearNoteTopicTagBox(modal);

            LoadNoteDetailsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');

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

    $('.noteTopicSelectImage').unbind('click');

    $('.noteTopicSelectImage').click(function (e) {

        SetupNoteTopicsMultiselectModal();

        $(modal).find('.tagdropdowncontainer').show();

        $('.savenotetopics').unbind('click');

        $('.savenotetopics').click(function (e) {

               var newsavednotetopics = GetCheckedNoteTopics();

               $.map(newsavednotetopics, function (topicid) {

                    MakeServiceCall('GET', 'notetopics/' + topicid, null, function (data) {

                        var topic = data.Data;

                        StyleAndSetupIndividualTags(topic, function () {

                            var removeid = '#' + data.Data.Id;

                            $('.noteTopicSelect > div').remove(removeid);

                            MakeServiceCall('DELETE', 'notes/' + id + '/notetopics/' + topic.Id, null, null,

                            function (xhr, status, err) {
                                DisplayErrorMessage('Error', 'An error occurred during saving the note topics.');
                            });

                        }, function (xhr, status, err) {
                            DisplayErrorMessage('Error', 'An error occurred during saving the note topics.');
                        });

                    });

                });

                $(modal).find('.tagdropdowncontainer').hide();

                ClearNoteTopicSelectModal();

                newsavednotetopics = null;

            });

        $('.cancelnotetopics').click(function (e) {

            e.preventDefault();

            $(modal).find('.tagdropdowncontainer').hide();

            ClearNoteTopicSelectModal();

        });


    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        ClearNoteTopicTagBox(modal);

    });

    $('.savenotedetails').unbind('click');

    $('.savenotedetails').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetNoteDetailsToSave();

        MakeServiceCall('PATCH', 'notes/' + id, item, function (data) {

            MakeServiceCall('POST', 'notes/' + data.Data.Id + '/notetopics/', JSON.stringify(topicsavelist), function (data) {

                DisplaySuccessMessage('Success', 'Note topics saved successfully.');

            }, function (xhr, status, err) {

                DisplayErrorMessage('Error', 'An error occurred during saving the Note topics.');

            });

            DisplaySuccessMessage('Success', 'Note Details saved successfully.');

            CloseModal(modal);

            ClearNoteTopicTagBox(modal);

            LoadNoteDetailsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');
        });

    });
}

function LoadNoteDetails(id) {

    MakeServiceCall('GET', 'notes/' + id, null, function (data) {

        LoadSelectedNoteTopics(id);

        PopulateDropDown('.nd-Category', 'notecategories', '', '', data.Data.CategoryId);
        PopulateDropDown('.nd-NoteCode', 'notecodes', '', '', data.Data.NoteCodeId);
        PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '', data.Data.ContactMethodId);

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


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Note Details.');
    });

}

function GetNoteDetailsToSave() {

    var rawitem = {

        Title: $(modal).find('.nd-Title').val(),
        AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
        AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
        Text: $(modal).find('.nd-Description').val(),
        CategoryId: $(modal).find('.nd-Category').val(),
        ContactDate: $(modal).find('.nd-ContactDate').val(),
        NoteCodeId: $(modal).find('.nd-NoteCode').val(),
        ParentEntityId: currentEntity.Id,
        EntityType: NoteEntity[0],
        ContactMethodId: $(modal).find('.nd-ContactMethod').val(),

    };

    var item = JSON.stringify(rawitem);

    return item;

}

/* End Notes Tab */

/* NoteTopics */

function SaveNewNoteTopics(modal) {

    SetupNoteTopicsMultiselectModal();

    $(modal).find('.tagdropdowncontainer').show();

    $('.savenotetopics').unbind('click');

    $('.savenotetopics').click(function (e) {

        var newsavednotetopics = GetCheckedNoteTopics();

        $.map(newsavednotetopics, function (topicid) {

            MakeServiceCall('GET', 'notetopics/' + topicid, null, function (data) {

                var topic = data.Data

                StyleAndSetupIndividualTags(topic, function () {

                    var removeid = '#' + data.Data.Id;

                    $('.noteTopicSelect > div').remove(removeid);

                });

            }, function (xhr, status, err) {
                DisplayErrorMessage('Error', 'An error occurred during displaying the note topics.');
            });
                    
        });

        $(modal).find('.tagdropdowncontainer').hide(); 

        ClearNoteTopicSelectModal();

        newsavednotetopics = null; 

    });

    $('.cancelnotetopics').click(function (e) {

        e.preventDefault();

        $(modal).find('.tagdropdowncontainer').hide();

        ClearNoteTopicSelectModal();

    });

}

function SetupNoteTopicsMultiselectModal() {

    MakeServiceCall('GET', 'notetopics', null, function (data) {

        CreateMultiSelectTopics(data.Data, $('.tagdropdowncontainer'));

    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', 'An error occurred during loading the note topics.');

    });

}

function ClearNoteTopicSelectModal() {

    $('.tagdropdowncontainer > ul').remove();

}

function ClearNoteTopicTagBox(modal) {

    $(modal).find('.noteTopicSelect > div.dx-tag-content').remove();

}

function StyleAndSetupIndividualTags(topic, DeleteFunction) { 

    var t = $('<div>').addClass('dx-tag-content').attr('id', topic.Id).appendTo($('.noteTopicSelect')); 
    $('<span>').text(topic.Name).appendTo($(t));
    $('<div>').addClass('dx-tag-remove-button')
        .click(DeleteFunction)
        .appendTo($(t));
}

function LoadSelectedNoteTopics(id) {

    MakeServiceCall('GET', 'notetopics/' + id + '/notes', null, function (data) {

        $.map(data.Data, function (topic) {

            StyleAndSetupIndividualTags(topic, function () {

                var removeid = '#' + topic.Id;

                $('.noteTopicSelect > div').remove(removeid);

                MakeServiceCall('DELETE', '/notes/' + id + '/notetopics/' + topic.Id, null, null,

                function (xhr, status, err) {

                    DisplayErrorMessage('Error', 'An error occurred during deleting the note topics.');

                });

            });

        });

    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', 'An error occurred during loading the Note Topics.');

    });

} 

function GetNoteTopicsToSave() {
    var topicidobject = { 
        
        TopicIds: []
            
     };

    var divs = $('.noteTopicSelect').children('div');

    $.map(divs, function (divid) { topicidobject.TopicIds.push(divid.id) });

    return topicidobject;

}

function GetCheckedNoteTopics() {

    var item = [];

    $('.tagdropdowncontainer').find('input').each(function (index, value) {

        if ($(value).prop('checked')) {

            item.push($(value).val());

        }

    });

    return item;

}

function CreateMultiSelectTopics(topics, container) {

    var ul = $('<ul>');

    $.map(topics, function (topic) {

        var li = $('<li>');

        $('<input>').attr('type', 'checkbox').val(topic.Id).appendTo($(li));
        $('<span>').text(topic.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));
    });

    $(ul).appendTo($(container));
}

/* End NoteTopics */

/* Note Alerts Modal */

function GetNoteAlerts() {

    MakeServiceCall('GET', 'notealert/' + currentEntity.Id, null, function (data) {

        if (data.Data.length > 0) {

            SetupNoteAlertModal();

            LoadNoteAlertGrid(data.Data);

            $('.notealertmodal').show();

        }

    },

    function (xhr, status, err) {

    });
}

function LoadNoteAlertGrid(data) {

    var columns = [
        { dataField: 'Id', width: '0px', },
        { dataField: 'AlertStartDate', caption: 'Alert Date Start', dataType: 'date' },
        { dataField: 'AlertEndDate', caption: 'Alert Date End', dataType: 'date' },
        { dataField: 'Title', caption: 'Title' }
    ];

    CustomLoadGrid('notealertgrid', '.notealertgridcontainer', columns, 'notealert/' + currentEntity.Id, null, EditNoteDetails, null, null);

}

function SetupNoteAlertModal() {

    var modal = $('.notealertmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

}
/* End Note Alerts Modal */