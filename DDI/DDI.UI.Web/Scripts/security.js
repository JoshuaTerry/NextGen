var currentGroup = null;

$(document).ready(function () {

    //SetupNewUserModal();

    LoadGroupsGrid();
    LoadRolesTagBox();


    NewGroupModal();
    PopulateDropDown('.ConstituentId', 'constituents', '', '');
    LoadUsersGrid();
    NewUserModal();

});

function SetupNewUserModal() {

    $('.addnewuser').click(function (e) {

        e.preventDefault();

        $('.usermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            height: 225,
            resizable: false
        });

        $('.submituser').click(function () {

            var model = {
                Email: $('.newusername').val(),
                Password: $('.newpassword').val(),
                ConfirmPassword: $('.newconfirmpassword').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'users',
                data: model,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                headers: GetApiHeaders(),
                success: function () {

                    AddUsersToRoles($('.newusername').val(), ['Administrators', 'Users']);

                    location.href = "/Login.aspx";

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });

        });

    });

}

function LoadSecuritySettingsGrid() {

    var columns = [
        { dataField: 'UserId', caption: 'User ID' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('securitysettingsgrid', 'securitysettingsgridcontainer', columns, 'groupsettings');

}

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

        $('.savegroupbutton').unbind('click');

        $('.savegroupbutton').click(function (e) {

            var item = {
                Name: $('.gp-Name').val()
            };

            MakeServiceCall('POST', 'groups/', JSON.stringify(item), function (data) {

                id = data.Data.Id;

                LoadGroup(id);

                $('.hidgroupid').val(id);

                $('.rolesgriditems').show();

                $('.addrolesbutton').unbind('click');

                $('.addrolesbutton').click(function (e) {

                    $('.rolesmodal').show();

                    $('.saverolesbutton').click(function (e) {

                        AddRolesToGroup(id);

                        $('.rolesmodal').hide();

                        LoadGroup(id);

                        $('.rolestagbox').dxTagBox('instance').reset();


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

    $('.savegroupbutton').unbind('click');

    $('.savegroupbutton').click(function (e) {

        var item = {
            Name: $('.gp-Name').val()
        };

        MakeServiceCall('PATCH', 'groups/' + id, JSON.stringify(item), function (data) {

            LoadGroup(id);

            CloseModal(modal);

            LoadGroupsGrid();

            $('.rolestagbox').dxTagBox('instance').reset();

            $('.gp-Name').val("");

        });

    });

    $('.addrolesbutton').unbind('click');

    $('.addrolesbutton').click(function (e) {

        $('.rolesmodal').show();

        $('.saverolesbutton').click(function (e) {

            AddRolesToGroup(id);

            $('.rolesmodal').hide();

            $('.rolestagbox').dxTagBox('instance').reset();

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

function SaveGroup(id) {

    MakeServiceCall('PATCH', 'groups/' + id, function () {
        // save group title

    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);


    });

    MakeServiceCall('PATCH', 'groups/' + id, function () {
        // save roles to group

    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);


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

    MakeServiceCall('PATCH', '/groups/remove/' + groupid + '/role', null, function (data) {

        DisplaySuccessMessage('Success', 'Role successfully removed from Group.');

        LoadGroup(id);


    });

}

function AddRolesToGroup(id) {

    var values = $('.rolestagbox').dxTagBox('instance').option('values');
    var items = "{ item: " + JSON.stringify(values) + " }";

    MakeServiceCall('POST', 'groups/' + id + '/roles/', items, function (data) {

        DisplaySuccessMessage('Success', 'Roles successfully added to Group.');

        $('.rolesmodal').hide();

        LoadGroup(id)

    }, function (xhr, status, err) {
        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);

    });
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

    PopulateDropDown($('.user-DefaultBusinessUnitId'), 'businessunits', null);
    PopulateDropDown($('.user-GroupId'), 'groups', null);

    //LoadGrid('.gridcontainer', 'prefixgrid', prefixcolumns, 'prefixes?fields=all', 'prefixes', null, 'prefix-',
    //          '.prefixmodal', '.prefixmodal', 250, true, false, false, null);
    //LoadGrid('.usersgridcontainer', 'usergrid', columns, 'users', 'users', null, 'user-',
      //  '.usermodal', '.usermodal', 250, true, true, true, null); // DELETE after following path to new modal link stuff
    CustomLoadGrid('usergrid', 'usersgridcontainer', columns, 'users?fields=all', null, EditUser, null);
    //NewModalLink('.usersgridcontainer', 'users', 'user-', '.usermodal', 250, '');
    //NewModalLink('.usergrid', 'users', 'user-', '.usermodal', 250, '');
    //NewModalLink('.tab-users', 'users', 'user-', '.usermodal', 250, '');
    //NewModalLink('.usersgridcontainer', 'users', 'user-', '.usermodal', 250, '');
    //NewModalLink(container, saveRoute, prefix, newModalClass, modalWidth, refreshGrid);


    //var modalLinkClass = 'user-newmodallink';

    //$('.' + modalLinkClass).remove();

    //var link = $('<a>')
    //        .attr('href', '#')
    //        .addClass('newmodallink')
    //        .addClass(modalLinkClass)
    //        .text('New Item')
    //        .click(function (e) {
    //            e.preventDefault();

    //            NewEntityModal('users', 'user-', '.usermodal', 250, '');

    //        });
    //$('.usersgridcontainer').prepend($(link));
    

}

function NewUserModal() {

    $('.newusermodal').click(function (e) {

        e.preventDefault();


        modal = $('.usermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false,
            beforeClose: function (e) {

                //$('.rolesmodal, .rolesgriditems').hide();

            }
        });

        $('.submituser').unbind('click');

        $('.submituser').click(function (e) {

            var item = {
                Email: $(modal).find('.user-Email').val()
            }

            $.ajax({
                type: 'POST',
                url: WEB_API_ADDRESS + 'users',
                data: item,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                headers: GetApiHeaders(),
                success: function (data) {

                    //AddUsersToRoles($('.newusername').val(), ['Administrators', 'Users']);

                    DisplaySuccessMessage('Success', 'User saved successfully.');

                    CloseModal(modal);
                    EditUser(data.Id);
                            
                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
                }
            });

        });


        $('.cancelusermodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

            //$('.rolesmodal, .rolesgriditems').hide();

            //$('.rolestagbox').dxTagBox('instance').reset();

        });

    });

}

function DisplayUserBusinessUnits(businessUnits) {

    $(modal).find('.tagselect').empty();
    $(modal).find('.user-BusinessUnits').empty();

    $(businessUnits).each(function (i, businessUnit) {

        var id = businessUnit.Id;
        var name = businessUnit.Name;

        var t = $('<div>').addClass('dx-tag-content').attr('id', id).appendTo($('.tagselect'));
        $('<span>').text(name).appendTo($(t));
        $('<div>').addClass('dx-tag-remove-button')
            .click(function () {
                MakeServiceCall('GET', 'DELETE' + $(modal).find('.user-Id').val() + '/businessunits/' + businessUnit.Id, null, function (data) {

                    if (data.Data) {
                        DisplaySuccessMessage('Success', 'Business Unit was deleted successfully.');
                        CloseModal(modal);
                        EditConstituentType($(modal).find('.user-Id').val());
                    }

                }, null)
            })
            .appendTo($(t));

    });


}

function LoadUserBusinessUnitSelector(id, container) {

    $('.tagselect').each(function () {

        var img = $('.tagSelectImage');
        var userId = id;

        if (img.length === 0) {
            img = $('<div>').addClass('tagSelectImage');
        }

        $(img).click(function () {

            tagmodal = $('.tagselectmodal').dialog({
                closeOnEscape: false,
                modal: true,
                width: 450,
                resizable: false
            });

            LoadAvailableBusinessUnits(tagmodal, true);
            //LoadTagBoxes(tagBox, container, routeForAllOptions, routeForSelectedOptions)
            //LoadTagBoxes('tagBoxBusinessUnits', 'user-BusinessUnits', 'businessunits', '/users/' + id + '/businessunit');

            $('.cancelmodal').click(function (e) {

                e.preventDefault();

                CloseModal(tagmodal);
                CloseModal(modal);
                EditUser(userId);

            });

            $('.saveselectedbusinessunits').unbind('click');

            $('.saveselectedbusinessunits').click(function () {

                var buIds = [];

                $('.tagselectgridcontainer').find('input').each(function (index, value) {

                    if ($(value).prop('checked')) {
                        buIds.push($(value).val());
                    }
                });

                MakeServiceCall('POST', 'users/' + userId + '/businessunits', JSON.stringify({ businessUnits: buIds }), function (data) {

                    if (data.Data) {
                        DisplaySuccessMessage('Success', 'BusinessUnits saved successfully.');
                        CloseModal(tagmodal);
                        EditUser(userId);
                    }

                }, null);

            });
        });
        $(this).after($(img));
    });
}

function LoadAvailableBusinessUnits(container, isCategorySpecific) {

    var route = 'businessunits';

    MakeServiceCall('GET', route, null, function (data) {

        if (data.Data) {

            $(container).find('.tagselectgridcontainer').html('');

            $.map(data.Data, function (group) {

                var header = $('<div>').addClass('tagSelectorHeader').text(group.DisplayName);
                var tagsContainer = $('<div>').addClass('tagSelectorContainer');

                if (group.Tags) {

                    switch (group.TagSelectionType) {

                        case 0:
                            CreateMultiSelectTags(group.Tags, tagsContainer);
                            break;
                        case 1:
                            CreateSingleSelectTags(group.Tags, group.Id, tagsContainer);
                            break;
                        default:
                            break;
                    }

                }

                $(header).appendTo($(container).find('.tagselectgridcontainer'));
                $(tagsContainer).appendTo($(container).find('.tagselectgridcontainer'));

            });

            RefreshBusinessUnits();

        }

    }, null);

}

function RefreshBusinessUnits() {

    if (currentEntity && currentEntity.BusinessUnits) {

        $.map(currentEntity.BusinessUnits, function (item) {

            $('input[value=' + item.Id + ']').prop('checked', true);

        });

    };

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

    $('.submituser').unbind('click');

    $('.submituser').click(function () {

        // Make the call to add the group to the user here
        // if the call is successful then proceed with saving the rest of user data
        // otherwise, display an error
        // GroupId: $(modal).find('.user-GroupId').val(),


        var item = {
            FullName: $(modal).find('.user-FullName').val(),
            UserName: $(modal).find('.user-UserName').val(),
            Email: $(modal).find('.user-Email').val(),
            DefaultBusinessUnitId: $(modal).find('.user-DefaultBusinessUnitId').val(),
            ConstituentId: $(modal).find('.user-Constituent1Id').val(),
            GroupId: $(modal).find('.user-GroupId').val(),
            IsActive: true
        }

        MakeServiceCall('PATCH', 'users/' + id, JSON.stringify(item), function (data) {

            if (data.Data) {

                //if (item.DefaultBusinessUnitId != null && item.DefaultBusinessUnitId != '') {

                //    MakeServiceCall('PATCH', 'users/' + id + '/businessunit/' + item.DefaultBusinessUnitId, JSON.stringify(item), null, null);
                //}

                //var group = $(modal).find('.user-Constituent1Id').val();
                //if (group != null && group != '') {
                //    MakeServiceCall ('PATCH', 'users/' + id + '/group/' + group, '', null, null);
                //}

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
                $(modal).find('.user-Constituent1Id').val(data.Data.ConstituentId);
                $(modal).find('.user-IsActive').prop('checked', data.Data.IsActive);
                
                //LoadTagBoxes('tagBoxBusinessUnits', 'user-BusinessUnits', 'businessunits', '/users/' + id + '/businessunit');
                LoadUserBusinessUnitSelector(data.Data.Id, $('.user-BusinessUnits'));

                if (data.Data.BusinessUnits.length > 0) {
                    DisplayUserBusinessUnits($(data.Data.BusinessUnits));
                }
                else {
                    $(modal).find('.user-BusinessUnits').empty();
                }
            }
        }

    }, null);

}

//function LoadBusinessUnits()
//{
   
//}

//function CreateBusinessUnitCheckBoxes(data)
//{


//}

//function LoadUsersGroupsGrid() {

//    var columns = [
//        { dataField: 'UserId', caption: 'User ID' },
//        { dataField: 'Name', caption: 'Name' }
//    ];

//    LoadGrid('usersgroupsgrid', 'usergroupsgridcontainer', columns, 'usergroups');

//}

//function DisplayUserInfo(id) {

//    $.ajax({
//        url: WEB_API_ADDRESS + 'users/' + id,
//        method: 'GET',
//        contentType: 'application/json; charset-utf-8',
//        dataType: 'json',
//        crossDomain: true,
//        headers: GetApiHeaders(),
//        success: function (data) {

//            if (IsSuccessful) {

//                $('.userid').val(data.Data.UserId);
//                $('.username').val(data.Data.Name);
//                $('.useremail').val(data.Data.Email);

//                if (data.Data.IsActive && data.Data.IsActive == 1) {
//                    $('.userstatus').prop('checked', true);
//                }
//                else {
//                    $('.userstatus').prop('checked', false);
//                }
                
//            }

//        },
//        error: function (xhr, status, err) {
//            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
//        }
//    })

//}

//function AddUsersToRoles(user, roles) {

//    var data = {
//        Email: user,
//        Roles: roles
//    }

//    $.ajax({
//        type: 'POST',
//        url: WEB_API_ADDRESS + 'users/roles/add',
//        data: data,
//        contentType: 'application/x-www-form-urlencoded',
//        crossDomain: true,
//        headers: GetApiHeaders(),
//        success: function () {

//        },
//        error: function (xhr, status, err) {
//            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
//        }
//    });

//}
/* END USERS TAB */





