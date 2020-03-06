$('.rating').children().on('click', function () {
    let rateValue = $("input[type='radio']:checked").val();
    let rateString = `:  ${rateValue} / 5`
    $('#rating-value').text(rateString);
});