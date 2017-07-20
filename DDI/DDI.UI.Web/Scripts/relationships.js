/* Relationships Tab */

function LoadRelationshipsQuickView() {
    var quickviewdata;

    MakeServiceCall('GET', 'constituents/' + currentEntity.Id + '/relationships', null, function (data) {

        var formattedData = $('<ul>').addClass('relationshipQuickViewData');

        $.map(data.Data, function (item) {

            if (item.RelationshipType.RelationshipCategory.IsShownInQuickView === true) {

                var li = $('<li>');

                var rowText = item.RelationshipType.Name + ': ';
                var link = $('<a>').attr('href', '#').text(item.Constituent1.FormattedName).click(function (e) {
                    e.preventDefault();
                    RelationshipLinkClicked(item.Constituent1.Id)
                });

                $(li).html(rowText);
                $(link).appendTo($(li));

                $(li).appendTo($(formattedData));

            }

        });

        $('.relationshipsQuickView').empty();

        $(formattedData).appendTo($('.relationshipsQuickView'));

    }, null);

}

function RelationshipLinkClicked(id) {

    sessionStorage.setItem('constituentid', id);

    if (sessionStorage.getItem('constituentid')) {
        $('.hidconstituentid').val(sessionStorage.getItem('constituentid'));
    }

    GetConstituentData($('.hidconstituentid').val());

}

function LoadAttachmentsTab(id) {

    container = $('.attachments');
    container.empty();
    LoadAttachments(container, "contituentsgrid", id, null, NoteEntity[0]);
}

function LoadRelationshipsTab() {

    var columns = [
        { dataField: 'RelationshipType.RelationshipCategory.Name', caption: 'Category', groupIndex: 0 },
        { dataField: 'RelationshipType.Name', caption: 'Relationship', width: '30%' },
        { dataField: 'Constituent1.ConstituentNumber', caption: 'ID', width: '20%' },
        { dataField: 'Constituent1.FormattedName', caption: 'Name', width: '50%' }
    ];

    // CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete)
    CustomLoadGrid('relationshipsgrid', 'relationshipstable', columns, 'constituents/' + currentEntity.Id + '/relationships', null, EditRelationship, null, NewRelationshipModal);
}

function LoadRelationshipsGrid() {

    LoadRelationshipsData();
}

function EditRelationship(id) {
    var constituentId = $('.hidconstituentid').val();

    EditEntity('constituents/' + constituentId + '/relationships', 'rs-', id, '.relationshipmodal', 500, LoadRelationshipsTab);
}

function NewRelationshipModal() {
    var modalLinkClass = 'rs-newmodallink';
    var constituentId = $('.hidconstituentid').val();

    $('.' + modalLinkClass).remove();

    var link = $('<a>')
        .attr('href', '#')
        .addClass('newmodallink')
        .addClass(modalLinkClass)
        .text('New Item')
        .click(function (e) {
            e.preventDefault();

            NewEntityModal('constituents/' + constituentId + '/relationships', 'rs-', '.relationshipmodal', 250, LoadRelationshipsTab);

            PrePopulateNewRelationshipModal(modal);

        });
    $('.relationshipstable').prepend($(link));

}

function PrePopulateNewRelationshipModal(modal) {
    $(modal).find('.rs-Constituent2Id').val(currentEntity.Id);
    $(modal).find('.rs-Constituent2Information').val(currentEntity.FormattedName);
}

function LoadRelationshipData(data, modal) {

    $(modal).find('.hidrelationshipid').val(data.Data.Id);
    $(modal).find('.hidrelationshipisswapped').val(data.Data.IsSwapped);
    $(modal).find('.hidconstituentlookupid').val(data.Data.Constituent1.Id);
    $(modal).find('.RelationshipTypeId').val(data.Data.RelationshipType.Id);

}
/* End Relationships Tab */