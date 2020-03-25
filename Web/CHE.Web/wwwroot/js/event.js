import { draw } from './schedule.js';
const uri = 'Events';

function addEvent() {
    const titleTextbox = document.getElementById('title');
    const descriptionTextbox = document.getElementById('description');
    const scheduleIdTextbox = document.getElementById('schedule-id');
    const isFullDayTextbox = document.getElementById('is-full-day');
    const startTimeTextbox = document.getElementById('start-time');
    const endTimeTextbox = document.getElementById('end-time');
    const dateLi = document.getElementsByClassName('selected-day')[0];

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

    let startDate = new Date(date + ' ' + startTime);
    let endDate = new Date(date + ' ' + endTime);

    let event = {
        title: titleTextbox.value.trim(),
        description: descriptionTextbox.value.trim(),
        isFullDay: isFullDayTextbox.checked,
        scheduleId: scheduleIdTextbox.value.trim(),
        startDate: startDate,
        endDate: endDate
    }

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

window.addEvent = addEvent;