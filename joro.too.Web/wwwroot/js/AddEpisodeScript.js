$(document).ready(function () {
    let epcount = 0;
    $("").on("click", function () {
            $(".EpisodeList").append(
                '      <div class="input-group ep">' +
                '         <input type="text" name="episodeNames" id="' + epcount + '" asp-validation-for="" class="input" required="" placeholder="Input Episode Name Here">' +
                '         <input type="file" name="episodeFiles" id="' + epcount + '" accept="video/*">' +
                '         <button class="RemoveEpisodeBtn">Remove Episode</button>' +
                '      </div>'
            );
            $(".RemoveEpisodeBtn").on("click", function () {
                $(this).closest(".ep").remove();
            });
    });
}
