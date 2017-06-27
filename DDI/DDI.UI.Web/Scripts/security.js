
$(document).ready(function () {

    Resize();

    $('.groupnav').click(function (e) {

        e.preventDefault();

        ShowGroupSection();

    });

    $('.usernav').click(function (e) {

        e.preventDefault();

        ShowUsersSection();

    });

    ShowGroupSection();

    $(window).resize(function () {
        Resize();
    });

});

function Resize() {

    var windowHeight = $(window).height();
    var header = $('header').height();
    var adjustedHeight = (windowHeight - header) - 160;

    $('.groupselectcontainer ul').height(adjustedHeight);
    $('.userselectcontainer ul').height(adjustedHeight);

}

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

    // Add roles link
    $('.addroles').unbind('click');
    $('.addroles').click(function (e) {

        e.preventDefault();

        AddRolesModal();

    });

}

function ClearGroupFields() {

    $('.hidgroupid').val('');
    $('.gp-Name').val('');

    $(this).parent().find('li').removeClass('selected');

    $('.groupdetailscontainer').show();
    $('.newlinkcontainer').hide();
    $('.accordions').hide();

}

function LoadGroups() {

    $('.groupselectcontainer').show();

    MakeServiceCall('GET', 'groups', null, function (data) {

        $('.groupselectcontainer').html('');
        var groups = $('<ul>');

        var newgroup = $('<div>').html('<img src="../../Images/New_16.png" /> New Group').addClass('newgroup').click(function () {

            ClearGroupFields();

            $('.savegroup').unbind('click');
            $('.savegroup').click(function () {
                SaveGroup(null);

                $('.newlinkcontainer').show();
                $('.accordions').show();
            });

        }).prependTo($('.groupselectcontainer'));

        $.map(data.Data, function(item) {

            var group = $('<li>').html(item.DisplayName).click(function () {
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

            var delImg = $('<img>').attr('src', '../../Images/erase-16.png').click(function () {

                ConfirmModal('Are you sure you want to delete this Group?', function () {

                    MakeServiceCall('DELETE', 'groups/' + item.Id, null, function () {

                        ClearGroupFields();

                        DisplaySuccessMessage("Delete successful.");

                        LoadGroups();

                    }, null);

                }, function () { return; });

            }).appendTo($(group));

            if ($('.hidgroupid').val() == item.Id) {
                $(group).addClass('selected');
                LoadGroup(item.Id);
            }

            $(groups).append($(group));

        });

        $('.groupselectcontainer').append($(groups));

        Resize();

    }, null)

}

function LoadGroup(id) {

    MakeServiceCall('GET', 'groups/' + id, null, function (data) {

        $('.gp-Name').val(data.Data.DisplayName);

    });

    var columns = [
        { dataField: 'DisplayName', caption: 'Name' }
    ];

    // Roles in Group Grid
    LoadGrid('grouprolesgridcontainer', 'rolesgrid', columns, 'group/' + id + '/roles', null, null, 'gp-', null, null, null, false, false, false, null);

    // Users in Group Grid    
    LoadGrid('groupusersgridcontainer', 'groupusersgrid', columns, 'group/' + id + '/users', null, null, null, null, null, null, false, false, false, null);
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

function AddRolesModal() {

    var groupid = $('.hidgroupid').val();

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

        $('.roleselection div').empty();

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

    MaskFields();

    $('.canceluser').click(function (e) {

        e.preventDefault();

        LoadGroup($('.hiduserid').val());

    });

    LoadUsers();

    $('.hiduserid').val('');
    $('.userdetailscontainer').hide();
    $('.userselectcontainer ul li').removeClass('selected');

    $('.managegroups').unbind('click');
    $('.managegroups').click(function (e) {

        e.preventDefault();

        LoadUserGroupsModal();

    });

    $('.managebu').unbind('click');
    $('.managebu').click(function (e) {

        e.preventDefault();

        LoadUserBusinessUnitsModal();

    });

}

function LoadUsers() {

    $('.userselectcontainer').show();
    
    PopulateDropDown('.user-DefaultBusinessUnitId', 'businessunits', null, null, null, null, null);

    MakeServiceCall('GET', 'users', null, function (data) {

        $('.userselectcontainer').html('');
        var users = $('<ul>');

        var newuser = $('<div>').html('<img src="../../Images/New_16.png" /> New User').addClass('newuser').click(function () {

            $('.hiduserid').val('');
            $('.rs-Constituent1Id').val('');
            $('.user-FullName').val('');
            $('.userdetailscontainer input[type="text"').val('');
            $('.userdetailscontainer select').val('');
            $('.user-IsActive').prop('checked', true);

            $('.saveuser').unbind('click');
            $('.saveuser').click(function () {
                SaveUser(null);

                $('.newlinkcontainer').show();
            });

            $(this).parent().find('li').removeClass('selected');

            $('.userdetailscontainer').show();
            $('.newlinkcontainer').hide();
            $('.userextendeddetails').hide();

        }).prependTo($('.userselectcontainer'));

        $.map(data.Data, function (item) {

            var user = $('<li>').text(item.DisplayName).click(function () {
                // Select the user
                $('.hiduserid').val(item.Id);
                LoadUser(item.Id);

                $('.saveuser').unbind('click');
                $('.saveuser').click(function () {
                    SaveUser(item.Id);
                });

                $(this).parent().find('li').removeClass('selected');
                $(this).addClass('selected');

                $('.userdetailscontainer').show();
                $('.newlinkcontainer').show();
            });

            if ($('.hiduserid').val() == item.Id) {
                $(user).addClass('selected');
                LoadUser(item.Id);
            }

            $(users).append($(user));

        });

        $('.userselectcontainer').append($(users));

        Resize();

    }, null)

}

function LoadUser(id) {

    MakeServiceCall('GET', 'users/' + id, null, function (data) {
        
        $('.user-UserName').val(data.Data.UserName);
        $('.user-FullName').val(data.Data.FullName);
        $('.user-Email').val(data.Data.Email);
        $('.user-PhoneNumber').val(data.Data.PhoneNumber);
        $('.user-DefaultBusinessUnitId').val(data.Data.DefaultBusinessUnitId);

        if (data.Data.Constituent) {
            $('.rs-Constituent1Information').val(data.Data.Constituent.ConstituentNumber + ": " + data.Data.Constituent.Name + ", " + data.Data.Constituent.PrimaryAddress);
            $('.rs-Constituent1Id').val(data.Data.ConstituentId);
        }
        else {
            $('.rs-Constituent1Information').val('');
            $('.rs-ConstituentId').val('');
        }
        
        $('.user-IsActive').prop('checked', data.Data.IsActive);

        $('.userextendeddetails').show();

        LoadUserGroups();

        LoadUserBusinessUnits();

    }, null);

}

function LoadUserGroups() {
    
    var columns = [
        { dataField: 'DisplayName', caption: 'Name' }
    ];

    LoadGrid('usergroupsgridcontainer', 'usergroupsgrid', columns, 'users/' + $('.hiduserid').val() + '/groups', null, null, null, null, null, null, false, false, false, null);

}

function LoadUserGroupsModal() {

    var userid = $('.hiduserid').val();

    modal = $('.usergroupsmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 550,
        resizable: false,
        beforeClose: function (e) {
            ClearFields('.modalcontent');
        }
    });

    $(modal).find('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $(modal).find('.saveusergroups').unbind('click');

    $(modal).find('.saveusergroups').click(function () {

        var groupIds = []

        $(modal).find('.usergroupsselection input[type="checkbox"]').each(function (i, v) {
            if ($(v).prop('checked')) {
                groupIds.push($(v).attr('id'));
            }
        });

        var item = {
            groups: groupIds
        }

        MakeServiceCall('PATCH', 'users/' + userid + '/groups', JSON.stringify(item), function () {
            DisplaySuccessMessage("Save successful.");

            CloseModal(modal);

            LoadUser(userid);
        }, null);

    });

    MakeServiceCall('GET', 'groups', null, function (data) {

        $('.usergroupsselection div').empty();

        var cid = 0;
        var concon = $('<div>').addClass('twocolumn');
        var left = $('<div>').appendTo($(concon));
        var right = $('<div>').appendTo($(concon));

        // display all groups
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


            $(modal).find('.usergroupsselection').append($(concon));

        });

        // check selected groups
        if (userid) {

            MakeServiceCall('GET', 'users/' + userid + '/groups', null, function (data) {

                $(modal).find('input[type="checkbox"]').prop('checked', false);

                $.each(data.Data, function (i, v) {

                    $(modal).find('#' + v.Id).prop('checked', true);

                });

            }, null);

        }


    }, null);

}

function LoadUserBusinessUnits() {

    var columns = [
        { dataField: 'DisplayName', caption: 'Name' }
    ];

    LoadGrid('userbugridcontainer', 'userbugrid', columns, 'users/' + $('.hiduserid').val() + '/businessunit', null, null, null, null, null, null, false, false, false, null);

}

function LoadUserBusinessUnitsModal() {

    var userid = $('.hiduserid').val();

    modal = $('.userbusinessunitsmodal').dialog({
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

    $(modal).find('.saveuserbu').unbind('click');

    $(modal).find('.saveuserbu').click(function () {

        var buIds = []

        $(modal).find('.userbuselection input[type="checkbox"]').each(function (i, v) {
            if ($(v).prop('checked')) {
                buIds.push($(v).attr('id'));
            }
        });

        var item = {
            businessUnits: buIds
        }

        MakeServiceCall('PATCH', 'users/' + userid + '/businessunits', JSON.stringify(item), function () {
            DisplaySuccessMessage("Save successful.");

            CloseModal(modal);

            LoadUser(userid);
        }, null);

    });

    MakeServiceCall('GET', 'businessunits', null, function (data) {

        $('.userbuselection div').empty();

        var cid = 0;
        var concon = $('<div>').addClass('twocolumn');
        var left = $('<div>').appendTo($(concon));
        var right = $('<div>').appendTo($(concon));

        // display all business units
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


            $(modal).find('.userbuselection').append($(concon));

        });

        // check selected business unit
        if (userid) {

            MakeServiceCall('GET', 'users/' + userid + '/businessunit', null, function (data) {

                $(modal).find('input[type="checkbox"]').prop('checked', false);

                $.each(data.Data, function (i, v) {

                    $(modal).find('#' + v.Id).prop('checked', true);

                });

            }, null);

        }


    }, null);

}

function SaveUser(id) {

    var item = {
        Id: id,
        UserName: $('.user-UserName').val(),
        FullName: $('.user-FullName').val(),
        Email: $('.user-Email').val(),
        PhoneNumber: $('.user-PhoneNumber').val(),
        ConstituentId: $('.user-ConstituentId').val(),
        DefaultBusinessUnitId: $('.user-DefaultBusinessUnitId').val(),
        IsActive: $('.user-IsActive').prop('checked')
    }

    if (id) {

        MakeServiceCall('POST', 'users/' + id, JSON.stringify(item), function (data) {

            DisplaySuccessMessage("Save successful.");

            LoadUsers();

        }, null);

    }
    else {

        MakeServiceCall('POST', 'users/', JSON.stringify(item), function (data) {

            DisplaySuccessMessage("Save successful.");

            $('.hiduserid').val(data.Data.Id);

            LoadUsers();

        }, null);

    }

}