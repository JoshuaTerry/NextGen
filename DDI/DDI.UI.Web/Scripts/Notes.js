var notetopicsloadedflag = false;

$(document).ready(function () {

    LoadNoteDetailsGrid();

    NewNoteDetailsModal();

    SetupNoteTopicSelectModal()

});

/* Notes Tab */

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
                // ParentEntityId: currentEntity.Id

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
            LastModifiedOn: $.datepicker.formatDate('yy-mm-dd', new Date()),
            LastModifiedBy: currentEntity.Id,
            NoteTopics: GetNoteTopicsToSave()

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

function GetNoteTopicsToSave() {
    var topicid= [];

    var divs = $('.noteTopicSelect').children('div');

    $.map(divs, function (divid) { topicid.push(divid.id) });

    return topicid;

}

function LoadNoteDetails(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notes/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            if (!notetopicsloadedflag) {

                LoadSelectedNoteTopics();

                notetopicsloadedflag = true;
            }

            PopulateDropDown('.nd-Category', 'notecategories', '', '', data.Data.CategoryId);

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

        LoadNoteDetailsTagBox();

    });
}

function LoadNoteDetailsTagBox() {

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

                // route to save checked notedetails (save item)

                // var item = []
                //$('.tagselectgridcontainer').find('input').each(function (index, value) {

                //    if ($(value).prop('checked')) {
                //        item.push($(value).val());
                //    }

                //});

                // will want to refresh

                // route to update notedetails from lookup table (

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

function LoadTagSelector(type) { // this will show you how to find the topics to save

    $('.tagselect').each(function () {

        var img = $('.tagSelectImage');

        if (img.length === 0) {
            img = $('<div>').addClass('tagSelectImage');

            $(img).click(function () {

                modal = $('.tagselectmodal').dialog({
                    closeOnEscape: false,
                    modal: true,
                    width: 450,
                    resizable: false
                });

                LoadAvailableTags();

                $('.saveselectedtags').unbind('click');

                $('.saveselectedtags').click(function () {

                    var tagIds = [];

                    $('.tagselectgridcontainer').find('input').each(function (index, value) {

                        if ($(value).prop('checked')) {
                            tagIds.push($(value).val());
                        }

                    });

                    $.ajax({
                        url: WEB_API_ADDRESS + 'constituents/' + currentEntity.Id + '/constituenttags',
                        method: 'POST',
                        headers: GetApiHeaders(),
                        data: JSON.stringify({ tags: tagIds }),
                        contentType: 'application/json; charset-utf-8',
                        dataType: 'json',
                        crossDomain: true,
                        success: function (data) {

                            // Display success
                            DisplaySuccessMessage('Success', 'Tags saved successfully.');

                            CloseModal(modal);

                            currentEntity = data.Data;

                            DisplaySelectedTags();

                        },
                        error: function (xhr, status, err) {
                            DisplayErrorMessage('Error', 'An error occurred during saving the tags.');
                        }
                    });

                });

            });

            $(this).after($(img));
        }

        $(img).hide();

    });

}

function LoadSelectedNoteTopics() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notetopics', // this will be the lookup table
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function (topic) {

                var t = $('<div>').addClass('dx-tag-content').attr('id', topic.Id).appendTo($('.noteTopicSelect')); 
                $('<span>').text(topic.Name).appendTo($(t));
                $('<div>').addClass('dx-tag-remove-button')
                    .click(function () { // adds cancel functionality - should point to lookup table
                        $.ajax({
                            url: WEB_API_ADDRESS,
                            method: 'DELETE',
                            headers: GetApiHeaders(),
                            contentType: 'application/json; charset-utf-8',
                            dataType: 'json',
                            crossDomain: true,
                            success: function (data) {
                                //currentEntity = data.Data;

                                //DisplaySelectedTags();

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

} // working

function RefreshTags() {

    if (currentEntity && currentEntity.Tags) {

        $.map(currentEntity.Tags, function (item) {

            $('input[value=' + item.Id + ']').prop('checked', true);

        });

    };

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