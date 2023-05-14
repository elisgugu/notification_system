"use strict";

var reqCon = new signalR.HubConnectionBuilder().withUrl("/requestMessages").build();
var cerCon = new signalR.HubConnectionBuilder().withUrl("/certificateMessages").build();

$(function () {
    reqCon.start().then(function () {
        InvokeRequests();
    }).catch(function (err) {
        return console.error(err.toString());
    });
    cerCon.start();
});

// Request
function InvokeRequests() {
    reqCon.invoke("sendRequests").catch(function (err) {
        return console.error(err.toString());
    });
}

reqCon.on("receivedRequest", function (requests) {
    BindRequestsToGrid(requests);
});

reqCon.on("statusChanged", function (newRequest) {
    ShowNotification(newRequest);
});

cerCon.on("certificatesExpired", function (expiredCertificates) {
    var notification = `Certificates expired for ${expiredCertificates[0].user.name}`

    $("#tblCertificate").html(notification)
});

function ShowNotification(newRequest) {
    var notification = `Request changed to ${newRequest.status.status}`

    $("#tblNotification").html(notification)
    InvokeRequests();
}

function BindRequestsToGrid(requests) {
    $('#tblRequest tbody').empty();

    var tr;
    $.each(requests, (k, v) => {
        tr = tr + `<tr>
                         <td>${v.date}</td>
                         <td>${v.user.name}</td>
                         <td>${v.status.status}</td>
                     </tr>`
        $("#tblRequest").html(tr)
    });
}