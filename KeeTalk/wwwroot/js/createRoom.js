window.addEventListener("load", startup, false);

function changeImage() {
    var imgUpload = document.getElementById("imgUpload");
    const [file] = imgUpload.files
    console.log("before if");
    if (file) {
        console.log("if");
        imageUpload.src = URL.createObjectURL(file);
    }
}

function startup() {
    var inputColor = document.querySelector("#inputColor");
    var defaultColor = "#7abfff";
    inputColor.value = defaultColor;
    inputColor.addEventListener("input", changeColor, false);
    inputColor.select();
    document.querySelectorAll("#checkColor").forEach(function (p) {
        checkColor.style.backgroundColor = defaultColor;
    });
}
function changeColor(event) {
    document.querySelectorAll("#checkColor").forEach(function (p) {
        checkColor.style.backgroundColor = event.target.value;
    });
}
function changeDescription(input) {
    console.log("change changeDescription")
    checkDescription = document.getElementById("checkDescription");
    checkDescription.innerHTML = input.value;
}
function changeName(input) {
    console.log("change changeName")
    checkName = document.getElementById("checkName");
    checkName.innerHTML = input.value;
}