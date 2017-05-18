
var currentwizpos = 0;

$(document).ready(function () {

    $('.import').click(function (e) {
        e.preventDefault();

        DisplayImportWizardModal();
    });

    $('.cancelmodal').click(function (e) {

        e.preventDefault();

        CloseModal(modal);

    });

    $('.nextstep').click(function (e) {

        $(modal).find('.stepsinner').animate({
            left: '-=618'
        }, 500);

    });

    $('.prevstep').click(function (e) {

        $(modal).find('.stepsinner').animate({
            left: '+=618'
        }, 500);

    });

});

function DisplayImportWizardModal() {

    modal = $('.importwizardmodal').dialog({
        closeOnEscape: false,
        modal: true,
        width: 640,
        height: 480,
        resizable: false
    });

}

