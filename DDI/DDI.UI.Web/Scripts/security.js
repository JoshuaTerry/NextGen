var currentGroup = null;

$(document).ready(function () {

   // SetupNewUserModal();

    LoadGroupsGrid();
    LoadRolesTagBox();


    NewGroupModal();
    PopulateDropDown('.ConstituentId', 'constituents', '', '');
    LoadUsersGrid();

});

function SetupNewUserModal() {

    $('.addnewuser').click(function (e) {

        e.preventDefault();

        $('.newusermodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            height: 225,
            resizable: false
        });

        $('.submitnewuser').click(function () {

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

        });

    });

    $('.addrolesbutton').unbind('click');

    $('.addrolesbutton').click(function (e) {

        $('.rolesmodal').show();

        $('.saverolesbutton').click(function (e) {

            AddRolesToGroup(id);

            $('.rolesmodal').hide();

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
        { dataField: 'DisplayName', caption: 'User Name' },
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
   // PopulateBusinessUnits();
    LoadGrid('.usersgridcontainer', 'usergrid', columns, 'users', 'users'
       , null, 'user-', '.usermodal', '.usermodal', 250, false, true, false, null);
   

}

function LoadBusinessUnits()
{
   
}

function CreateBusinessUnitCheckBoxes(data)
{


}

function LoadUsersGroupsGrid() {

    var columns = [
        { dataField: 'UserId', caption: 'User ID' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('usersgroupsgrid', 'usergroupsgridcontainer', columns, 'usergroups');

}

function DisplayUserInfo(id) {

    $.ajax({
        url: WEB_API_ADDRESS + 'users/' + id,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function (data) {

            if (IsSuccessful) {

                $('.userid').val(data.Data.UserId);
                $('.username').val(data.Data.Name);
                $('.useremail').val(data.Data.Email);

                if (data.Data.IsActive && data.Data.IsActive == 1) {
                    $('.userstatus').prop('checked', true);
                }
                else {
                    $('.userstatus').prop('checked', false);
                }
                
            }

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    })

}

function AddUsersToRoles(user, roles) {

    var data = {
        Email: user,
        Roles: roles
    }

    $.ajax({
        type: 'POST',
        url: WEB_API_ADDRESS + 'users/roles/add',
        data: data,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        headers: GetApiHeaders(),
        success: function () {

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);
        }
    });

}
/* END USERS TAB */





