$(document).ready(function () {
    let epcount = 0;
    $("#AddEpisode").on("click", function () {
            $(".EpisodeList").append(
                '      <div class="input-group ep">' +
                '         <input type="text" required="required" minlength="3" maxlength="69" name="episodeNames" id="' + epcount + '" asp-validation-for="" class="input" required="" placeholder="Input Episode Name Here">' +
                '         <input type="file" required="required" name="episodeFiles" id="' + epcount + '" accept="video/*">' +
                '         <button class="RemoveEpisodeBtn boton-elegante">Remove Episode</button>' +
                '      </div>'
            );
            $(".RemoveEpisodeBtn").on("click", function () {
                $(this).closest(".ep").remove();
            });
    });
});
