var currentGroup = null;

$(document).ready(function () {

    $('.groupnav').click(function (e) {

        e.preventDefault();

        ShowGroupSection();

    });

    $('.usernav').click(function (e) {

        e.preventDefault();

        ShowUsersSection();

    });

    ShowGroupSection();

});

function ShowGroupSection() {

    $('li').removeClass('selected');

    $('.groupscontainer').show();
    $('.userscontainer').hide();

    $('.groupnav').parent().addClass('selected');

    $('.cancelgroup').click(function (e) {

        e.preventDefault();

        LoadGroup($('.hidgroupid').val());

    });

    LoadGroups();

    $('.hidgroupid').val('');
    $('.groupdetailscontainer').hide();
    $('.groupselectcontainer ul li').removeClass('selected');

}

function LoadGroups() {

    $('.groupselectcontainer').show();

    MakeServiceCall('GET', 'groups', null, function (data) {

        $('.groupselectcontainer').html('');
        var groups = $('<ul>');

        var newgroup = $('<li>').text('New Group').addClass('newgroup').click(function () {

            $('.hidgroupid').val('');
            $('.gp-Name').val('');

            $('.savegroup').unbind('click');
            $('.savegroup').click(function () {
                SaveGroup(null);

                $('.newlinkcontainer').show();
                $('.accordions').show();
            });

            $(this).parent().find('li').removeClass('selected');

            $('.groupdetailscontainer').show();
            $('.newlinkcontainer').hide();
            $('.accordions').hide();

        }).appendTo($(groups));

        $.map(data.Data, function(item) {

            var group = $('<li>').text(item.Name).click(function () {
                // Select the group
                $('.hidgroupid').val(item.Id);
                LoadGroup(item.Id);

                $('.savegroup').unbind('click');
                $('.savegroup').click(function () {
                    SaveGroup(item.Id);
                });

                $(this).parent().find('li').removeClass('selected');
                $(this).addClass('selected');

                $('.groupdetailscontainer').show();
                $('.newlinkcontainer').show();
                $('.accordions').show();
            });

            if ($('.hidgroupid').val() == item.Id) {
                $(group).addClass('selected');
                LoadGroup(item.Id);
            }

            $(groups).append($(group));

        });

        $('.groupselectcontainer').append($(groups));

    }, null)

}

function LoadGroup(id) {

    // Add roles link
    $('.addroles').click(function (e) {

        e.preventDefault();

        AddRolesModal(id);

    });

    MakeServiceCall('GET', 'groups/' + id, null, function (data) {

        $('.gp-Name').val(data.Data.DisplayName);

    });

    var columns = [
        { dataField: 'DisplayName', caption: 'Name' }
    ];

//  LoadGrid(container, gridClass, columns, getRoute, saveRoute, selected, prefix, editModalClass, newModalClass, modalWidth, showDelete, showFilter, showGroup, onComplete)
    LoadGrid('grouprolesgridcontainer', 'rolesgrid', columns, 'group/' + id + '/roles', null, null, 'gp-', null, null, null, false, false, false, null);
}

function SaveGroup(id) {

    var item = {
        Name: $('.gp-Name').val()
    }

    if (id) {

        MakeServiceCall('PATCH', 'groups/' + id, JSON.stringify(item), function (data) {

            DisplaySuccessMessage("Save successful.");

            LoadGroups();

        }, null);

    }
    else {

        MakeServiceCall('POST', 'groups/', JSON.stringify(item), function (data) {

            DisplaySuccessMessage("Save successful.");

            $('.hidgroupid').val(data.Data.Id);

            LoadGroups();

        }, null);

    }

}

function AddRolesModal(groupid) {

    modal = $('.rolesmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 500,
        resizable: false,
        beforeClose: function (e) {
            ClearFields('.modalcontent');
        }
    });

    $(modal).find('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $(modal).find('.saveroles').unbind('click');

    $(modal).find('.saveroles').click(function () {
        
        var roleIds = []

        $(modal).find('.roleselection input[type="checkbox"]').each(function (i, v) {
            if ($(v).prop('checked')) {
                roleIds.push($(v).attr('id'));
            }
        });

        var item = {
            roles: roleIds
        }

        MakeServiceCall('POST', 'groups/' + groupid + '/roles', JSON.stringify(item), function () {
            DisplaySuccessMessage("Save successful.");

            CloseModal(modal);

            LoadGroup(groupid);
        }, null);

    });

    MakeServiceCall('GET', 'roles', null, function (data) {

        $(modal).find('.roleselection').html('');

        var cid = 0;
        var concon = $('<div>').addClass('twocolumn');
        var left = $('<div>').appendTo($(concon));
        var right = $('<div>').appendTo($(concon));

        // display all roles
        $.map(data.Data, function (item) {

            var field = $('<div>').addClass('fieldblock');
            var role = $('<input>').attr('type', 'checkbox').attr('id', item.Id).appendTo($(field));
            var label = $('<label>').attr('for', item.Id).text(item.DisplayName).appendTo($(field));

            if (cid === 0) {

                $(field).appendTo($(left));

                cid = 1;
            }
            else {

                $(field).appendTo($(right));

                cid = 0;
            }
            

            $(modal).find('.roleselection').append($(concon));

        });

        // check selected roles
        if (groupid) {

            MakeServiceCall('GET', 'group/' + groupid + '/roles', null, function (data) {

                $(modal).find('input[type="checkbox"]').prop('checked', false);

                $.each(data.Data, function (i, v) {

                    $(modal).find('#' + v.Id).prop('checked', true);

                });

            }, null);

        }
        

    }, null);

}

function ShowUsersSection() {

    $('li').removeClass('selected');

    $('.userscontainer').show();
    $('.groupscontainer').hide();

    $('.usernav').parent().addClass('selected');

}

function LoadUsers() {

    $('.userselectcontainer').show();



}











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





