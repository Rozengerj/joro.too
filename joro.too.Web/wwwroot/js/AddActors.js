$(document).ready(function () {
    let actorcount =0;
    $("#AddActor").on("click", function () {
        $("#SeasonList").append(
            '<li class="actor"> ' +
            '   <div class="form-container helper">' +
            '      <div class="input-group">' +
            '        <label class="search-label">'+
            '         <input type="text" name="actorNames" id="' + actorcount + '" class="input" required="" placeholder="Input Actor Name Here">' +
            '         </label>'+
            '          <input type="text" name="actorRoles" id="' + actorcount + '" class="input" required="" placeholder="Input Role">' +
            '      </div>' +
            '   <button class="RemoveActorBtn">Remove Season</button> ' +
            '   </div>'+
            '</li>');
        actorcount++;
        $('.RemoveActorBtn').click(function (event) {
            console.log("does this run multiple times??")
            $(this).closest('.actor').remove();
        }); 
    });
}