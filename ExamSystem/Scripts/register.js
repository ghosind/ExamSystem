var email = false;
var username = false;
var password = false;

function check_can_submit() {
    if (username === true && email === true && password === true) {
        $("#submit").removeClass("disabled");
    } else if (!$("#submit").hasClass("disabled")) {
        $("#submit").addClass("disabled");
    }
}

function username_validate() {
    $.ajax({
        type: "POST",
        url: "/User/UserNameValidate",
        data: $("#username").serialize(),
        success: function (data) {
            if (data === true) {
                username = true;
            } else {
                username = false;
                $("#username").popover('show');
            }
            check_can_submit();
        }
    });
}

function email_validate() {
    $.ajax({
        type: "POST",
        url: "/User/EmailValidate",
        data: $("#email").serialize(),
        success: function (data) {
            if (data === true) {
                email = true;
            } else {
                email = false;
                $("#email").popover('show');
            }
            check_can_submit();
        }
    });
}

function password_validate() {
    if ($('#password').val() === $('#confirm').val()) {
        password = true;
    } else {
        password = false;
        $("#confirm").popover('show');
    }
    check_can_submit();
}

$("#register").submit(function (event) {
    $.ajax({
        type: "POST",
        url: "/User/RegisterWithJson",
        data: $("#register").serialize(),
        success: function (data) {
            if (data === true) {
                window.location.href = "/Index";
            } else {
                $('.es-warning').fadeIn();
            }
        }
    });
    event.preventDefault();
});