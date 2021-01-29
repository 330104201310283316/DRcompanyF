$(function() {
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
					type: "Post",
					data: formData,
					contentType: false,
					processData: false,
					success: function(data) {
						fileImg.attr("src", data.data["url"]);
					},
					error: function(data) {
						alert("上传失败")
					}
				});
			}
		}
	})
})

function GetRadioValue(RadioName){
    var obj;   
    obj=document.getElementsByName(RadioName);
    if(obj!=null){
        var i;
        for(i=0;i<obj.length;i++){
            if(obj[i].checked){
                return obj[i].value;           
            }
        }
    }
    return null;
}

function submit() {
		var Url = document.getElementById('Url').src;
		var Title = document.getElementById('Title').value;
		var PictureExplain = document.getElementById('Explain').value;
		var ExplainIndex = document.getElementById('ExplainIndex').value;
		var select = document.getElementById('select').value;
		var PhotoType = GetRadioValue("girl");
		if(select=="")
		{
			alert("请选择图片类型！")
			return
		}
		if(ExplainIndex=="")
		{
			ExplainIndex=1
		}
		var body = {
			"url": Url,
			"PictureTitle": Title,
			"PictureExplain": PictureExplain,
			"Index": ExplainIndex,
			"PictureType" :select,
			 "PhotoType" :PhotoType
		};
		$.ajax({
			//请求方式
			type: "Post",
			//请求的媒体类型
			contentType: "application/json;charset=UTF-8",
			//请求地址
			url: 'http://192.168.3.49:8081/api/dr/PictureUpLoad/PictureUpload',
			//数据，json字符串
			data: JSON.stringify(body),
			async: false,
			beforeSend: function(request) {
				request.setRequestHeader("Authorization", window.sessionStorage.token);
			},
			success: function(data) {
				if (data.code == 200) {
					alert("上传成功")
					window.location.href = "/Node/PictureInfo.html";
				}
			},
			error: function(data) {
				alert("登录已失效，请重新登录！")
			}

		});

	}
	$(function() {
		$('.ExplainIndex').change(function() {
			if (this.value < 1) {
				document.getElementById('ExplainIndex').value = 1;
				alert("输入值不能小于1")
			}
			if (this.value > 5) {
				document.getElementById('ExplainIndex').value = 5;
				alert("输入值不能大于5")
			}
		})

	})