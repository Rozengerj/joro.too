$(document).ready(function () {
    let prevselectedep = "#season-0"
    let firststr = $(".episodebutton").val();
    console.log(firststr);
    $(".vidplayer").attr("src",firststr);
    $("#Seasons").change(function (event) {
        $(prevselectedep).hide();
        console.log("i cant deal with this shit");
        let selectedep = $("#SeasonList").find("option:selected").val();
        let thingstr = "#season-" + selectedep;
        $(thingstr).show();
        prevselectedep = thingstr;
    });
    $(".episodebutton").on("click", function (event) {
        $(".vidplayer").attr("src",$(this).val());
        $(".vidplayer").attr("title",$(this).text());
    })
});