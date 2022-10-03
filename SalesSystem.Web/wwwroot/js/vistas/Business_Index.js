$(document).ready(function () {
    $(".card-body").LoadingOverlay("show");
    fetch("/Business/Get")
        .then(response => {
            $(".card-body").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response)
    })
    .then(responseJson => {

        console.log(responseJson);

        if (responseJson.state) {
            const d = responseJson.object;
            $("#txtNumeroDocumento").val(d.numeroDocumento)
            $("#txtRazonSocial").val(d.nombre)
            $("#txtCorreo").val(d.correo)
            $("#txtDireccion").val(d.direccion)
            $("#txtTelefono").val(d.telefono)
            $("#txtImpuesto").val(d.porcentajeImpuesto)
            $("#txtSimboloMoneda").val(d.simboloMoneda)
            $("#imgLogo").attr("src", d.urlLogo)
        } else {
            swal("Lo sentimos", responseJson.message, "error")
        }
    })
})

$("#btnGuardarCambios").click(function () {
    const inputs = $("input.input-validar").serializeArray();
    const inputsNull = inputs.filter((item) => item.value.trim() == "")
    if (inputsNull.length > 0) {
        const mensaje = `Debe completar el campo : "${inputsNull[0].name}"`;
        toastr.warning("", mensaje);
        $(`input[name="${inputsNull[0].name}"]`).focus();
        return;
    }

    const model = {
        numeroDocumento : $("#txtNumeroDocumento").val(),
        nombre : $("#txtRazonSocial").val(),
        correo : $("#txtCorreo").val(),
        direccion: $("#txtDireccion").val(),
        telefono: $("#txtTelefono").val(),
        porcentajeImpuesto: $("#txtImpuesto").val(),
        simboloMoneda: $("#txtSimboloMoneda").val()
    }

    const inputLogo = document.getElementById("txtLogo");
    const formData = new FormData();
    formData.append("logo", inputLogo.files[0]);
    formData.append("model", JSON.stringify(model));

    $(".card-body").LoadingOverlay("show");

    fetch("/Business/SaveChanges", {
        method: "POST",
        body: formData
    })
    .then(response => {
        $(".card-body").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response)
    })
    .then(responseJson => {

        console.log(responseJson);

        if (responseJson.state) {
            const d = responseJson.object;
            $("#imgLogo").attr("src", d.urlLogo)
        } else {
            swal("Lo sentimos", responseJson.message, "error")
        }
    })
})