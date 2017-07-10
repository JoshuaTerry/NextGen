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

function LoadNoteDetailsGrid(entityType) {

    if (currentEntity) {


        var columns = [
            { dataField: 'DisplayName', caption: 'Title' },
            { dataField: 'CreatedOn', caption: 'Created Date', dataType: 'date' },
            { dataField: 'CreatedBy', caption: 'Created By' }
        ];


        CustomLoadGrid('notedetailsgrid', '.notedetailsgridcontainer', columns, 'entity/' + currentEntity.Id + '/notes?fields=Id,CreatedBy,CreatedOn,DisplayName', null, EditNoteDetails, null, function (e) {

            NewNoteDetailsModal(entityType);

        });




       
    }

}

function NewNoteDetailsModal(entityType) {

    $('.newnotesdetailmodallink').unbind('click');

    $('.newnotesdetailmodallink').click(function (e) {

    PopulateDropDown('.nd-Category', 'notecategories/', '', '');
    PopulateDropDown('.nd-NoteCode', 'notecodes/', '', '');
    PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '');
    PopulateDropDown('.nd-UserResponsible', 'users', '', '');

    e.preventDefault();

    var modal = $('.notesdetailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 700,
        resizable: false
    });

    $('.noteTopicSelectImage').unbind('click');

    $('.noteTopicSelectImage').click(function (e) {

        SaveNewNoteTopics(modal);

    });

    $('.cancelnotesmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        ClearNoteTopicTagBox(modal);

        $('.editnoteinfo').hide();

        $('.nd-CreatedBy').text('');
        $('.nd-UpdatedBy').text('');
        $('.nd-CreatedOn').text('');
        $('.nd-UpdatedOn').text('');

    });

    $('.savenotedetails').unbind('click');
    
    $('.savenotedetails').click(function () {

        var item = GetNoteDetailsToSave(modal, entityType);

        MakeServiceCall('POST', 'notes', item, function (data) {

            DisplaySuccessMessage('Success', 'Note Details saved successfully.');

            CloseModal(modal);

            $('.editnoteinfo').hide();

            $('.nd-CreatedBy').text('');
            $('.nd-UpdatedBy').text('');
            $('.nd-CreatedOn').text('');
            $('.nd-UpdatedOn').text('');

            ClearNoteTopicTagBox(modal);

            LoadNoteDetailsGrid();

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');

        });

        });

    });
}

function LoadNoteDetails(id) {

    $('.attachmentscontainer').empty()

    MakeServiceCall('GET', 'notes/' + id, null, function (data) {

        LoadSelectedNoteTopics(id);

        PopulateDropDown('.nd-Category', 'notecategories', '', '', data.Data.CategoryId);
        PopulateDropDown('.nd-NoteCode', 'notecodes', '', '', data.Data.NoteCodeId);
        PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '', data.Data.ContactMethodId);
        PopulateDropDown('.nd-UserResponsible', 'users', '', '', data.Data.UserResponsibleId);

        $('.nd-Title').val(data.Data.Title),
        $('.nd-Description').val(data.Data.Text),
        $('.nd-AlertStartDate').val(FormatJSONDate(data.Data.AlertStartDate)),
        $('.nd-AlertEndDate').val(FormatJSONDate(data.Data.AlertEndDate)),
        $('.nd-ContactDate').val(FormatJSONDate(data.Data.ContactDate)),
        $('.nd-NoteCode').val(data.Data.NoteCode),
        $('.nd-CreatedBy').text(data.Data.CreatedBy),
        $('.nd-UpdatedBy').text(data.Data.LastModifiedBy),
        $('.nd-CreatedOn').text(FormatJSONDate(data.Data.CreatedOn)),
        $('.nd-UpdatedOn').text(FormatJSONDate(data.Data.LastModifiedOn))
        LoadAttachments($('.attachmentscontainer'), "notesattachments", currentEntity.Id , id);
        if (data.Data.PrimaryContact != null) {
            var constituentLabel = data.Data.PrimaryContact.ConstituentNumber + ": " + data.Data.PrimaryContact.Name + ", " + data.Data.PrimaryContact.PrimaryAddress;
            $('.rs-Constituent1Information').val(constituentLabel);
        }
        else {
            $(modal).find('.rs-Constituent1Information').empty();
        }


    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', 'An error occurred during loading the Note Details.');
    });

}

function GetNoteDetailsToSave(modal, entityType) {

    var rawitem = {

        Title: $(modal).find('.nd-Title').val(),
        AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
        AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
        Text: $(modal).find('.nd-Description').val(),
        CategoryId: $(modal).find('.nd-Category').val(),
        ContactDate: $(modal).find('.nd-ContactDate').val(),
        NoteCodeId: $(modal).find('.nd-NoteCode').val(),
        UserResponsibleId: $(modal).find('.nd-UserResponsible').val(),
        PrimaryContactId: $(modal).find('.rs-Constituent1Id').val(),
        ParentEntityId: currentEntity.Id,
        EntityType: entityType, 
        ContactMethodId: $(modal).find('.nd-ContactMethod').val(),
        NoteTopics: GetNoteTopicsToSave()


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
    $('<span>').text(topic.DisplayName).appendTo($(t));
    $('<div>').addClass('dx-tag-remove-button')
        .click(DeleteFunction)
        .appendTo($(t));
}

function LoadSelectedNoteTopics(id) {

    MakeServiceCall('GET', 'notes/' + id + '/notetopics', null, function (data) {

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
        
        TopicIds = []
            
    var divs = $('.noteTopicSelect').children('div');

    $.map(divs, function (divid) { TopicIds.push({ "Id":  divid.id  }) });

    return TopicIds;

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

/* Loading Notes Modal Independently from Notes grid*/
function LoadNotesModal(id, entityType, cancelCallBack, saveCallBack)
{
    
    PopulateDropDown('.nd-Category', 'notecategories/', '', '');
    PopulateDropDown('.nd-NoteCode', 'notecodes/', '', '');
    PopulateDropDown('.nd-ContactMethod', 'notecontactcodes', '', '');

    //e.preventDefault();

    var modal = $('.notesdetailmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 750,
        resizable: false
    });

    LoadNoteDetails(id);
    
    $('.noteTopicSelectImage').unbind('click');

    $('.noteTopicSelectImage').click(function (e) {

        SaveNewNoteTopics(modal);

    });

    $('.cancelnotesmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        ClearNoteTopicTagBox(modal);

        $('.editnoteinfo').hide();

        $('.nd-CreatedBy').text('');
        $('.nd-UpdatedBy').text('');
        $('.nd-CreatedOn').text('');
        $('.nd-UpdatedOn').text('');

        if (cancelCallBack)
        {
            cancelCallBack();
        }

    });

    $('.savenotedetails').unbind('click');

    $('.savenotedetails').click(function () {

        var topicsavelist = GetNoteTopicsToSave();

        var item = GetNoteDetailsToSave(modal, entityType);

        MakeServiceCall('PATCH', 'notes', item, function (data) {

            DisplaySuccessMessage('Success', 'Note Details saved successfully.');

            CloseModal(modal);

            $('.editnoteinfo').hide();

            $('.nd-CreatedBy').text('');
            $('.nd-UpdatedBy').text('');
            $('.nd-CreatedOn').text('');
            $('.nd-UpdatedOn').text('');

            ClearNoteTopicTagBox(modal);

            LoadNoteDetailsGrid();

            if (saveCallBack) {
                saveCallBack();
            }

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred during saving the Note Details.');

        });

    });
}
