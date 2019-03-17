$(function () {

    $('#choose').click(function () {
        inHTML = "";
        $("#Authors option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Authors option:selected").remove();
        $("#ChoosenAuthors").append(inHTML);
    });

    $('#remove').click(function () {
        inHTML = "";
        $("#ChoosenAuthors option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#ChoosenAuthors option:selected").remove();
        $("#Authors").append(inHTML);
    });


    $('#choose1').click(function () {
        inHTML = "";
        $("#Genres option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Genres option:selected").remove();
        $("#ChoosenGenres").append(inHTML);
    });

    $('#remove1').click(function () {
        inHTML = "";
        $("#ChoosenGenres option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Genres").append(inHTML);
        $("#ChoosenGenres option:selected").remove();
    });

    $("form").submit(function (e) {
        $("#ChoosenAuthors option").attr("selected", "selected");
        $("#ChoosenGenres option").attr("selected", "selected");
    });

});