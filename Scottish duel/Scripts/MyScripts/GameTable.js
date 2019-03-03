
var count = 1;

function AddTable(id)
{
    var table = document.getElementById("GameRooms");
    var newRow = table.insertRow(0)
    var newCell = newRow.insertCell(0);
    newCell.innerHTML = "ID:";

    var newCell = newRow.insertCell(1);
    newCell.innerHTML = id;



    table.appendChild(newRow);

    count++;
    
};