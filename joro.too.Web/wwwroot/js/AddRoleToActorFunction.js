$(document).ready(function () {
    let bol = $(".selctthingie").find("option:selected").attr("name");
    let thing = bol.substring(0, bol.length - 1);
    $(".isshow").val(thing)
    $(".selctthingie").on("change", function () {
        console.log("mrazq cigani")
        let bol = $(".selctthingie").find("option:selected").attr("name");
        let thing = bol.substring(0, bol.length - 1);
        $(".isshow").val(thing)
        console.log(thing)
    })
});