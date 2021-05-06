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

    var shortDistrName = '';
    if (distrName === "FDistribution") {
        shortDistrName = 'FDistr';
    } else {
        var index = distrName.indexOf("Distribution");
        shortDistrName = distrName.substring(0, index);
    }

    var distrExpression = `${shortDistrName}(`;

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
    //if (input.innerHTML === '') {
    //    input.innerHTML += distrExpression;
    //} else {
    //    input.innerHTML += `&#13;&#10;${distrExpression}`;
    //}

    //if (input.innerHTML.length + distrExpression.length > 35) {
    //    input.innerHTML += `&#13;&#10;${distrExpression}`;
    //} else {
    //    input.innerHTML += distrExpression;
    //}

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
            EvaluateExpression(input.innerHTML);
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

function EvaluateExpression(expression) {
    $.ajax({
        url: '/Home/EvaluateExpression',
        type: 'post',
        data: { expression },
        dataType: 'json',
        accept: 'application/json',
        success: function (receivedData) {
            var args = Array.from(JSON.parse(receivedData.x));
            var values = Array.from(JSON.parse(receivedData.y));

            var data = [];

            var i = 0;
            args.forEach(x => {
                data.push([x, values[i]]);
                i++;
            });

            // График функции плотности
            var pdfChart = anychart.area();
            var series1 = pdfChart.area(data);
            pdfChart.title("График функции плотности f(x)");

            //pdfChart.xGrid().enabled(true);
            pdfChart.yGrid().enabled(true);
            // enable minor grids
            //chart.xMinorGrid().enabled(true);
            //chart.yMinorGrid().enabled(true);

            //chart.xAxis().staggerMode(true);
            //// adjusting settings for stagger mode
            //chart.xAxis().staggerLines(2);

            pdfChart.container("pdfChartContainer");
            pdfChart.draw();

            var select1 = document.getElementById('pdfTypeSelect');
            select1.onchange = () => {
                series1.seriesType(select1.value);
            };

            // График функции распределения
            var cdfChart = anychart.area();
            var series2 = cdfChart.area(data);
            cdfChart.title("График функции распределения F(x)");

            cdfChart.xGrid().enabled(true);
            cdfChart.yGrid().enabled(true);

            cdfChart.container("cdfChartContainer");
            cdfChart.draw();

            var select2 = document.getElementById('cdfTypeSelect');
            select2.onchange = () => {
                series2.seriesType(select2.value);
            };
        }
    });
}

//function switchType() {
//    var select = document.getElementById("typeSelect");
//    series.seriesType(select.value);
//}

anychart.onDocumentLoad(function () {
    // create an instance of a pie chart
    //var chart = anychart.pie();
    //// set the data
    //chart.data([
    //    ["Chocolate", 5],
    //    ["Rhubarb compote", 2],
    //    ["Crêpe Suzette", 2],
    //    ["American blueberry", 2],
    //    ["Buttermilk", 1]
    //]);
    //// set chart title
    //chart.title("Top 5 pancake fillings");
    //// set the container element 
    //chart.container("chartContainer");
    //// initiate chart display
    //chart.draw();
});

//const labels = [
//    'January',
//    'February',
//    'March',
//    'April',
//    'May',
//    'June',
//];
//const data = {
//    labels: labels,
//    datasets: [{
//        label: 'My First dataset',
//        backgroundColor: 'rgb(255, 99, 132)',
//        borderColor: 'rgb(255, 99, 132)',
//        data: [0, 10, 5, 2, 20, 30, 45],
//    }]
//};

//const config = {
//    type: 'line',
//    data,
//    options: {}
//};

//var myChart1 = new Chart(
//    document.getElementById('myChart1'),
//    config
//);

//var myChart2 = new Chart(
//    document.getElementById('myChart2'),
//    config
//);

