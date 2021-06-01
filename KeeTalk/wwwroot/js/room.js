function clearText() {
    document.getElementById("content_text").textContent = "";
}
document.addEventListener("DOMContentLoaded", function () {
    var objDiv = document.getElementById("chat_window");
    objDiv.scrollTop = objDiv.scrollHeight;
});
