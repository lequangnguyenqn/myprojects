﻿/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />

$(document).ready(function () {
    function getDateYymmdd(value) {
        if (value == null)
            return null;
        return $.datepicker.parseDate("mm/dd/yy", value);
    }
    $('.date').each(function () {
        var minDate = getDateYymmdd($(this).data("val-rangedate-min"));
        var maxDate = getDateYymmdd($(this).data("val-rangedate-max"));
        $(this).datepicker({
            dateFormat: "mm/dd/yy",  // hard-coding uk date format, but could embed this as an attribute server-side (based on the current culture)
            minDate: minDate,
            maxDate: maxDate,
            buttonImage: '../../Content/images/cal_icon.png',
            showOn: 'both'
        });
    });

});