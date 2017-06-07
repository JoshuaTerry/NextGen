
$(document).ready(function () {

    SetupNewUserModal();

    LoadGroupsGrid();
    PopulateDropDown('.ConstituentId', 'constituents', '', '');
     LoadUsersGrid();

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

/* GROUPS TAB */
function LoadGroupsGrid() {

    var columns = [
        { dataField: 'DisplayName', caption: 'Group Name' }
    ];

    LoadGrid('.groupstable', 'groupgrid', columns, 'groups', 'groups'
        , null, 'gp-', '.groupmodal', '.groupmodal', 250, false, true, false, null);   
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
    PopulateDropDown($('.user-Groups'), 'groups', null);
    
    CustomLoadGrid('usergrid', 'usersgridcontainer', columns, 'users?fields=all', null, EditUser, null);

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

            LoadAvailableBusinessUnits(tagmodal, false);

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

                MakeServiceCall('POST', 'users/' + constTypeId + '/businessunits', JSON.stringify({ businessUnits: buIds }), function (data) {

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



// USER SETTINGS 
function EditUser(id) {

    LoadUser(id);


    modal = $('.usermodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 400,
        resizable: false
    });

    $('.user-tagselect').show();

    $('.cancelusermodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

        LoadUsersGrid()

    });

    $('.submituser').unbind('click');

    $('.submituser').click(function () {

        var item = {
            FullName: $(modal).find('.user-FullName').val(),
            UserName: $(modal).find('.user-UserName').val(),
            Email: $(modal).find('.user-Email').val(),
            DefaultBusinessUnitId: $(modal).find('.user-DefaultBusinessUnitId').val(),
            //GroupId: $(modal).find('.user-GroupId').val(),
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
                $(modal).find('.user-IsActive').prop('checked', data.Data.IsActive);
                
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





