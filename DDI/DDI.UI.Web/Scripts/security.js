var currentGroup = null;

$(document).ready(function () {

    LoadGroupsGrid();
    LoadRolesTagBox();
    NewGroupModal();

    LoadUsersGrid();
    NewUserModal();

    PopulateDropDown('.ConstituentId', 'constituents', '', '');
    PopulateDropDown($('.user-DefaultBusinessUnitId'), 'businessunits', null);
    PopulateDropDown($('.user-GroupId'), 'groups', null);
    


});



/* GROUPS TAB */
function LoadGroupsGrid() {

    var columns = [
        { dataField: 'DisplayName', caption: 'Group Name' }
    ];

    CustomLoadGrid('groupgrid', '.groupstable', columns, 'groups', '', EditGroup, DeleteGroup, null); 

}


function LoadRolesTagBox() {

    DisplayTagBox('roles', 'rolestagbox', '.gp-rolesdropdowncontainer', null, false);

}


function NewGroupModal() {

    $('.newgroupmodal').click(function (e) {

        e.preventDefault();

        modal = $('.groupmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false,
            beforeClose: function (e) {

                $('.rolesmodal, .rolesgriditems').hide();

                $('.gp-Name').val("");

            }
        });

        $('.savegroupbutton').val('Next...');

        $('.savegroupbutton').unbind('click');

        $('.savegroupbutton').click(function (e) {

            $('.savegroupbutton').val('Save');

            var item = {
                Name: $('.gp-Name').val()
            };

            MakeServiceCall('POST', 'groups/', JSON.stringify(item), function (data) {

                id = data.Data.Id;

                LoadGroup(id);

                PopulateDropDown($('.user-GroupId'), 'groups', null);

                $('.hidgroupid').val(id);

                $('.rolesgriditems').show();

                $('.addrolesbutton').unbind('click');

                $('.addrolesbutton').click(function (e) {

                    $('.rolesmodal').show();

                    $('.saverolesbutton').click(function (e) {

                        AddRolesToGroup(id);


                    });

                    $('.savegroupbutton').unbind('click');

                    $('.savegroupbutton').click(function (e) {

                        var item = {
                            Name: $('.gp-Name').val()
                        };

                        MakeServiceCall('PATCH', 'groups/' + id, JSON.stringify(item), function (data) {

                            LoadGroup(id);

                            CloseModal(modal);

                            LoadGroupsGrid();

                            PopulateDropDown($('.user-GroupId'), 'groups', null);

                            $('.gp-Name').val("");

                        });

                    });

                    $('.cancelrolesmodal').click(function (e) {

                        e.preventDefault();

                        $('.rolesmodal').hide();

                        $('.rolestagbox').dxTagBox('instance').reset();


                    });
                });

                LoadGroupsGrid();
            });

        });


        $('.cancelgroupmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

            $('.rolesmodal, .rolesgriditems').hide();

            $('.rolestagbox').dxTagBox('instance').reset();

            $('.gp-Name').val("");

        });

    });

}

function EditGroup(id) {
    
    LoadGroup(id);

    $('.hidgroupid').val(id);

    modal = $('.groupmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false,
        beforeClose: function (e) {

            $('.rolesmodal, .rolesgriditems').hide();

            $('.gp-Name').val("");

        }
    });

    $('.rolesgriditems').show();

    $('.groupmodal').show();

    $('.savegroupbutton').val('Save');
    
    $('.savegroupbutton').unbind('click');

    $('.savegroupbutton').click(function (e) {

        var item = {
            Name: $('.gp-Name').val()
        };

        MakeServiceCall('PATCH', 'groups/' + id, JSON.stringify(item), function (data) {

            LoadGroup(id);

            CloseModal(modal);

            LoadGroupsGrid();

            PopulateDropDown($('.user-GroupId'), 'groups', null);

            $('.rolestagbox').dxTagBox('instance').reset();

            $('.gp-Name').val("");

        });

    });

    $('.addrolesbutton').unbind('click');

    $('.addrolesbutton').click(function (e) {

        $('.rolesmodal').show();

        $('.saverolesbutton').click(function (e) {

            AddRolesToGroup(id);


        });

        $('.cancelrolesmodal').click(function (e) {

            e.preventDefault();

            $('.rolesmodal').hide();

            $('.rolestagbox').dxTagBox('instance').reset();
            

        });

    });

    $('.cancelgroupmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        $('.rolesmodal, .rolesgriditems').hide();

        $('.rolestagbox').dxTagBox('instance').reset();

        $('.gp-Name').val("");

    });

}


function DeleteGroup(id) {

    MakeServiceCall('DELETE', 'groups/' + id, null, function (data) {

        DisplaySuccessMessage('Success', 'Group deleted successfully.');

        LoadGroupsGrid();

    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);


    });


}

function LoadGroup(id) {

    var columns = [
        { dataField: 'DisplayName', caption: 'Roles' }
    ];

    MakeServiceCall('GET', 'groups/' + id, null, function (data) {

        $('.gp-Name').val(data.Data.DisplayName);

    });

    CustomLoadGrid('rolesgrid', '.rolesgridcontainer', columns, 'group/' + id + '/roles', '', null, DeleteRole, null); 

}

function DeleteRole(role) {

    var groupid = $('.hidgroupid').val();
    
    MakeServiceCall('PATCH', '/groups/remove/' + groupid + '/role/' + role, null, function (data) {

        DisplaySuccessMessage('Success', 'Role successfully removed from Group.');

        LoadGroup(groupid);


    });

}

