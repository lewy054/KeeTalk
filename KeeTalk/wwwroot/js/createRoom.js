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
    var defaultColor = "#a2ff7a";
    inputColor.value = defaultColor;
    inputColor.addEventListener("input", changeColor, false);
    inputColor.select();
}
function changeColor(event) {
    console.log('test')
    document.querySelectorAll("#checkColor").forEach(function (p) {
        console.log('test')
        console.log(event.target.value)
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