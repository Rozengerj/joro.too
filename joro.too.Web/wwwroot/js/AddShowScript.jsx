$( document ).ready(function() {

    $( ".widget input[type=submit], .widget a, .widget button" ).button();
    $( "#AddSeasonBtn" ).on( "click", function( event ) {
        console.log("lele kolko mrazq cigani")
        event.preventDefault();
    } );
});
