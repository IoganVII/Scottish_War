
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
};

function setCookie(name, value, days, path) {

    path = path || '/'; // заполняем путь если не заполнен
    days = days || 10;  // заполняем время жизни если не получен параметр

    var last_date = new Date();
    last_date.setDate(last_date.getDate() + days);
    var value = escape(value) + ((days == null) ? "" : "; expires=" + last_date.toUTCString());
    document.cookie = name + "=" + value + "; path=" + path; // вешаем куки
}


var pb = 0;
var eb = 0;
var cardmoment = false;
$(document).ready(
    function () {

        var room = $.connection.roomHub;

        room.client.generatedTeam = function (color) {
            if (color == "С") {
                $('#0').attr("src", $('#saveC0').attr('src'));
                $('#1').attr("src", $('#saveC1').attr('src'));
                $('#2').attr("src", $('#saveC2').attr('src'));
                $('#3').attr("src", $('#saveC3').attr('src'));
                $('#4').attr("src", $('#saveC4').attr('src'));
                $('#5').attr("src", $('#saveC5').attr('src'));
                $('#6').attr("src", $('#saveC6').attr('src'));
                $('#7').attr("src", $('#saveC7').attr('src'));
            } else {
                $('#0').attr("src", $('#saveK0').attr('src'));
                $('#1').attr("src", $('#saveK1').attr('src'));
                $('#2').attr("src", $('#saveK2').attr('src'));
                $('#3').attr("src", $('#saveK3').attr('src'));
                $('#4').attr("src", $('#saveK4').attr('src'));
                $('#5').attr("src", $('#saveK5').attr('src'));
                $('#6').attr("src", $('#saveK6').attr('src'));
                $('#7').attr("src", $('#saveK7').attr('src'));
            }
        };

        room.client.getboolcard = function () {
            cardmoment = true;
        };


        room.client.enemyCard = function (cardId, color, flag) {
            if (flag == 0) {
                $('#eb' + eb.toString()).attr('src', $('#e0').attr('src'));
            }
            if (flag == 1) {
                if (color == "С")
                    $('#eb' + eb.toString()).attr('src', $('#saveC' + cardId).attr('src'));
                if (color == "K")
                    $('#eb' + eb.toString()).attr('src', $('#saveK' + cardId).attr('src'));
                eb++;
            }
        };

        room.client.resultbattle = function (mes, VP1, VP2) {
            alert(mes);
            $('#LabelFirstPlayer').text("Синий игрок: " + String(VP1));
            $('#LabelSecondPlayer').text("Красный игрок: " + String(VP2));
            cardmoment = false;
        };

        room.client.upDateRoom = function () {
            document.location.href = "ClientRoom";
        };


        $.connection.hub.start().
            done(function () {

                var Login = getCookie("Login");
                room.server.init(Login);

                //Обработка перемещение картинок отпрака на сервер
                $('#0').click(function () {

                    var Login = getCookie("Login");
                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }


                });

                $('#1').click(function () {
                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#2').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#3').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#4').click(function () {


                    var Login = getCookie("Login");

                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#5').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#6').click(function () {


                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#7').click(function () {



                    var Login = getCookie("Login");



                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        room.server.inputCard($(this).attr("id"), Login);
                    }
                });

            });
    });