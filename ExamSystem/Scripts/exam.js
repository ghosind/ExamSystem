function load_questions(content) {
    content = content.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    var parser = new DOMParser();
    var xml = parser.parseFromString(content, "text/xml");
    var scnum = new Number($(xml).find("totalsc").find("number").text());
    var mcnum = new Number($(xml).find("totalmc").find("number").text());
    var fqnum = new Number($(xml).find("totalfq").find("number").text());
    var dqnum = new Number($(xml).find("totaldq").find("number").text());
    var number = 0;

    if (scnum != 0) {
        $(".es-exam-content").append("<h4>单项选择题（共" + scnum + "题，" +
            $(xml).find("totalsc").find("score").text() + "分。下列每题给出的选项中，只有一个是符合题目要求的）</h4>");
        
        $(xml).find("singlechoice").each(function () {
            number++;
            $(".es-exam-content").append("<fieldset><label class='es-question'>（" + number +
                "）" + $(this).find("text").text() + "</label>");
            var choice_number = new Number($(this).find("choicenumber").text());
            for (var i = 0; i < choice_number; i++) {
                $(".es-exam-content").append("<input class='es-answer' name='q" + number +
                    "' type='radio' value='" + (i + 1) + "'>" +
                    $(this).find("choice" + String.fromCharCode(97 + i)).text() + "<br />");
            }
            $(".es-exam-content").append("<fieldset>");
        });
    }

    if (mcnum != 0) {
        $(".es-exam-content").append("<h4>多项选择题（共" + mcnum + "题，" +
            $(xml).find("totalmc").find("score").text() +
            "分。下列每题给出的选项中，有一个或一个以上是符合题目要求的，多选漏选均不得分）</h4>");

        $(xml).find("multichoice").each(function () {
            number++;
            $(".es-exam-content").append("<fieldset><label class='es-question'>（" + number +
                "）" + $(this).find("text").text() + "</label>");
            var choice_number = new Number($(this).find("choicenumber").text());
            for (var i = 0; i < choice_number; i++) {
                $(".es-exam-content").append("<input class='es-answer' name='q" + number +
                    "' type='checkbox' value='" + (i + 1) + "'>" +
                    $(this).find("choice" + String.fromCharCode(97 + i)).text() + "<br />");
            }
            $(".es-exam-content").append("<fieldset>");
        });
    }

    if (fqnum != 0) {
        $(".es-exam-content").append("<h4>填空题（共" + fqnum + "题，" +
            $(xml).find("totalfq").find("score").text() +
            "分。）</h4>");

        $(xml).find("fillin").each(function () {
            number++;
            $(".es-exam-content").append("<fieldset><label class='es-question'>（" + number +
                "）" + $(this).find("text").text() + "</label>");
            $(".es-exam-content").append("<input class='form-control' name='q" + number +
                "' type='text' /><br /><fieldset>");
        });
    }

    if (dqnum != 0) {
        $(".es-exam-content").append("<h4>问答题（共" + dqnum + "题，" +
            $(xml).find("totaldq").find("score").text() +
            "分。）</h4>");

        $(xml).find("discuss").each(function () {
            number++;
            $(".es-exam-content").append("<fieldset><label class='es-question'>（" + number +
                "）" + $(this).find("text").text() + "</label>");
            $(".es-exam-content").append("<textarea class='form-control' name='q" + number +
                "' row='10' /><br /><fieldset>");
        });
    }

    $(".es-exam-content").append("<input type='hidden' name='number' value='" + number + "' />");
}

