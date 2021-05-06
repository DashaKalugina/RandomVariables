// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//$(document).ready(function () {
//    $(".addDistrButton").click(function (e) {

//        e.preventDefault();
//        //$('#dialogContent').html(data);
//        $('#selectParametersModal').modal('show');
//    });
//})

function OpenSelectParametersDialog(event, paramsString, distrName) {
    event.preventDefault();

    var parameters = JSON.parse(paramsString);

    var paramsContainer = $("#distrParametersContainer");
    paramsContainer.empty();

    document.getElementById('distrParametersContainer').setAttribute("data-distrName", distrName);

    parameters.forEach(param => {

        var element = `<div class="form-group row"><label class="col-sm-6 col-form-label">${param}</label><div class="col-sm-6"><input class="paramInput form-control"></div></div>`;
        paramsContainer.append($(element));
    });

    $('#selectParametersModal').modal('show');
}

function AddDistributionToScreen(e) {
    // получаем название распределения
    var distrName = document.getElementById('distrParametersContainer').getAttribute("data-distrName");

    // получаем значения параметров
    var paramsContainer = $("#distrParametersContainer");
    var params = paramsContainer.find("input");

    var distrExpression = `${distrName}(`;

    var firstParam = true;
    Array.from(params).forEach(p => {
        if (!firstParam) {
            distrExpression += ',';
        }

        distrExpression += Number(p.value);
        firstParam = false;
    });

    distrExpression += ')';

    var input = document.querySelector('.screen');
    input.innerHTML += distrExpression;

    $('#selectParametersModal').modal('hide');

    e.preventDefault();
}

var keys = document.querySelectorAll('#calculator span');
var operators = ['+', '-', 'x', '/'];
var decimalAdded = false;

for (var i = 0; i < keys.length; i++) {
    keys[i].onclick = function (e) {
        var input = document.querySelector('.screen');
        var inputVal = input.innerHTML;
        var btnVal = this.innerHTML;

        if (btnVal === 'C') {
            input.innerHTML = '';
            decimalAdded = false;
        } 
        else if (btnVal === "<i class=\"fas fa-backspace\"></i>") {
            var screenValueLength = input.innerHTML.length;
            input.innerHTML = input.innerHTML.substring(0, screenValueLength - 1);
        }
        else if (btnVal === '=') {

        }
        else if (operators.indexOf(btnVal) > -1) {
            // Operator is clicked
            // Get the last character from the equation
            var lastChar = inputVal[inputVal.length - 1];

            // Only add operator if input is not empty and there is no operator at the last
            if (inputVal !== '' && operators.indexOf(lastChar) === -1) {
                input.innerHTML += btnVal;
            }
            // Allow minus if the string is empty
            else if (inputVal === '' && btnVal === '-') {
                input.innerHTML += btnVal;
            }

            // Replace the last operator (if exists) with the newly pressed operator
            if (operators.indexOf(lastChar) > -1 && inputVal.length > 1) {
                // Here, '.' matches any character while $ denotes the end of string, so anything (will be an operator in this case) at the end of string will get replaced by new operator
                input.innerHTML = inputVal.replace(/.$/, btnVal);
            }

            decimalAdded = false;
        }
        else if (btnVal === '.') {
            if (!decimalAdded) {
                if (input.innerHTML === "") {
                    input.innerHTML += "0.";
                }
                else {
                    input.innerHTML += btnVal;
                }
                
                decimalAdded = true;
            }
        }
        else if (btnVal === "+/-") {
            if (inputVal === '') {
                input.innerHTML += '-';
            }
        }
        // if any other key is pressed, just append it
        else {
            input.innerHTML += btnVal;
        }

        // prevent page jumps
        e.preventDefault();
    }
}

const labels = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
];
const data = {
    labels: labels,
    datasets: [{
        label: 'My First dataset',
        backgroundColor: 'rgb(255, 99, 132)',
        borderColor: 'rgb(255, 99, 132)',
        data: [0, 10, 5, 2, 20, 30, 45],
    }]
};

const config = {
    type: 'line',
    data,
    options: {}
};

var myChart1 = new Chart(
    document.getElementById('myChart1'),
    config
);

var myChart2 = new Chart(
    document.getElementById('myChart2'),
    config
);