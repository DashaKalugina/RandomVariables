// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
});

function OpenSelectParametersDialog(event, paramsString, distrName, shortDistrName) {
    event.preventDefault();

    var parameters = JSON.parse(paramsString);

    var paramsContainer = $("#distrParametersContainer");
    paramsContainer.empty();

    document.getElementById('distrParametersContainer').setAttribute("data-distrName", shortDistrName);

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
            distrExpression += ';';
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
var operators = ['+', '-', '*', '/'];
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
            var pdf = receivedData.pdf;
            var cdf = receivedData.cdf;

            var args1 = Array.from(JSON.parse(pdf.x));
            var values1 = Array.from(JSON.parse(pdf.y));

            var args2 = Array.from(JSON.parse(cdf.x));
            var values2 = Array.from(JSON.parse(cdf.y));

            var pdfData = [];
            var i = 0;
            args1.forEach(x => {
                pdfData.push([x, values1[i]]);
                i++;
            });

            var cdfData = [];
            i = 0;
            args2.forEach(x => {
                cdfData.push([x, values2[i]]);
                i++;
            });

            // График функции плотности
            var pdfChart = anychart.area();
            var series1 = pdfChart.area(pdfData);
            pdfChart.title("График функции плотности f(x)");

            pdfChart.xGrid().enabled(true);
            pdfChart.yGrid().enabled(true);

            pdfChart.yScale(anychart.scales.linear());
            pdfChart.xScale(anychart.scales.linear());

            pdfChart.container("pdfChartContainer");
            pdfChart.draw();

            var select1 = document.getElementById('pdfTypeSelect');
            select1.onchange = () => {
                series1.seriesType(select1.value);
            };

            // График функции распределения
            var cdfChart = anychart.area();
            var series2 = cdfChart.area(cdfData);
            cdfChart.title("График функции распределения F(x)");

            cdfChart.xGrid().enabled(true);
            cdfChart.yGrid().enabled(true);

            cdfChart.yScale(anychart.scales.linear());
            cdfChart.xScale(anychart.scales.linear());

            cdfChart.container("cdfChartContainer");
            cdfChart.draw();

            var select2 = document.getElementById('cdfTypeSelect');
            select2.onchange = () => {
                series2.seriesType(select2.value);
            };
        }
    });
}

function AddCustomDistrToScreen(event) {
    var input = document.querySelector('.screen');
    input.innerHTML += event.target.innerHTML;
}

var customDistributionNumber = 0;
function UploadFile() {
    var fileInput = $('#fileToUploadInputId');
    if (!fileInput.prop('files')) {
        toastr.warning('Ваш браузер не поддерживает загрузку файлов');
        return;
    }

    if (!fileInput.prop('files')[0]) {
        toastr.warning('Для загрузки необходимо сначала выбрать загружаемый файл');
        return;
    }
    // Проверяем, есть ли файлы с таким именем.
    var file = fileInput.prop('files')[0];

    var formData = new FormData();
    formData.append("FileUpload", file);
    customDistributionNumber++;
    formData.append("fileName", "CustomDistr" + customDistributionNumber);
    $.ajax({
        url: '/Home/AddFile',
        type: 'post',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (receivedData) {
            if (receivedData.messageError) {
                toastr.error(`При загрузке файла возникли ошибки: ${receivedData.messageError}`);
                return;
            }

            var listOfDistrButtons = $('#listOfDistrButtons');
            var newButton = `<button type="button" class="addDistrButton list-group-item list-group-item-action" onclick="AddCustomDistrToScreen(event)">${receivedData.distrName}</button>`;
            listOfDistrButtons.append($(newButton));

            toastr.success(`Содержимое файла успешно загружено в распределение ${receivedData.distrName}`);
        }
    });
}