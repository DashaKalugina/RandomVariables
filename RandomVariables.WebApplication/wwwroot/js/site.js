// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
        else if (btnVal === '.') {
            if (!decimalAdded) {
                input.innerHTML += btnVal;
                decimalAdded = true;
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