
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

        var chat = $.connection.chatHub;
        var text = document.getElementById("discussion");
        text.value = "";




        

        //Конец обработки картинок

        //Обработка перемещения карт противника

        chat.client.enemyCard = function (cardId, color) {
            if (color == "С")
                $('#eb' + eb.toString()).attr('src', $('#saveC' + cardId).attr('src'));
            if (color == "K")
                $('#eb' + eb.toString()).attr('src', $('#saveK' + cardId).attr('src'));
            eb++;
        };


        //Конец обработки перемещения карт противника







        $('#displayname').val(getCookie("Login"));

        chat.client.broadcastMessage = function (name, message) {
            text.value = text.value + name + ": " + message;
            text.value = text.value + "\n";
        };

        chat.client.upDateTableRoom = function () {
            document.location.href = "ClientRoom";
        };

        chat.client.upDateRoom = function () {
            document.location.href = "CreatedRoom";
        };

        chat.client.startGame = function (id) {
            document.location.href = "Game";
        };

        chat.client.resultbattle = function (mes) {
            alert(mes);
            cardmoment = false;
        };

        chat.client.generatedTeam = function (color) {
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

        chat.client.backTableRoom = function () {

        };






        $('#message').focus();

        $.connection.hub.start().
            done(function () {




                $('#sendmessage').click(function () {
                    chat.server.send($('#displayname').val(), $('#message').val());
                    $('#message').val('').focus();
                });

                $('#ViewRoom').click(function () {
                    chat.server.viewRoomGroup($('#displayname').val(), $('#name').val(), $('#password').val())
                });

                $('#joinRoom').click(function () {
                    chat.server.joinRoomGroup($('#displayname').val(), point.find('#id').text());
                });

                if (document.location.pathname == "/Play/ClientRoom") {
                    chat.server.waitPlayer("WaitPlayer");
                };

                if (document.location.pathname == "/Play/CreatedRoom") {
                    chat.server.groupPlayerInRoom($('#idRoom').val());
                };

                if (document.location.pathname == "/Play/Game") {
                    var Login = getCookie("Login");
                    chat.server.startgame(Login, 1);
                };

                $('#bStartGame').click(function () {
                    var Login = getCookie("Login");
                    chat.server.startgame(Login, 0);
                });

                $('#Back').click(function () {
                    var Login = getCookie("Login");
                    chat.server.backInTableRoom(Login);
                });




                $('#Show-chat').click(function () {
                    var chatBodyNode = $('.js-chat-body');
                    if (chatBodyNode.hasClass('hidden')) {
                        chatBodyNode.removeClass('hidden');
                    } else {
                        chatBodyNode.addClass('hidden');
                    }
                })


                //Обработка перемещение картинок отпрака на сервер
                $('#0').click(function () {
                    
                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;         
                        chat.server.inputCard($(this).attr("id"), Login);
                    }


                });

                $('#1').click(function () {
                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#2').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#3').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#4').click(function () {


                    var Login = getCookie("Login");

                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#5').click(function () {

                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#6').click(function () {


                    var Login = getCookie("Login");


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });

                $('#7').click(function () {



                    var Login = getCookie("Login");
    


                    if (($(this).attr('src') != $('#e0').attr('src')) && (cardmoment == false)) {
                        $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                        $(this).attr('src', $('#e0').attr('src'));
                        pb++;
                        chat.server.inputCard($(this).attr("id"), Login);
                    }
                });


                $('#HomePlace').click(function () {
                    var Login = getCookie("Login");
                    if (confirm("Вы хотите выйти? Да Или Нет")) {
                        chat.server.outPlayer(Login);
                    }
                });

                $('#RegisterPlace').click(function () {
                    var Login = getCookie("Login");
                    if (confirm("Вы хотите выйти? Да Или Нет")) {
                        chat.server.outPlayer(Login);
                    }
                });

                $('#DescriptionPace').click(function () {
                    var Login = getCookie("Login");
                    if (confirm("Вы хотите выйти? Да Или Нет")) {
                        chat.server.outPlayer(Login);
                    }
                });

                window.onbeforeunload = function () {
                    var Login = getCookie("Login");
                    setCookie('Login', Login, -2, '/');
                    chat.server.playerClosWindow(Login);
                }

            });
    });