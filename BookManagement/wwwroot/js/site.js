"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("OrderStatusChanged", function (orderId, newStatus) {
    const orderCard = document.querySelector(`.order-card[data-order-id='${orderId}']`);
    if (orderCard) {
        const statusSpan = orderCard.querySelector('.order-status');
        if (statusSpan) {
            statusSpan.textContent = newStatus;
            statusSpan.className = 'order-status ' + newStatus;
        }
    }
});

connection.start()
    //.then(() => console.log("SignalR connected"))
    .catch(function (err) {
        return console.error(err.toString());
    });

 