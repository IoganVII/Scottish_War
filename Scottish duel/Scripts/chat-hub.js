
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}
var pb = 0;
var eb = 0;
$(document).ready(
    function () {

        var chat = $.connection.chatHub;
        var text = document.getElementById("discussion");
        text.value = "";




        //Обработка картинок
        $('#0').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });



        $('#1').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#2').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#3').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#4').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#5').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#6').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

        $('#7').click(function () {
            if ($(this).attr('src') != $('#e0').attr('src')) {
                $('#pb' + pb.toString()).attr('src', $(this).attr('src'));
                $(this).attr('src', $('#e0').attr('src'));
                pb++;
            }
        });

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
            alert("mes");
        }

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
        }

        




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
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#1').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#2').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#3').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#4').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#5').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#6').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

                $('#7').click(function () {
                    var Login = getCookie("Login");
                    chat.server.inputCard($(this).attr("id"), Login);
                });

            });
    });