﻿import { draw } from './schedule.js';
const uri = 'Events';

const titleTextbox = document.getElementById('title');
const descriptionTextbox = document.getElementById('description');
const scheduleIdTextbox = document.getElementById('schedule-id');
const eventIdTextbox = document.getElementById('event-id');
const isFullDayTextbox = document.getElementById('is-full-day');
const startTimeTextbox = document.getElementById('start-time');
const endTimeTextbox = document.getElementById('end-time');
const createdOnTextbox = document.getElementById('created-on');

//modal actions

$("#is-full-day").change(function () {
    if ($(this).is(":checked"))
        $('input[type="time"]').prop("disabled", true);
    else
        $('input[type="time"]').prop("disabled", false);
});

function _hideModal() {
    $('#event-modal').modal('hide');
}

function _showModal() {
    $('#event-modal').modal('show');
}

function _clearModal() {
    titleTextbox.value = '';
    descriptionTextbox.value = '';
    eventIdTextbox.value = '';
    isFullDayTextbox.checked = false;
    startTimeTextbox.value = '';
    endTimeTextbox.value = '';
}

//event actions
function _setDates() {
    let dateLi = document.getElementsByClassName('selected-day')[0];

    let day = dateLi.dataset.day;
    let month = parseInt(dateLi.dataset.month, 10) + 1;
    let year = dateLi.dataset.year;
    let date = `${year}-${month}-${day}`;
    let startTime;
    let endTime;

    if (isFullDayTextbox.checked) {
        startTime = '00:00';
        startTime = '12:00';
    }
    else {
        startTime = startTimeTextbox.value.trim();
        endTime = endTimeTextbox.value.trim();
    }

    let startDate = `${date} ${startTime}`;
    let endDate = `${date} ${endTime}`;
    console.log(startTime);
    console.log(startDate);
    return {
        startDate: startDate,
        endDate: endDate
    }
};

function _setEvent(id) {
    let event = {
        id: id,
        title: titleTextbox.value.trim(),
        description: descriptionTextbox.value.trim(),
        isFullDay: isFullDayTextbox.checked,
        scheduleId: scheduleIdTextbox.value.trim(),
        startDate: _setDates().startDate,
        endDate: _setDates().endDate,
        createdOn: createdOnTextbox.value
    };

    return event;
}

function addEvent() {

    let event = _setEvent();

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })
        .then(response => response.json())
        .then(() => draw())
        .then(() => $('#event-modal').modal('hide'))
        .catch(error => console.error('Unable to add event.', error));
}

function updateEvent(id) {
    let event = _setEvent(id);

    fetch(`${uri}/${id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(event)
    })
        .then(() => draw())
        .then(() => $('#event-modal').modal('hide'))
        .catch(error => console.error('Unable to update event.', error));
}

function loadEvent(id) {
    if (id == undefined) {
        _clearModal();
    }
    else {
        fetch(`${uri}/${id}`)
            .then(response => response.json())
            .then((currentEvent) => {
                titleTextbox.value = currentEvent.title;
                descriptionTextbox.value = currentEvent.description;
                eventIdTextbox.value = currentEvent.id;
                isFullDayTextbox.checked = currentEvent.isFullDay;
                startTimeTextbox.value = currentEvent.startDate.substring(11, 16);
                endTimeTextbox.value = currentEvent.endDate.substring(11, 16);
                createdOnTextbox.value = currentEvent.createdOn;
            });
    }

    _showModal();
}

function handleEvent() {
    let eventId = eventIdTextbox.value.trim();
    if (eventId) {
        updateEvent(eventId);
    }
    else {
        addEvent();
    }
    _hideModal();
}

function deleteEvent(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => draw())
        .catch(error => console.error('Unable to delete event.', error));
}

window.deleteEvent = deleteEvent;
window.loadEvent = loadEvent;
window.handleEvent = handleEvent;