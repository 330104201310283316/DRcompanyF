function login() {
	if (window != top) {
		parent.location.href = location.href;
	}
	var username = $('input[name="username"]').val();
	var password = $('input[name="password"]').val();
	if (username == "" | password == "") {
		alert("账号或密码不能为空！！")
		return;
	}
	var body = {
		"UserName": username,
		"Password": password
	};
	$.ajax({
		//请求方式
		type: "Post",
		//请求的媒体类型
		contentType: "application/json;charset=UTF-8",
		//请求地址
		url: 'http://192.168.3.49:8081/api/dr/Login/AuthLogin',
		//数据，json字符串
		data: JSON.stringify(body),
		//请求成功
		success: function(result) {
			switch (result.data["authRoles"]) {
				case 0:
					sessionStorage.setItem('token', result.data["token"])
					window.location.href = "../index.html"
					break;
				case 1:
					alert("您不是管理员!")
					break;
				default:
					alert("账号密码不正确!")
					break;
			}
		},
		//请求失败，包含具体的错误信息
		error: function(e) {
			console.log(e.status);
			console.log(e.responseText);
		}
	});
}
