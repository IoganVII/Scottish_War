var point;


AddTable = function () {

    var table = document.getElementById("GameRooms");
    var row = document.createElement("tr");



    var td1 = document.createElement("TH");
    td1.attributes('id', 'id');
    var td2 = document.createElement("TH");
    var td3 = document.createElement("TH");
    var td4 = document.createElement("TH");

    td1.innerHTML = "Id";
    td2.innerHTML = "FirstPlayGame";
    td3.innerHTML = "0";
    td4.innerHTML = "228";

    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);
    row.appendChild(td4);

    table.appendChild(row);
};

$('#GameRooms th').on('click', function () {

    var bg = $(this).parent().css("background-color");

    if (bg == "rgb(255, 255, 0)")
        $(this).parent().css("background-color", "white");
    else {
        if ($(this).parent().find('#id').text() != "ID") {
            $(this).parent().css("background-color", "yellow");
            $('#CurrentRoom').val($(this).parent().find('#id').text());
            if ((point != null) && (point.val != $('#CurrentRoom').val))
                point.css("background-color", "white");
        }
    }
    if($(this).parent().find('#id').text() != "ID")
        point = $(this).parent();
});
