/* Author: 

*/

$(function () {
    $("input[type='checkbox']:not(:checked)").parent().removeClass("highlight");
    $("input[type='checkbox']:not(:checked)").parent().siblings().removeClass("highlight");
    $("input[type='checkbox']:checked, input[type='checkbox'][class!='chk_account']:checked").parent().addClass("highlight");
    $("input[type='checkbox']:checked, input[type='checkbox'][class!='chk_account']:checked").parent().siblings().addClass("highlight");

    $("input").click(function () {
        if ($(this).is("[type='checkbox']") && $(this).is(":checked") && !($(this).hasClass("chk_account"))) {
            $(this).parent().addClass("highlight");
            $(this).parent().siblings().addClass("highlight");
        }

        $("input[type='checkbox']:not(:checked)").parent().removeClass("highlight");
        $("input[type='checkbox']:not(:checked)").parent().siblings().removeClass("highlight");
    });

    if (!$.browser.opera) {
        // select element styling
        $('select.select').each(function () {
            var title;
            if ($(this).val() != '') {
                title = $('option:selected', this).text();
            }
            else {
                title = $(this).attr('title');
            }
            $(this)
					.css({ 'z-index': 10, 'opacity': 0, '-khtml-appearance': 'none' })
					.after('<span class="select">' + title + '</span>')
					.change(function () {
					    val = $('option:selected', this).text();
					    $(this).next().text(val);
					})
        });
    };
});





