function AddRolesToGroup(id) {

    var values = $('.rolestagbox').dxTagBox('instance').option('values');
    if (values != null)
    {
        var items = "{ item: " + JSON.stringify(values) + " }";

        MakeServiceCall('POST', 'groups/' + id + '/roles/', items, function (data) {

            DisplaySuccessMessage('Success', 'Roles successfully added to Group.');

            $('.rolesmodal').hide();

            $('.rolestagbox').dxTagBox('instance').reset();
            EditGroup(id);

        }, function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);

        });
    }
}


/* END GROUPS TAB */


/* USERS TAB */
function LoadUsersGrid() {

    var columns = [
        { dataField: 'FullName', caption: 'Full Name' },
        { dataField: 'Email', caption: 'Email Address' },
        { caption: 'Active', cellTemplate: function (container, options) {
                var type = 'Yes';

                if (options.data.IsActive != '1') {
                    type = 'No';
                }
                $('<label>').text(type).appendTo(container);
            }
        }
    ];

    CustomLoadGrid('usergrid', 'usersgridcontainer', columns, 'users?fields=all', null, EditUser, null);
}

function NewUserModal() {

    $('.newusermodal').click(function (e) {

        e.preventDefault();

        $('.user-editonly1').hide();
        $('.user-editonly2').hide();

        modal = $('.usermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false,
            beforeClose: function (e) {}
        });

        
        $('.submituser').val('Next...');

        $('.submituser').unbind('click');

        $('.submituser').click(function (e) {

            var item = {
                UserName: $(modal).find('.user-UserName').val(),
                Email: $(modal).find('.user-Email').val()
            }

            
            MakeServiceCall('POST', 'users', JSON.stringify(item), function (data) {

                CloseModal(modal);
                EditUser(data.Data.Id);

            }, null);

        });


        $('.cancelusermodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);
        });
    });
}


// USER SETTINGS 
function EditUser(id) {


    LoadUser(id);


    modal = $('.usermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });


    $('.user-editonly1').show();
    $('.user-editonly2').show();

    $('.cancelusermodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        LoadUsersGrid()

    });

    $('submituser').val('Save');

    $('.submituser').unbind('click');

    $('.submituser').click(function () {

        var buIds = $('.tagBoxBusinessUnits').dxTagBox('instance').option('values');

        var item = {
            FullName: $(modal).find('.user-FullName').val(),
            UserName: $(modal).find('.user-UserName').val(),
            Email: $(modal).find('.user-Email').val(),
            DefaultBusinessUnitId: $(modal).find('.user-DefaultBusinessUnitId').val(),
            ConstituentId: $(modal).find('.rs-Constituent1Id').val(),
            GroupId: $(modal).find('.user-GroupId').val(),
            BusinessUnitIds: buIds,
            IsActive: true
        }

        MakeServiceCall('PATCH', 'users/' + id, JSON.stringify(item), function (data) {

            if (data.Data) {

                DisplaySuccessMessage('Success', 'User saved successfully.');
                CloseModal(modal);
                LoadUsersGrid();
            }

        }, function (xhr, status, err) {

            DisplayErrorMessage('Error', 'An error occurred while saving the User.');

        });
        
    });

}


function LoadUser(id) {

    MakeServiceCall('GET', 'users/' + id, null, function (data) {

        if (data.Data) {
            if (data && data.Data && data.IsSuccessful) {

                $(modal).find('.user-Id').val(data.Data.Id);
                $(modal).find('.user-FullName').val(data.Data.FullName);
                $(modal).find('.user-UserName').val(data.Data.UserName);
                $(modal).find('.user-Email').val(data.Data.Email);
                $(modal).find('.user-DefaultBusinessUnitId').val(data.Data.DefaultBusinessUnitId);
                $(modal).find('.rs-Constituent1Id').val(data.Data.ConstituentId);
                $(modal).find('.user-IsActive').prop('checked', data.Data.IsActive);

                if (data.Data.Constituent != null) {
                    var constituentLabel = data.Data.Constituent.ConstituentNumber + ": " + data.Data.Constituent.Name + ", " + data.Data.Constituent.PrimaryAddress;
                    $(modal).find('.rs-Constituent1Information').val(constituentLabel);
                }
                else {
                    $(modal).find('.rs-Constituent1Information').empty();
                }

                LoadBusinessUnits('tagBoxBusinessUnits', 'user-BusinessUnits', 'businessunits', '/users/' + data.Data.UserName + '/businessunit');

                if (data.Data.Groups.length > 0) {
                    $(data.Data.Groups).each(function (i, group) {

                        $(modal).find('.user-GroupId').val(group.Id);
                        
                    });
                }
            }
        }

    }, null);

}

function LoadBusinessUnits(tagBox, container, routeForAllOptions, routeForSelectedOptions, disabled) {
    if ($.type(container) === "string" && container.indexOf('.') != 0)
        container = '.' + container;

    $(container).html('');

    var selectedItems = [];

    MakeServiceCall('GET', routeForSelectedOptions, null, function (data) {
        data.Data.forEach(function (item) {
            selectedItems.push(item.Id);
        });
        DisplaySelectedBusinessUnits(routeForAllOptions, tagBox, container, selectedItems, disabled);
    }, null);

}

function DisplaySelectedBusinessUnits(routeForAllOptions, tagBox, container, selectedItems, disabled) {

    var tagBoxControl = $('<div>').addClass(tagBox); // will probably have to apply this to the tagbox itself...

    MakeServiceCall('GET', routeForAllOptions, null, function (data) {
        $(tagBoxControl).dxTagBox({
            items: data.Data,
            value: selectedItems,
            displayExpr: 'DisplayName',
            valueExpr: 'Id',
            showClearButton: true,
            disabled: disabled,
            height: data.length,
            scrollbar: true

        });

        $(tagBoxControl).appendTo(container);
    }, null);

}


/* END USERS TAB */





