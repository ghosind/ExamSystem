function logout() {
    $.ajax({
        type: "POST",
        url: "/User/Logout",
        success: function (data) {
            location.href = location.pathname;
        }
    });
}

function text_validate(text) {
    text = text.replace(/</g, "&lt;");
    text = text.replace(/>/g, "&gt;");
    return text;
}

function join_group(id) {
    $.ajax({
        type: "POST",
        url: "/Group/Join",
        data: "gid=" + id,
        success: function (data) {
            if (data === true) {
                alert("加入成功");
                location.href = "/Group/Detail/" + id;
            } else {
                alert("加入失败");
            }
        }
    });
}

function dismiss_group(id) {
    if (confirm("确定解散群组？")) {
        $.ajax({
            type: "POST",
            url: "/Group/Dismiss",
            data: "gid=" + id,
            success: function (data) {
                if (data === true) {
                    alert("群组已解散");
                    location.href = "/Group/Index";
                } else {
                    alert("解散失败");
                }
            }
        });
    }
}

function quit_group(id) {
    if (confirm("确定退出群组？")) {
        $.ajax({
            type: "POST",
            url: "/Group/Quit",
            data: "gid=" + id,
            success: function (data) {
                if (data === true) {
                    alert("已退出群组");
                    location.href = "/Group/Index";
                } else {
                    alert("退出失败");
                }
            }
        });
    }
}