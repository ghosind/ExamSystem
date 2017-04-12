var store_section = 0;
var store_type = -1;
var store = null;
var sc = 0;
var mc = 0;
var fq = 0;
var dq = 0;

function selected_group() {
    $('#group').val("");
    $(".groups:checked").each(function () {
        $('#group').val($('#group').val() + " " + $(this).val());
    });
    $(".select_group").modal("hide");
}

function load_section() {
    var subject = $("#subject").val();
    $.ajax({
        type: "POST",
        url: "/Manage/GetSectionsBySubject",
        data: "subject=" + subject,
        success: function (data) {
            if (data !== null) {
                $("#section").html("");
                for (var i = 0; i < data.length; i++) {
                    $("#section").append("<option value='" + data[i].id + "'>" + data[i].name + "</option>");
                }
            }
        }
    });
}

$("document").ready(function () {
    load_section();
});

function add(type) {
    $("#add-sc").hide();
    $("#add-mc").hide();
    $("#add-fq").hide();
    $("#add-dq").hide();
    switch (type) {
        case 0:
            $("#add-sc").show();
            break;
        case 1:
            $("#add-mc").show();
            break;
        case 2:
            $("#add-fq").show();
            break;
        case 3:
        default:
            $("#add-dq").show();
            break;
    }
    $("#questiontype").text(type);
    $(".add-question").modal("show");
}

function load_question() {
    var section = $("#section").val();
    var type = $("#questiontype").text();
    if (section !== store_section || type !== store_type) {
        $('#questions').html("");
        $.ajax({
            type: "POST",
            url: "/Exam/GetQuestions",
            data: "section=" + section + "&type=" + type,
            success: function (data) {
                if (data !== null) {
                    store_section = section;
                    store_type = type;
                    store = data;
                    for (var i = 0; i < data.length; i++) {
                        $('#questions').html($('#questions').html() + "<div class='row'><input type='radio' value='" + i + "' />" + data[i].content + "</div>");
                    }
                }
            }
        });
    }
}

function selected_question() {
    if ($('#questions > .row > input:checked') !== null) {
        var index = $('#questions > .row > input:checked').val();
        var type = $("#questiontype").text();
        var score = new Number(prompt("请输入分值"));
        switch (type) {
            case '0':
                sc++;
                var sctext = "<tr id='sc_" + sc + "'><td class='exam-table-qid'>" + store[index].qid +
                    "</td><td class='exam-table-sc-text'>" + store[index].content +
                    "</td><td class='exam-table-choice-number'>" + store[index].number +
                    "</td><td class='exam-table-sc-choice'>" + store[index].a +
                    "</td><td class='exam-table-sc-choice'>" + store[index].b + 
                    "</td><td class='exam-table-sc-choice'>" + store[index].c + 
                    "</td><td class='exam-table-sc-choice'>" + store[index].d +
                    "</td><td class='exam-table-sc-answer'>" + store[index].answer + 
                    "</td><td class='exam-table-score'>" + score + 
                    "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#sc_" + sc +
                    "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
                $('#sc_area').append(sctext);
                $('#scnum').text(new Number($('#scnum').text()) + 1);
                var scscr = new Number($('#scscr').text()) + score;
                $('#scscr').text(scscr);
                break;
            case '1':
                mc++;
                var mctext = "<tr id='mc_" + mc + "'><td class='exam-table-qid'>" + store[index].qid +
                    "</td><td class='exam-table-mc-text'>" + store[index].content +
                    "</td><td class='exam-table-choice-number'>" + store[index].number +
                    "</td><td class='exam-table-mc-choice'>" + store[index].a +
                    "</td><td class='exam-table-mc-choice'>" + store[index].b +
                    "</td><td class='exam-table-mc-choice'>" + store[index].c +
                    "</td><td class='exam-table-mc-choice'>" + store[index].d +
                    "</td><td class='exam-table-mc-answer'>" + store[index].answer +
                    "</td><td class='exam-table-score'>" + score +
                    "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#mc_" + mc +
                    "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
                $('#mc_area').append(mctext);
                $('#mcnum').text(new Number($('#mcnum').text()) + 1);
                var mcscr = new Number($('#mcscr').text()) + score;
                $('#mcscr').text(mcscr);
                break;
            case '2':
                fq++;
                var fqtext = "<tr id='fq_" + fq + "'><td class='exam-table-qid'>" + store[index].qid +
                    "</td><td class='exam-table-fq-text'>" + store[index].content +
                    "</td><td class='exam-table-fq-answer'>" + store[index].answer +
                    "</td><td class='exam-table-score'>" + score +
                    "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#fq_" + fq +
                    "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
                $('#fq_area').append(fqtext);
                $('#fqnum').text(new Number($('#fqnum').text()) + 1);
                var fqscr = new Number($('#fqscr').text()) + score;
                $('#fqscr').text(fqscr);
                break;
            case '3':
                dq++;
                var dqtext = "<tr id='dq_" + dq + "'><td class='exam-table-qid'>" + store[index].qid +
                    "</td><td class='exam-table-dq-text'>" + store[index].content +
                    "</td><td class='exam-table-dq-answer'>" + store[index].answer +
                    "</td><td class='exam-table-score'>" + score +
                    "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#dq_" + dq +
                    "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
                $('#dq_area').append(dqtext);
                $('#dqnum').text(new Number($('#dqnum').text()) + 1);
                var dqscr = new Number($('#dqscr').text()) + score;
                $('#dqscr').text(dqscr);
                break;
        }
        $('#aqnum').text(new Number($('#aqnum').text()) + 1);
        $('#aqscr').text(new Number($('#aqscr').text()) + score);
    }
    $('.load_question').modal("hide");
    $('.add-question').modal("hide");
}

