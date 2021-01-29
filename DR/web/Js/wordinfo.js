//注意：若将代码写到$(function(){})中，则里面的所有方法都不是全局的，故此a标签中的onclick调用不到里面的方法！！！。
//总记录数
var totalNum = 0;

//总页数
var totalPage = 0;

//默认每页显示数
var avageNum = 10

//默认显示第一页
var currentPage = 1;


getUserList(currentPage, avageNum);
initPagination(totalNum, totalPage);


function selectList() {
	var start = $("#start").val();
	var end = $("#end").val();
	getUserList(currentPage, avageNum, start, end)
}

//与后台交互获取数据，异步加载到页面上
function getUserList(pageNum, pageSize, StartTime, EndTime) {
	currentPage = pageNum;
	console.log(pageNum + "," + pageSize)
	$(".panel-body table tbody").html(" ");
	var body = {
		"StartTime": StartTime,
		"EndTime": EndTime,
		"pageNum": pageNum,
		"pageSize": pageSize
	};
	$.ajax({
		//请求方式
		type: "Post",
		//请求的媒体类型
		contentType: "application/json;charset=UTF-8",
		//请求地址
		url: 'http://192.168.3.49:8081/api/dr/UpLoadWord/WordInfo',
		//数据，json字符串
		data: JSON.stringify(body),
		beforeSend: function(request) {
			request.setRequestHeader("Authorization", window.sessionStorage.token);
		},
		success: function(data) {

			if (data.code == 200) {
				var length = data["total"];
				if (length == 0) {
					$(".userListTable tbody").append(
						'<p style="font-size:20px;color:red; ">暂无数据</p>'
					)
				}
				for (var i = 0; i < data.data.length; i++) {

					var userId = data.data[i]["id"];
					var userName = data.data[i]["userName"];
					var createtime = data.data[i]["createTime"];
					var htmlTitle = data.data[i]["htmlTitle"];
					var status = data.data[i]["articleType"];
					var htmlExplain = data.data[i]["htmlExplain"];
					var pictureTitle = data.data[i]["pictureTitle"];
					if (status == 0) {
						status = "丝绸"
					} else {
						status = "衣服"
					}

					$(".userListTable tbody").append(
						'<tr>' +
						'<td id="userId" style="display: none;">' + userId + '</td>' +
						'<td>' + userName + '</td>' +
						'<td>' + pictureTitle + '</td>' +
						'<td>' + htmlTitle + '</td>' +
						'<td>' + htmlExplain + '</td>' +
						'<td>' + createtime + '</td>' +
						'<td>' + createtime + '</td>' +
						'<td><button type="submit" class="btn btn-primary" onclick="edit(this)">修改</button> </td>' +
						'</tr>'
					)
				}

				var count = Math.ceil(data["total"] / pageSize);
				if (count == 0) {
					count = 1;
				}
				initPagination(count, data["total"]);
			}
		},
		error: function(data) {
			alert("登录已失效，请重新登录！")
		}

	});
}

//初始化分页栏
function initPagination(totalPage, totalNum) {
	$('#pagination').html(" ");
	$('#pagination').append(
		'<ul class="pagination" style="display:inline;">' +
		'<span style="float: right; ">每页显示' +
		'<select style="width:50px;border-radius:5px;" id="dataNum">' +
		'<option value="10">10</option>' +
		'<option value="20">20</option>' +
		'<option value="30">30</option>' +
		'</select>条记录,总共有' + totalPage + '页，总共' + totalNum + '条记录</span>' +
		'</ul>'
	)
	$("#pagination ul").append(
		'<li style="list-style-type:none; border-radius:30px;" ><a style="border-radius:12px;float:left;background-color: #e6e6e6;padding-bottom:4px;text-decoration: none;opacity: 0.65;padding-top:4px;padding-left:12px;padding-right:12px;border-color: rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.25);border: 1px solid #cccccc;font-size: 14px;" href="javascript:void(0);" id="prev">上一页</a>'
	)
	$("#pagination ul").append(
		'<li style="list-style-type:none;border-radius:30px;"><a style="border-radius:12px;float:left; background-color: #e6e6e6;padding-bottom:4px;text-decoration: none;opacity: 0.65; padding-top:4px;padding-left:12px;padding-right:12px;border-color: rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.1) rgba(0, 0, 0, 0.25);border: 1px solid #cccccc;font-size: 14px;" href="javascript:void(0);"  id="next">下一页</a>'
	)
	$("select option[value=" + avageNum + "]").attr('selected', true)
	$("#page1").addClass("active");
	checkA();
}

//很关键，因为执行initPagination方法时，将select删除再重新添加，所以需要先将select上的结点移除off
//然后再绑定结点on，如果不这么做，会出现change事件只被触发一次。
$(document).off('change', '#dataNum').on('change', '#dataNum', function() {
	avageNum = $(this).children('option:selected').val();
	currentPage = 1;
	getUserList(currentPage, avageNum);
	initPagination(totalPage, totalNum);
});

//设置分页栏点击时候的高亮
$("#pagination").on("click", "li", function() {
	var aText = $(this).find('a').html();
	checkA();
	if (aText == "上一页") {
		$(".pagination > li").removeClass("active");
		$("#page" + (currentPage - 1)).addClass("active");
		getUserList(currentPage - 1, avageNum);
		checkA();
	} else if (aText == "下一页") {
		$(".pagination > li").removeClass("active");
		$("#page" + (currentPage + 1)).addClass("active");
		getUserList(currentPage + 1, avageNum);
		checkA();
	} else {
		$(".pagination > li").removeClass("active");
		$(this).addClass("active");
	}
})

//因为其他地方都需要用到这两句，所以封装出来
//主要是用于检测当前页如果为首页，或者尾页时，上一页和下一页设置为不可选中状态
function checkA() {
	currentPage == 1 ? $("#prev").addClass("disabled") : $("#prev").removeClass("disabled");
	currentPage == totalPage ? $("#next").addClass("disabled") : $("#next").removeClass(" disabled");
}

function edit(id) {
	var tr = $(id).closest('tr'); //找到tr元素
	var getId = tr.find('td:eq(0)').html(); //找到td元素
	sessionStorage.setItem('EID', getId);
	window.location.href = "../Kindeditor/examples/demo-09.html";
}
