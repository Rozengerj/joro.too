$(document).ready(function () {
    //function that acts upon 
    let seasonsCount = 0;
    $(".widget button").button();
    $("#AddSeasonBtn").on("click", function (event) {
        let epcount = 0;
        $("#SeasonList").append(
            '<li class="season"> ' +
            '   <div class="form-container helper" id="s' + seasonsCount + '">' +
            '      <div class="input-group">' +
            '        <label class="search-label">'+
            '         <input type="text" name="season" id="' + seasonsCount + '" class="input" required="" placeholder="Input Season Name Here">' +
            '         </label>'+          
            '         <div>' +
            '            <div class="AddEpisodeDiv">' +
            '               <button class="AddEpisodeBtn'+ seasonsCount +'">Add Episode</button>' +
            '            </div>'+
            '         </div>' +
            '            <input type="hidden" name="episode" id="99999s' + seasonsCount + '" value="_-_-_@_-_-_">' +
            '            </input>'+
            '      </div>' +
            '   <button class="RemoveSeasonBtn">Remove Season</button> ' +
            '   </div>'+
            '</li>');
        $('.RemoveSeasonBtn').click(function (event) {
            console.log("does this run multiple times??")
            $(this).closest('.season').remove();
        });
        //console.log("COUNT UP")
        let addepstring=".AddEpisodeBtn"+seasonsCount
        seasonsCount++;
        //console.log(addepstring)
        $(addepstring).click(function (event) {
            //$(this).closest('.input').remove();
            let seasonnum = $(this).closest('.helper').map(function(){
                return this.id;
            }).get().join();
            console.log("are you gonna write shit in here four times i na row you fucking piece of shit")
            $(this).closest('.AddEpisodeDiv').append(
                '      <div class="input-group ep">' +
                '         <input type="text" name="episode" id="'+ epcount + seasonnum + '" asp-validation-for="" class="input" required="" placeholder="Input Episode Name Here">' +
                '         <input type="file" name="episodevidsrc" id="' + epcount + seasonnum + '" accept="video/*">' +
                '         <button class="RemoveEpisodeBtn">Remove Episode</button>'+
                '      </div>'
            );
            epcount++;
            $(".RemoveEpisodeBtn").on("click", function () {
                $(this).closest(".ep").remove();
            })
            }
            //document.getElementById()
        );
    });
});