function delete_question(id) {
    var score = new Number($(id + " > .exam-table-score").text());
    $('#aqnum').text(new Number($('#aqnum').text()) - 1);
    $('#aqscr').text(new Number($('#aqscr').text()) - score);

    switch (id[1]) {
        case 's':
            $('#scnum').text(new Number($('#scnum').text()) - 1);
            $('#scscr').text(new Number($('#scscr').text()) - score);
            break;
        case 'm':
            $('#mcnum').text(new Number($('#mcnum').text()) - 1);
            $('#mcscr').text(new Number($('#mcscr').text()) - score);
            break;
        case 'f':
            $('#fqnum').text(new Number($('#fqnum').text()) - 1);
            $('#fqscr').text(new Number($('#fqscr').text()) - score);
            break;
        case 'd':
            $('#dqnum').text(new Number($('#dqnum').text()) - 1);
            $('#dqscr').text(new Number($('#dqscr').text()) - score);
            break;
    }

    $(id).remove();
}

function add_question() {
    var type = $("#questiontype").text();
    var score;
    switch (type) {
        case '0':
            sc++;
            score = new Number($('#add-sc input[name=score]').val());
            var sctext = "<tr id='sc_" + sc + "'><td class='exam-table-qid'>" + 0 +
                "</td><td class='exam-table-sc-text'>" + $('#add-sc input[name=text]').val() +
                "</td><td class='exam-table-choice-number'>" + $('#add-sc input[name=number]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-sc input[name=choicea]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-sc input[name=choiceb]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-sc input[name=choicec]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-sc input[name=choiced]').val() +
                "</td><td class='exam-table-sc-answer'>" + $('#add-sc input[name=answer]').val() +
                "</td><td class='exam-table-score'>" + score +
                "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#sc_" + sc +
                "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
            $('#sc_area').append(sctext);
            $('#scnum').text(new Number($('#scnum').text()) + 1);
            var scscr = new Number($('#scscr').text()) + score;
            $('#scscr').text(scscr);
            break;
        case '1':
            mc++;
            score = new Number($('#add-mc input[name=score]').val());
            var answer = "";
            $('#add-mc input[name=answer]:checked').each(function () { answer += $(this).val(); });
            var mctext = "<tr id='mc_" + mc + "'><td class='exam-table-qid'>" + 0 +
                "</td><td class='exam-table-sc-text'>" + $('#add-mc input[name=text]').val() +
                "</td><td class='exam-table-choice-number'>" + $('#add-mc input[name=number]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-mc input[name=choicea]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-mc input[name=choiceb]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-mc input[name=choicec]').val() +
                "</td><td class='exam-table-sc-choice'>" + $('#add-mc input[name=choiced]').val() +
                "</td><td class='exam-table-sc-answer'>" + answer +
                "</td><td class='exam-table-score'>" + score +
                "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#mc_" + mc +
                "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
            $('#mc_area').append(mctext);
            $('#mcnum').text(new Number($('#mcnum').text()) + 1);
            var mcscr = new Number($('#mcscr').text()) + score;
            $('#mcscr').text(mcscr);
            break;
        case '2':
            fq++;
            score = new Number($('#add-fq input[name=score]').val());
            var fqtext = "<tr id='fq_" + fq + "'><td class='exam-table-qid'>" + 0 +
                "</td><td class='exam-table-fq-text'>" + $('#add-fq input[name=text]').val() +
                "</td><td class='exam-table-fq-answer'>" + $('#add-fq input[name=answer]').val() +
                "</td><td class='exam-table-score'>" + score +
                "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#fq_" + fq +
                "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
            $('#fq_area').append(fqtext);
            $('#fqnum').text(new Number($('#fqnum').text()) + 1);
            var fqscr = new Number($('#fqscr').text()) + score;
            $('#fqscr').text(fqscr);
            break;
        case '3':
            dq++;
            score = new Number($('#add-dq input[name=score]').val());
            var dqtext = "<tr id='dq_" + dq + "'><td class='exam-table-qid'>" + 0 +
                "</td><td class='exam-table-dq-text'>" + $('#add-dq input[name=text]').val() +
                "</td><td class='exam-table-dq-answer'>" + $('#add-dq input[name=answer]').val() +
                "</td><td class='exam-table-score'>" + score +
                "</td><td><button class='btn btn-sm btn-primary btn-danger' onclick=\"delete_question('#dq_" + dq +
                "')\"><span class='glyphicon glyphicon-remove'></span></button></td></tr>";
            $('#dq_area').append(dqtext);
            $('#dqnum').text(new Number($('#dqnum').text()) + 1);
            var dqscr = new Number($('#dqscr').text()) + score;
            $('#dqscr').text(dqscr);
            break;
    }
    $('#aqnum').text(new Number($('#aqnum').text()) + 1);
    $('#aqscr').text(new Number($('#aqscr').text()) + score);
    $('.add-question').modal("hide");
}

