$(document).ready(function () {
    $("#Seasons").change(function(){
       console.log("i cant deal with this shit");
       let selectedep = $("#SeasonList").find("option:selected").val();
       let thingstr = ".season+"+selectedep;
       
       $(thingstr).hide();
    });
});