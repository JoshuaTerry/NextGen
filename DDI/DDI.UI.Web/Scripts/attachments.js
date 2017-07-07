function LoadAttachments(container,gridcontainername, entityId, noteId, entityType)
{
    CreateAddFileUploadLink(container,gridcontainername, entityId, noteId, entityType);

    $('<div>').addClass(gridcontainername).appendTo($(container));

    var route = "";

    if (noteId != null)
    {
        route = 'attachments/notes/' + noteId ;
    }
    else
    {
        route = 'attachments/entity/' + entityId ;
    }


    var columns = [

        { dataField: 'Title', caption: 'Title', sortOrder: 'asc', sortIndex: 0 },
        { dataField: 'CreatedBy', caption: 'Created By'},
        {
            dataField: 'CreatedOn', caption: 'Date', dataType: 'date', width: '100px'
        },
        {
            dataField: 'File.Name', caption: 'Name'
        },
        {
            dataField: 'File.Extension', caption: 'Type', alignment: 'center', width: '50px'
        },
        {
            dataField: 'File.Size', caption: 'Size', alignment: 'center', width: '100px'
        },
        {
            width: '85px',
            alignment: 'left',
            cellTemplate: function (container, options) {
                $('<a/>')
                    .addClass('actionbuttons')
                    .addClass('viewbutton')
                    .addClass('viewlink')
                    .attr('href','../../Pages/Common/ViewDocument.aspx?Id=' + options.data.FileId)
                    .appendTo(container);
                $('<a/>')
                    .addClass('actionbuttons')
                    .addClass('deletebutton')
                    .addClass('deleteLink')
                    .click(function (e) {
                        e.preventDefault();

                        DeleteAttachment(container.parent(), options.data.Id, gridcontainername, entityId, noteId, entityType);
                    })
                    .appendTo(container);

                if (noteId == null) {

                    if (options.data.NoteId != null) {
                        $('<a/>')
                            .addClass('actionbuttons')
                            .addClass('notebutton')
                            .addClass('notelink')
                            .click(function (e) {
                                e.preventDefault();

                                ViewNoteDetail(container, gridcontainername, entityId, options.data.NoteId, entityType);
                            })
                            .appendTo(container);
                    }

                }

            }
        }
    ];


    CustomLoadGrid(container, gridcontainername, columns, route , false, false, false, false );
}

function CreateAddFileUploadLink(container, gridcontainername, entityId, noteId, entityType) {
    var button = $('<div>');
    $('<a>').attr('href', '#').attr('entitytype',entityType).text('File Upload').addClass(gridcontainername + "uploadbutton").addClass('newmodallink').appendTo(button);
    button.appendTo(container);
   

    $("."+ gridcontainername + "uploadbutton").click(function (e) {
        UploadFiles(function (data) {

            var attachment = {
                Title: $('.fileuploadmodal').find(".fu-description").val(),
                FileId: data.Id,
                EntityType: $("." + gridcontainername + "uploadbutton").attr("entitytype") ,
                ParentEntityId: entityId,
                CreatedBy: data.CreatedBy,
                CreatedOn: data.CreatedOn,
                NoteId: noteId
            }

            MakeServiceCall("Post", 'attachments', JSON.stringify(attachment), function(data){

                CloseModal($('.fileuploadmodal'))
                $(container).empty();
                LoadAttachments(container, gridcontainername, entityId, noteId, entityType)
                },
                null
            )
        }
        );
    });
}



function DeleteAttachment(container, Id,gridcontainername, entityId, noteId, entityType) {
    var okToDelete = confirm("Are you sure you want to delete the Attachment?");
    if (okToDelete === true) {
        // delete the entity
        MakeServiceCall("Delete", "attachments/"+Id, null, function (data) {
          
                DisplaySuccessMessage('Success', 'This item was deleted.');
                $(container).empty();
                LoadAttachments(container, gridcontainername, entityId, noteId, entityType)

        }, null);
    };
}

function ViewNoteDetail(container, gridcontainername, entityId, noteId, entityType)
{
    LoadNotesModal(noteId, function (e) {

        $(container).empty();
        LoadAttachments(container, gridcontainername, entityId, null, entityType)
    },
        function (e) {
            $(container).empty();
            LoadAttachments(container, gridcontainername, entityId, null, entityType)
        }
    );
}

function ReloadAttachmentsGrid(container, gridcontainername, entityId, noteId, entityType)
{
    $(container).empty();
    LoadAttachments(container, gridcontainername, entityId, noteId, entityType)
}