function generate_exam() {
    var exam_xml = "";
    var result_xml = "";
    var number = 0;

    exam_xml += "[lt]exam[rt]";
    result_xml += "[lt]exam[rt]";
    exam_xml += "[lt]header[rt]";
    exam_xml += "[lt]totalsc[rt]";
    exam_xml += "[lt]number[rt]" + $('#scnum').text() + "[lt]/number[rt]";
    exam_xml += "[lt]score[rt]" + $('#scscr').text() + "[lt]/score[rt]";
    exam_xml += "[lt]/totalsc[rt]";
    exam_xml += "[lt]totalmc[rt]";
    exam_xml += "[lt]number[rt]" + $('#mcnum').text() + "[lt]/number[rt]";
    exam_xml += "[lt]score[rt]" + $('#mcscr').text() + "[lt]/score[rt]";
    exam_xml += "[lt]/totalmc[rt]";
    exam_xml += "[lt]totalfq[rt]";
    exam_xml += "[lt]number[rt]" + $('#fqnum').text() + "[lt]/number[rt]";
    exam_xml += "[lt]score[rt]" + $('#fqscr').text() + "[lt]/score[rt]";
    exam_xml += "[lt]/totalfq[rt]";
    exam_xml += "[lt]totaldq[rt]";
    exam_xml += "[lt]number[rt]" + $('#dqnum').text() + "[lt]/number[rt]";
    exam_xml += "[lt]score[rt]" + $('#dqscr').text() + "[lt]/score[rt]";
    exam_xml += "[lt]/totaldq[rt]";
    exam_xml += "[lt]total[rt]";
    exam_xml += "[lt]number[rt]" + $('#aqnum').text() + "[lt]/number[rt]";
    exam_xml += "[lt]score[rt]" + $('#aqscr').text() + "[lt]/score[rt]";
    exam_xml += "[lt]/total[rt]";
    exam_xml += "[lt]/header[rt]";
    exam_xml += "[lt]content[rt]";
    $('#sc_area tr').each(function () {
        exam_xml += "[lt]singlechoice[rt]";
        result_xml += "[lt]singlechoice[rt]";
        number++;
        exam_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        result_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        exam_xml += "[lt]qid[rt]" + this.childNodes[0].innerHTML + "[lt]/qid[rt]";
        exam_xml += "[lt]text[rt]" + this.childNodes[1].innerHTML + "[lt]/text[rt]";
        exam_xml += "[lt]choicenumber[rt]" + this.childNodes[2].innerHTML + "[lt]/choicenumber[rt]";
        exam_xml += "[lt]choicea[rt]" + this.childNodes[3].innerHTML + "[lt]/choicea[rt]";
        exam_xml += "[lt]choiceb[rt]" + this.childNodes[4].innerHTML + "[lt]/choiceb[rt]";
        exam_xml += "[lt]choicec[rt]" + this.childNodes[5].innerHTML + "[lt]/choicec[rt]";
        exam_xml += "[lt]choiced[rt]" + this.childNodes[6].innerHTML + "[lt]/choiced[rt]";
        result_xml += "[lt]answer[rt]" + this.childNodes[7].innerHTML + "[lt]/answer[rt]";
        exam_xml += "[lt]score[rt]" + this.childNodes[8].innerHTML + "[lt]/score[rt]";
        exam_xml += "[lt]/singlechoice[rt]";
        result_xml += "[lt]/singlechoice[rt]";
    });
    $('#mc_area tr').each(function () {
        exam_xml += "[lt]multichoice[rt]";
        result_xml += "[lt]multichoice[rt]";
        number++;
        exam_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        result_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        exam_xml += "[lt]qid[rt]" + this.childNodes[0].innerHTML + "[lt]/qid[rt]";
        exam_xml += "[lt]text[rt]" + this.childNodes[1].innerHTML + "[lt]/text[rt]";
        exam_xml += "[lt]choicenumber[rt]" + this.childNodes[2].innerHTML + "[lt]/choicenumber[rt]";
        exam_xml += "[lt]choicea[rt]" + this.childNodes[3].innerHTML + "[lt]/choicea[rt]";
        exam_xml += "[lt]choiceb[rt]" + this.childNodes[4].innerHTML + "[lt]/choiceb[rt]";
        exam_xml += "[lt]choicec[rt]" + this.childNodes[5].innerHTML + "[lt]/choicec[rt]";
        exam_xml += "[lt]choiced[rt]" + this.childNodes[6].innerHTML + "[lt]/choiced[rt]";
        result_xml += "[lt]answer[rt]" + this.childNodes[7].innerHTML + "[lt]/answer[rt]";
        exam_xml += "[lt]score[rt]" + this.childNodes[8].innerHTML + "[lt]/score[rt]";
        exam_xml += "[lt]/multichoice[rt]";
        result_xml += "[lt]/multichoice[rt]";
    });
    $('#fq_area tr').each(function () {
        exam_xml += "[lt]fillin[rt]";
        result_xml += "[lt]fillin[rt]";
        number++;
        exam_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        result_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        exam_xml += "[lt]qid[rt]" + this.childNodes[0].innerHTML + "[lt]/qid[rt]";
        exam_xml += "[lt]text[rt]" + this.childNodes[1].innerHTML + "[lt]/text[rt]";
        result_xml += "[lt]answer[rt]" + this.childNodes[2].innerHTML + "[lt]/answer[rt]";
        exam_xml += "[lt]score[rt]" + this.childNodes[3].innerHTML + "[lt]/score[rt]";
        exam_xml += "[lt]/fillin[rt]";
        result_xml += "[lt]/fillin[rt]";
    });
    $('#dq_area tr').each(function () {
        exam_xml += "[lt]discuss[rt]";
        result_xml += "[lt]discuss[rt]";
        number++;
        exam_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        result_xml += "[lt]number[rt]" + number + "[lt]/number[rt]";
        exam_xml += "[lt]qid[rt]" + this.childNodes[0].innerHTML + "[lt]/qid[rt]";
        exam_xml += "[lt]text[rt]" + this.childNodes[1].innerHTML + "[lt]/text[rt]";
        result_xml += "[lt]answer[rt]" + this.childNodes[2].innerHTML + "[lt]/answer[rt]";
        exam_xml += "[lt]score[rt]" + this.childNodes[3].innerHTML + "[lt]/score[rt]";
        exam_xml += "[lt]/discuss[rt]";
        result_xml += "[lt]/discuss[rt]";
    });
    exam_xml += "[lt]/content[rt]";
    exam_xml += "[lt]/exam[rt]";
    result_xml += "[lt]/exam[rt]";

    var result = new Array(2);
    result[0] = exam_xml.replace(/&lt;/g, "[flt]").replace(/&rt;/g, "[frt]");
    result[1] = result_xml.replace(/&lt;/g, "[flt]").replace(/&rt;/g, "[frt]");
    return result;
}

function save_exam() {
    var file = generate_exam();
    $.ajax({
        type: "POST",
        url: "/Exam/SaveExam",
        data: $('#info').serialize() + "&exam_file=" + file[0] + "&result_file=" + file[1],
        success: function (data) {
            if (data !== 0) {
                alert("添加成功");
                location.href = "/Exam/Detail/" + data;
            } else {
                alert("添加失败");
            }
        }
    });
}