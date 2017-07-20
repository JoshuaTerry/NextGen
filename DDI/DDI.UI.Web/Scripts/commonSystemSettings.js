/* COMMON SETTINGS */
function LoadAlternateIDTypesSectionSettings() {



}

function LoadBusinessDateSectionSettings() {



}

function LoadCalendarDatesSectionSettings() {



}

function LoadDocumentTypesSectionSettings() {



}

function LoadCommonHomeSreenSectionSettings() {



}

function LoadMergeFormSystemSectionSettings() {



}

function LoadNoteSectionSettings() {

    var accordion = $('<div>').addClass('accordions');
    var noteCodes = $('<div>').addClass('noteCodecontainer');
    var noteCategories = $('<div>').addClass('noteCategorycontainer');
    var noteTopics = $('<div>').addClass('noteTopiccontainer');

    var header = $('<h1>').text('Note Code').appendTo($(accordion));
    $(noteCodes).appendTo($(accordion));

    var noteCodecolumns = [

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteCodecontainer', 'noteCodegrid', noteCodecolumns, 'notecodes', 'notecodes', null, 'noteCode-',
        '.noteCodemodal', '.noteCodemodal', 250, true, false, false, null);

    header = $('<h1>').text('Note Category').appendTo($(accordion));
    $(noteCategories).appendTo($(accordion));

    var noteCategorycolumns = [

        { dataField: 'Label', caption: 'Label' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteCategorycontainer', 'noteCategorygrid', noteCategorycolumns, 'notecategories?fields=all', 'notecategories', null, 'noteCategory-',
        '.noteCategorymodal', '.noteCategorymodal', 250, true, false, false, null);


    header = $('<h1>').text('Topic').appendTo($(accordion));
    $(noteTopics).appendTo($(accordion));

    var noteTopiccolumns = [

        { dataField: 'Code', caption: 'Code' },
        { dataField: 'Name', caption: 'Description' },
        { dataField: 'IsActive', caption: 'Active' }
    ];
    LoadGrid('.noteTopiccontainer', 'noteTopicgrid', noteTopiccolumns, 'notetopics?fields=all', 'notetopics', null, 'noteTopic-',
        '.noteTopicmodal', '.noteTopicmodal', 250, true, false, false, null);

    $(accordion).appendTo($('.gridcontainer'));

    LoadAccordions();

}

function LoadStatusCodesSectionSettings() {



}

function LoadTransactionCodesSectionSettings() {



}
/* END COMMON SETTINGS */