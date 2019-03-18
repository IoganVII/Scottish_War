
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

$(document).ready(
    function () {
        var chat = $.connection.chatHub;
        var text = document.getElementById("discussion");
        text.value = "";


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



            });
    });