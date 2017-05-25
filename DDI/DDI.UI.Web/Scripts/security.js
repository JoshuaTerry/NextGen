var currentGroup = null;

$(document).ready(function () {

   // SetupNewUserModal();

    LoadGroupsGrid();

    NewGroupModal();

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
        { dataField: 'Id', width: '0px' },
        { dataField: 'DisplayName', caption: 'Group Name' }
    ];

    MakeServiceCall('GET', 'groups/', null, function (data) {

        //LoadGridWithData(grid, container, columns, route, selected, editMethod, deleteMethod, data, oncomplete);
        LoadGridWithData('groupgrid', '.groupstable', columns, null, null, EditGroup, DeleteGroup, data, null);
        //maybe define a currentGroup variable and set the id when edit is clicked?

    }, function (xhr, status, err) {

        DisplayErrorMessage('Error', xhr.responseJSON.ExceptionMessage);


        });

    //function CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {
    //CustomLoadGrid('groupgrid', '.groupstable', columns, 'groups', ''
    //    , EditGroup, null, null); 



    //function DisplayTagBox(routeForAllOptions, tagBox, container, selectedItems) 

    DisplayTagBox('roles', 'rolestagbox', '.gp-rolesdropdowncontainer', null, false);

}

function NewGroupModal() {

    $('.newgroupmodal').click(function (e) {

        e.preventDefault();

        modal = $('.groupmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false
        });

        $('.cancelgroupmodal').click(function (e) {

            e.preventDefault();

            CloseModal(modal);

            $('.rolestagbox').dxTagBox('instance').reset();

        });

    });

}

function EditGroup(id) {
    
    // CustomLoadGrid(grid, container, columns, route, selected, editMethod, deleteMethod, oncomplete) {

    LoadGroup(id);

    modal = $('.groupmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    $('.groupmodal').show();

    $('.addrolesbutton').click(function (e) {

        var roleModal = $('.rolesmodal').dialog({
            closeOnEscape: false,
            modal: true,
            width: 400,
            resizable: false
        });

        $('.rolesmodal').show();

        $('.saverolesbutton').click(function (e) {

            AddRolesToGroup(id);

        });

    });

    $('.cancelgroupmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        $('.rolestagbox').dxTagBox('instance').reset();

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

    CustomLoadGrid('rolesgrid', '.rolesgridcontainer', columns, 'group/' + id + '/roles', '', null, DeleteRole, null); // will need delete method, but no edit method (probably)

}

function DeleteRole() {
    // need route to remove role from group

}

function RolesModal() {


}

function AddRolesToGroup(id) {

    var items = $('.rolestagbox').dxTagBox('instance').option('values');
    var doop = JSON.stringify(items);
    var pood = "Items: " + doop

    MakeServiceCall('POST', 'groups/' + id + '/roles/', pood, function (data) {

        DisplaySuccessMessage('Success', 'Roles successfully added to Group.');

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





