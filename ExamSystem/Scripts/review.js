function load_questions(content) {
    content = content.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    var parser = new DOMParser();
    var xml = parser.parseFromString(content, "text/xml");
    var scnum = new Number($(xml).find("totalsc").find("number").text());
    var mcnum = new Number($(xml).find("totalmc").find("number").text());
    var fqnum = new Number($(xml).find("totalfq").find("number").text());
    var dqnum = new Number($(xml).find("totaldq").find("number").text());
    var number = 0;

    if (scnum !== 0) {
        $(xml).find("singlechoice").each(function () {
            number++;
            $(".es-form").append("<div id='q_" + number + "' class='question'><div class='row'>（" + number +
                "）" + $(this).find("text").text() + "</div><span class='score' style='display:none;'>" +
                $(this).find("score").text() + "</span><span class='qid' style='display:none;'>" +
                $(this).find("qid").text() + "</span></div>");
        });
    }

    if (mcnum !== 0) {
        $(xml).find("multichoice").each(function () {
            number++;
            $(".es-form").append("<div id='q_" + number + "' class='question'><div class='row'>（" + number +
                "）" + $(this).find("text").text() + "</div><span class='score' style='display:none;'>" +
                $(this).find("score").text() + "</span><span class='qid' style='display:none;'>" +
                $(this).find("qid").text() + "</span></div>");
        });
    }

    if (fqnum !== 0) {
        $(xml).find("fillin").each(function () {
            number++;
            $(".es-form").append("<div id='q_" + number + "' class='question'><div class='row'>（" + number +
                "）" + $(this).find("text").text() + "</div><span class='score' style='display:none;'>" +
                $(this).find("score").text() + "</span><span class='qid' style='display:none;'>" +
                $(this).find("qid").text() + "</span></div>");
        });
    }

    if (dqnum !== 0) {
        $(xml).find("discuss").each(function () {
            number++;
            $(".es-form").append("<div id='q_" + number + "' class='question'><div class='row'>（" + number +
                "）" + $(this).find("text").text() + "</div><span class='score' style='display:none;'>" +
                $(this).find("score").text() + "</span><span class='qid' style='display:none;'>" +
                $(this).find("qid").text() + "</span></div>");
        });
    }
}

function load_answers(content) {
    content = content.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    var parser = new DOMParser();
    var xml = parser.parseFromString(content, "text/xml");
    var number = 0;

    $(xml).find("singlechoice").each(function () {
        number++;
        $("#q_" + number).append("<div class='row'>参考答案：" + $(this).find('answer').text() + "</div>");
        $("#q_" + number).append("<input class='sc form-control disabled' type='number' min='0' max='" + $("#q_" + number + " .score").text() + "' disabled />");
        if ($(this).find('answer').text() === $("#q_" + number + " .answer").text().replace("答案：", "")) {
            $("#q_" + number + " .sc").val(new Number($("#q_" + number + " .score").text()));
        } else {
            $("#q_" + number + " .sc").val(0);
        }
    });
    $(xml).find("multichoice").each(function () {
        number++;
        $("#q_" + number).append("<div class='row'>参考答案：" + $(this).find('answer').text() + "</div>");
        $("#q_" + number).append("<input class='sc form-control' type='number' min='0' max='" + $("#q_" + number + " .score").text() + "' disabled />");
        if ($(this).find('answer').text() === $("#q_" + number + " .answer").text().replace("答案：", "").replace(/,/g, "")) {
            $("#q_" + number + " .sc").val(new Number($("#q_" + number + " .score").text()));
        } else {
            $("#q_" + number + " .sc").val(0);
        }
    });
    $(xml).find("fillin").each(function () {
        number++;
        $("#q_" + number).append("<div class='row'>参考答案：" + $(this).find('answer').text() + "</div>");
        $("#q_" + number).append("<input class='sc form-control' type='number' min='0' max='" + $("#q_" + number + " .score").text() + "' required />");
        if ($("#q_" + number + " .answer").text().replace("答案：", "").indexOf($(this).find('answer').text()) >= 0) {
            $("#q_" + number + " .sc").val(new Number($("#q_" + number + " .score").text()));
        } else {
            $("#q_" + number + " .sc").val(0);
        }
    });
    $(xml).find("discuss").each(function () {
        number++;
        $("#q_" + number).append("<div class='row'>参考答案：" + $(this).find('answer').text() + "</div>");
        $("#q_" + number).append("<input class='sc form-control' type='number' min='0' max='" + $("#q_" + number + " .score").text() + "' required />");
        if ($("#q_" + number + " .answer").text().replace("答案：", "").indexOf($(this).find('answer').text()) >= 0) {
            $("#q_" + number + " .sc").val(new Number($("#q_" + number + " .score").text()));
        } else {
            $("#q_" + number + " .sc").val(0);
        }
    });

    $("#q_" + number).append("<a class='btn btn-primary btn-sm' href='#' onclick='submit_review()'>提交</a>");
}

function load_results(content) {
    content = content.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    var parser = new DOMParser();
    var xml = parser.parseFromString(content, "text/xml");
    var number = 0;

    $(xml).find("question").each(function () {
        number++;
        $("#q_" + number).append("<div class='row answer'>答案：" + $(this).find('answer').text() + "</div>");
    });
}

function submit_difficulty(id, d) {
    $.ajax({
        type: "Post",
        url: "/Exam/SubmitDifficulty",
        data: "qid=" + id + "&d=" + d
    });
}