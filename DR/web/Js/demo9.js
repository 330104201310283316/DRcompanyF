function submit() {
		var Title = $(document.getElementById('Title')).val();
		var Explain = $(document.getElementById('Explain')).val();
		var EID = sessionStorage.getItem('EID');
		var ID = sessionStorage.getItem('ID');
		var html = $(document.getElementsByTagName('iframe')[0].contentWindow.document.body).html();
		var body = {
			"htmlTitle": Title,
			"htmlExplain": Explain,
			"htmlContent": html,
			"ID": ID,
			"Eid": EID
		};
		$.ajax({
			//请求方式
			type: "Post",
			//请求的媒体类型
			contentType: "application/json;charset=UTF-8",
			//请求地址
			url: 'http://192.168.3.49:8081/api/dr/UpLoadWord/WordsUpLoad',
			//数据，json字符串
			data: JSON.stringify(body),
			async: false,
			beforeSend: function(request) {
				request.setRequestHeader("Authorization", window.sessionStorage.token);
			},
			success: function(data) {
				if (data.code == 200) {
					alert("上传成功")
					window.location.href="/Node/WordInfo.html";
				}else{
					alert(data.message+"，只能修改了！")
				}
			},
			error: function(data) {
				alert("登录已失效，请重新登录！")
				
			}

		});

	}
	$(function() {
		upload();
		$(".file").change(function() {
			var fileImg = $(".fileImg");
			var explorer = navigator.userAgent;
			var imgSrc = $(this)[0].value;
			if (explorer.indexOf('MSIE') >= 0) {
				if (!/\.(jpg|jpeg|png|JPG|PNG|JPEG)$/.test(imgSrc)) {
					imgSrc = "";
					fileImg.attr("src", "/img/default.png");
					return false;
				} else {
					fileImg.attr("src", imgSrc);
				}
			} else {
				if (!/\.(jpg|jpeg|png|JPG|PNG|JPEG)$/.test(imgSrc)) {
					imgSrc = "";
					fileImg.attr("src", "/img/default.png");
					return false;
				} else {
					var file = $(this)[0].files[0];
					var url = URL.createObjectURL(file);
					var formData = new FormData();
					formData.append('file', document.getElementById('photoFile').files[0]);
					$.ajax({
						url: "http://192.168.3.49:8081/api/dr/UpLoadWord/UploadFileAsync",
						type: "post",
						data: formData,
						contentType: false,
						processData: false,
						success: function(data) {
							$(document.getElementsByTagName('iframe')[0].contentWindow.document.body).append(
								"<img  src='" + data.data["url"] + "' class='fileImg' hspace='5' vspace='5'> ");
						},
						error: function(data) {
							alert("上传失败")
						}
					});
				}

			}
		});

		function upload() {
			var ID = sessionStorage.getItem('EID')
			if (ID != "") {
				var body = {
					"ID": ID,
				};
				$.ajax({
					//请求方式
					type: "Get",
					//请求的媒体类型
					contentType: "application/json;charset=UTF-8",
					//请求地址
					url: 'http://192.168.3.49:8081/api/dr/UpLoadWord/WordDetails',
					//数据，json字符串
					data: body,
					async: false,
					beforeSend: function(request) {
						request.setRequestHeader("Authorization", window.sessionStorage.token);
					},
					success: function(data) {
						var html = data.data["htmlContent"];
						$('#Title').val(data.data["HtmlTitle"]);
						var Title=	$(document.getElementById('Title'));
						Title.val(data.data["htmlTitle"]);
						$(document.getElementById('Explain')).val(data.data["htmlExplain"]);
						$(document.getElementsByTagName('iframe')[0].contentWindow.document.body).html(html);

					},
					error: function(data) {
						alert("登录已失效，请重新登录！")
					}

				});

			}
		};

	})