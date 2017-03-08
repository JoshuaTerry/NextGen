$(document).ready(function () {

    DisplaySelectedTags(); // may not actually want? 

    LoadNoteDetailsGrid();

    NewNoteDetailsModal();

});

/* Notes Tab */
function LoadNoteDetailsTagBox() {

    $.ajax({
        type: 'GET',
        url: WEB_API_ADDRESS + 'notetopics',
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function (data) {

            CreateMultiSelectTags(data.Data, '.tagdropdowndivcontainer');
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
        PopulateDropDown('.nd-Topics', 'notetopics/', '', '');
        SetupNoteTopicSelect();

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

    SetupNoteTopicSelect();

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

            LoadTagSelector(data.Data); // this will start to load the constituents, you can see if you set a breakpoint

            PopulateDropDown('.nd-Category', 'notecategories', '', '', data.Data.CategoryId);
            PopulateDropDown('.nd-Topics', 'notetopics', '', '', data.Data.NoteTopicId);

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
    $('.noteTopicSelectImage').click(function () {
        LoadNoteDetailsTagBox();
    });
}
/* End Notes Tab */

/* NoteTopics */


/* End NoteTopics*/