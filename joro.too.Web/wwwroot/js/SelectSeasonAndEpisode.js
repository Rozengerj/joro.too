$(document).ready(function () {
    let prevselectedep = "#season-0"
    let firststr = $(".episodebutton").val();
    $("."+firststr.substring(62,72)).show();
    $(this).find(".episodebutton").css("background-color","#524962");
    $(".episodebutton").css("background-color","#221932");
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
        $(".allcoments").hide();
        $(".episodebutton").css("background-color","#221932");
        let vidsrc = $(this).val();
        $(".vidplayer").attr("src",vidsrc);
        $(".vidplayer").attr("title",$(this).text());
        $(this).css("background-color","#524962");
        let id = "."+vidsrc.substring(62,72)
        $(id).show();
    })
});