
$(document).ready(function () {

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
                url: WEB_API_ADDRESS + 'Register',
                data: model,
                contentType: 'application/x-www-form-urlencoded',
                crossDomain: true,
                success: function () {

                    AddUsersToRoles($('.newusername').val(), ['Administrators', 'Users']);

                    location.href = "/Login.aspx";

                },
                error: function (xhr, status, err) {
                    DisplayErrorMessage('Error', 'An error occurred during user creation.');
                }
            });

        });

    });

});

/* GROUPS TAB */
function LoadGroupsGrid() {

    var columns = [
        { dataField: 'GroupName', caption: 'Group Name' },
        { dataField: 'Description', caption: 'Description' }
    ];

    LoadGrid('groupsgrkid', 'groupsgridcontainer', columns, 'groups');

}

function LoadGroupMembersGrid() {

    var columns = [
        { dataField: 'UserId', caption: 'User ID' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('groupmembersgrid', 'groupmembersgridcontainer', columns, 'groupmembers');

}

function LoadSecuritySettingsGrid() {

    var columns = [
        { dataField: 'UserId', caption: 'User ID' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('securitysettingsgrid', 'securitysettingsgridcontainer', columns, 'groupsettings');

}
/* END GROUPS TAB */



/* USERS TAB */
function LoadUsersGrid() {

    var columns = [
        { dataField: 'UserId', caption: 'User ID' },
        { dataField: 'Name', caption: 'Name' }
    ];

    LoadGrid('usersgrid', 'usersgridcontainer', columns, 'users');

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
            DisplayErrorMessage('Error', 'An error loading user info.');
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
        url: WEB_API_ADDRESS + 'UserRoles/AddMultiple',
        data: data,
        contentType: 'application/x-www-form-urlencoded',
        crossDomain: true,
        success: function () {

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error occurred during user creation.');
        }
    });

}
/* END USERS TAB */

function LoadGrid(grid, container, columns, route) {

    if (container.indexOf('.') != 0)
        container = '.' + container;

    var datagrid = $('<div>').addClass(grid);

    $.ajax({
        url: WEB_API_ADDRESS + route,
        method: 'GET',
        contentType: 'application/json; charset-utf-8',
        dataType: 'json',
        crossDomain: true,
        success: function (data) {

            $(datagrid).dxDataGrid({
                dataSource: data.Data,
                columns: columns,
                paging: {
                    pageSize: 25
                },
                pager: {
                    showNavigationButtons: true,
                    showPageSizeSelector: true,
                    showInfo: true,
                    allowedPageSizes: [15, 25, 50, 100]
                },
                groupPanel: {
                    visible: true,
                    allowColumnDragging: true
                },
                filterRow: {
                    visible: true,
                    showOperationChooser: false
                }
            });

            $(datagrid).appendTo($(container));

        },
        error: function (xhr, status, err) {
            DisplayErrorMessage('Error', 'An error loading grid.');
        }
    });

}


