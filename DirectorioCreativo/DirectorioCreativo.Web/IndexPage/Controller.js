////(() => {

    


////})();

$(document).ready(() => {

    document.getElementById('statusDB').style.color = 'red';

    status();

})

function status()
{
    const model = { Filtro: null };

    $.ajax({
        url: '/api/Obra/Get',        
        type: 'POST',       
        contentType: "application/json",
        data: { model: model },
        success: (e) => {
            console.log(e);
        },
        error: (e) => {
            console.log(e);
        }
    })
}