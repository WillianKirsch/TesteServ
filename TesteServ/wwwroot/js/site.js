// Write your Javascript code.

function exibirConteudo() {
    document.getElementsByClassName("loader")[0].style.display = "none";
    document.getElementsByClassName("conteudo")[0].style.display = "block";
}

function exibirLoader() {
    document.getElementsByClassName("loader")[0].style.display = "block";
    document.getElementsByClassName("conteudo")[0].style.display = "none";
}