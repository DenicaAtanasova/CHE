const connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/messenger")
        .build();

connection.on("NewMessage",
    function (message, sender, connectionId) {
        const currentConnectionId = this.connection.connectionId;
        if (!connectionId || connectionId === currentConnectionId) {
            renderMessage(message, sender);
        }
        else {
            renderMessage(message);
        }

        const messageCount = $('#msg-count').text().split(' ');
        messageCount[1] = parseInt(messageCount[1]) + 1;
        $('#msg-count').text(messageCount.join(' '));
        updateScroll();
    });

connection.on("UpdateStatus", function (users) {
    for (const [id, status] of Object.entries(users)) {
        $(`#${id} > .status > span`).text(`${status}`);
        $(`#${id} > .status > i`).attr('class', `fa fa-circle status-icon ${status}`);
    }
});

connection.on("Disconnect", function () {
    console.log("Disconnected");
});

function sendMessage(method, messengerId, receiverId) {

    const text = $("#message-to-send").val();

    if (!isMessageEmpty(text)) {
        connection.invoke(method, messengerId, receiverId, text);
        $("#message-to-send").val('');
    }
}

$(".private-chat").each(function () {
    $(this).on('click', function (event) {
        prepareChat();

        const receiverId = event.currentTarget.id;

        const uri = `/messengers/getprivate?receiverId=${receiverId}`;
        fetch(uri)
            .then(response => response.json())
            .then(data => {
                const messages = data.messages;
                messages.forEach(message => {
                    renderMessage(message, data.currentReceiver);
                });

                $('#msg-count').text(`already ${messages.length} messages`);

                return data;
            })
            .then(data => {
                $('#send-button').off('click');
                $('#send-button').on('click', function () {
                    sendMessage("SendPrivate", data.id, receiverId);
                })
            })
            .catch(err => console.log(err));

        const receiverName = $(`#${receiverId} > .name`).text();
        $('#current-msg-receiver').text(`Chat with: ${receiverName}`);
    });

    updateScroll();
});

$("a.group-chat").on('click', function (event) {
    prepareChat();

    const cooperativeId = event.currentTarget.id;
    const uri = `/parent/messengers/getcooperative?cooperativeId=${cooperativeId}`;

    fetch(uri)
        .then(response => response.json())
        .then(data => {
            const messages = data.messages;
            messages.forEach(message => {
                renderMessage(message, data.currentUser);
            });

            $('#msg-count').text(`already ${messages.length} messages`);
            return data;
        })
        .then(data =>
        {
            $('#send-button').off('click');
            $('#send-button').on('click', function () {
                sendMessage("SendGroup", data.id, cooperativeId);
            })
        })
        .catch(err => console.log(err));

    const cooperativeName = $('#group-chat > .name').text();
    $('#current-msg-receiver').text(`Chat with: ${cooperativeName}`);

    updateScroll();
});

$('#send-current-button').on('click', function () {
    const messengerId = $(this).attr('messengerId');
    const receiverId = $(this).attr('receiverId');

    sendMessage("SendPrivate", messengerId, receiverId);
});

connection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });

$(document).ready(function () {
    const searchFilter = {
        options: { valueNames: ['name'] },
        init: function () {
            const userList = new List('people-list', this.options);
            const noItems = $('<li id="no-items-found">No items found</li>');
            console.log();
            userList.on('updated', function (list) {
                if (list.matchingItems.length === 0) {
                    $(list.list).append(noItems);
                } else {
                    noItems.detach();
                }
            });
        }
    };

    searchFilter.init();
});

function renderMessage(message, currentUser) {
    const $spanTime = $('<span>')
        .addClass('message-data-time')
        .text(`${message.createdOn}`);

    const $spanName = $('<span>')
        .addClass('message-data-name')
        .text(` ${message.sender}`);

    const $divMsgData = $('<div>')
        .addClass('message-data');

    if (currentUser === message.sender) {
        $divMsgData
            .addClass('align-right')
            .append($spanTime)
            .append($spanName);
    }
    else {
        $divMsgData
            .append($spanName)
            .append($spanTime);
    }

    const $spanText = $('<span>')
        .text(`${message.text}`);

    const $divMsg = $('<div>')
        .addClass('message')
        .append($spanText);

    if (currentUser === message.sender) {
        $divMsg
            .addClass('float-right')
            .addClass('other-message');
    }
    else {
        $divMsg
            .addClass('my-message')
    }

    const $li = $('<li>')
        .append($divMsgData)
        .append($divMsg);

    $('#messages-list').append($li);
}

function updateScroll() {
    const element = document.getElementById('history');
    element.scrollTop = element.scrollHeight;
}

function isMessageEmpty(text) {
    if (text == "") {
        $('div.chat-message.clearfix > span').text("Meesage should not be empty");
        return true;
    }

    return false;
}

function prepareChat() {
    $("#messages-list").empty();
    $('#send-button').prop("disabled", false);
    $('#message-to-send').prop("disabled", false);
};