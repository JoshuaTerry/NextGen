$(document).ready(function () {

    CreateEditControls();

    SetupEditControls();
    
    LoadDropDowns();

});


function LoadDropDowns() {

    //interest frequency drop down
    var interestFrequency = $('.InterestFrequency')
    interestFrequency.append('<option value="None">None</option>');
    interestFrequency.append('<option value="Monthly">Monthly</option>');
    interestFrequency.append('<option value="Quarter">Quarter</option>');
    interestFrequency.append('<option value="Semi-Annual">Semi-Annual</option>');
    interestFrequency.append('<option value="Annual">Annual</option>');
    interestFrequency.append('<option value="Maturity Only">Maturity Only</option>');

}

function RefreshEntity() {

    DisplayInvestmentData();

}


function DisplayInvestmentData() {


}



