$( document ).ready(function() {
    //function that acts upon 
    let seasonsCount = 0;
    
    $( ".widget button" ).button();
    $("#AddSeasonBtn").on("click", function (event) {
        $("#SeasonList").append('<li class="season"> <label class="search-label">  <input type="text" name="season" id="' + seasonsCount + '" class="input" required="" placeholder="Input Season Name Here"> </label> <button class="RemoveSeasonBtn">idk bro help me</button> </li>');
        seasonsCount++;
        $('.RemoveSeasonBtn').click(function (event) {
            $(this).closest('.season').remove();
        });
        $("#AddEpisodeBtn").on("click", function (event) {
            //$(this).closest('.season').contents()
            //document.getElementById()
        });
    });
});
