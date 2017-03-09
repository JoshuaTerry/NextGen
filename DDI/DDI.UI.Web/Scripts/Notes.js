$(document).ready(function () {

  //  DisplaySelectedTags(); // may not actually want? 

    LoadNoteDetailsGrid();

    NewNoteDetailsModal();

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

            var item = {

                Title: $(modal).find('.nd-Title').val(),
                AlertStartDate: $(modal).find('.nd-AlertStartDate').val(),
                AlertEndDate: $(modal).find('.nd-AlertEndDate').val(),
                Text: $(modal).find('.nd-Description').val(),
                CategoryId: $(modal).find('.nd-Category').val(),
                ContactDate: $(modal).find('.nd-ContactDate').val(),
                CreatedOn: $.datepicker.formatDate('yy-mm-dd', new Date()),
                CreatedBy: currentEntity.Id,
                LastModifiedBy: currentEntity.Id,
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

    LoadNoteDetails(id, modal);

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
    var t = {};


    return t;

}

function LoadNoteDetails(id) {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notes/' + id,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            LoadAvailableNoteTopics();

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

function SetupNoteTopicSelect() {
    // still needs to exist, but needs to do something different
    $('.noteTopicSelectImage').click(function () {
        LoadNoteDetailsTagBox();
    });
}
/* End Notes Tab */

/* NoteTopics */

function LoadNoteDetailsTagBox() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notetopics',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

        


            $(modal).find('.tagdropdowncontainer').show();

            $('.savenotetopics').unbind('click');

            $('savenotetopics').click(function () {

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

function LoadTagSelector(type) {

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

function LoadAvailableNoteTopics() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notetopics',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            $.map(data.Data, function () {

                var t = $('<div>').addClass('dx-tag-content').attr('id', data.Data.Id).appendTo($('.nd-Topics.tagselect')); // this needs to be my tagbox
                $('<span>').text(data.Data.DisplayName).appendTo($(t));
                $('<div>').addClass('dx-tag-remove-button')
                    .click(function () { // adds the 'x'
                        $.ajax({
                            url: WEB_API_ADDRESS + 'constituents/' + currentEntity.Id + '/tag/' + tag.Id,
                            method: 'DELETE',
                            headers: GetApiHeaders(),
                            contentType: 'application/json; charset-utf-8',
                            dataType: 'json',
                            crossDomain: true,
                            success: function (data) {
                                currentEntity = data.Data;

                                DisplaySelectedTags();

                            },
                            error: function (xhr, status, err) {
                                DisplayErrorMessage('Error', 'An error occurred during saving the tags.');
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

function RefreshTags() {

    if (currentEntity && currentEntity.Tags) {

        $.map(currentEntity.Tags, function (item) {

            $('input[value=' + item.Id + ']').prop('checked', true);

        });

    };

}

function DisplaySelectedTopics(data) {

        $('.tagselect').html('');


        // end meat
}

function CreateSingleSelectTags(tags, groupId, container) {

    var ul = $('<ul>');

    $.map(tags, function (tag) {

        var li = $('<li>');

        $('<input>').attr('type', 'radio').attr('name', groupId).val(tag.Id).appendTo($(li));
        $('<span>').text(tag.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));

    });

    $(ul).appendTo($(container));

}

function CreateMultiSelectTags(tags, container) {

    var ul = $('<ul>');

    $.map(tags, function (tag) {

        var li = $('<li>');

        $('<input>').attr('type', 'checkbox').val(tag.Id).appendTo($(li));
        $('<span>').text(tag.DisplayName).appendTo($(li));

        $(li).appendTo($(ul));
    });

    $(ul).appendTo($(container));
}

/* End NoteTopics*/