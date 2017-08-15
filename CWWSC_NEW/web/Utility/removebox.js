
    var isPrize = false;

    //验证是否可以抽奖，信息是否全
    function validateSpin() {
        //$(".chai_btn").click(function () { return false; });
        //直接抽奖
        spin();
        //设置按钮连接回复
        //$(".chai_btn").click(function () { return true; });
    }

    //输入层提交按钮
    function Save() {
        var name = $("#name").val().trim();
        var phone = $("#phone").val().trim();
        var address = $("#address").val().trim();
        if (name != null && name != "" && phone != null && phone != "" && address != "") {
            setPrizeRecord(recordid, name, phone, address, "1");
        }else{
            alert_h("请输入姓名、电话及地址。");
        }
    }

    //修改抽奖明细表中的中奖用户信息
    function setPrizeRecord(rid, name, phone, address, isedit) {
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "SetPrizeRecord", RecordId: rid, Name: name, Phone: phone, Address: address, EditMember: isedit },
            async: false,
            success: function (resultData) {
                if (resultData.Result == "OK") {
                    //开始关闭输入层
                    CloseDiv('popup2');
                }
                //window.location.reload();
                window.location.href = updateUrl(window.location.href, 'ts'); //不传参，默认是“t”
            }
        });
    }


    var realname = "";
    var cellPhone = "";
    var currAddress = "";
    //验证是否存在昵称，电话，地址
    function getMemberInfo() {
        var iexist = 0;
        $.ajax({
            url: "/API/VshopProcess.ashx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { action: "GetMemberInfo" },
            async: false,
            success: function (resultData) {
                iexist = resultData.IExist;
                realname = resultData.RealName;
                cellPhone = resultData.CellPhone;
                currAddress = resultData.Address;
            }
        });
        return iexist;
    }
    
        //抽奖方法
    function spin() {
            if (isPrize) {
                alert_h("正在抽奖请等待...");
                return;
            } 
            isPrize = true;
            var no = parseInt(getPrize());
            if (no == -1) {
                alert_h("您已经达到抽奖次数上限");
            }
            else if (no == -2) {
                alert_h("您未登录或者，登录超时，请重新从微信进入！");
            }
            else if (no == -3) {
                alert_h("对不起，活动还未开始！");
            }
            else if (no == -4) {
                alert_h("对不起，活动已经结束！");
            }
            else if (no == -9) {
                alert_h(errorMsg);//抽奖出错
            }
            else if (no == 0) {
                updateNum();
                alert_h("很遗憾,您未中奖,谢谢参与.", function () {                    
                    //window.location.reload();
                    window.location.href = updateUrl(window.location.href, 'ts'); //不传参，默认是“t”
                });
            }
            else {
                updateNum();
                //if (errorMsg != null && errorMsg != "")
                //    alert_h("恭喜您获得了 " + jeMsg + "," + errorMsg, function () { window.location.reload() });
                //else
                alert_h("恭喜您获得了 " + jeMsg, function () {
                    if (recordid != "") {
                        var iexist = parseInt(getMemberInfo());
                        if (iexist == 0) {
                            ShowDiv('popup2');//显示信息输入弹出层
                        }
                        else {
                            setPrizeRecord(recordid, realname, cellPhone, currAddress, "0");
                        }
                    }
                    else {
                        //window.location.reload();
                        window.location.href = updateUrl(window.location.href, 'ts'); //不传参，默认是“t”
                    }
                });
            }
            setTimeout(function () { isPrize = false }, 1000);
        }


        function updateUrl(url, key) {
            var key = (key || 't') + '=';  //默认key是"t",可以传入key自定义
            var reg = new RegExp(key + '\\d+');  //正则：t=1472286066028
            var timestamp = +new Date();
            if (url.indexOf(key) > -1) { //有时间戳，直接更新
                return url.replace(reg, key + timestamp);
            } else {  //没有时间戳，加上时间戳
                if (url.indexOf('\?') > -1) {
                    var urlArr = url.split('\?');
                    if (urlArr[1]) {
                        return urlArr[0] + '?' + key + timestamp + '&' + urlArr[1];
                    } else {
                        return urlArr[0] + '?' + key + timestamp;
                    }
                } else {
                    if (url.indexOf('#') > -1) {
                        return url.split('#')[0] + '?' + key + timestamp + location.hash;
                    } else {
                        return url + '?' + key + timestamp;
                    }
                }
            }
        }


        function GetActivityid() {
            var reg = new RegExp("(^|&)" + "activityid" + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

        //function GetActivityid() {
        //    alert(window.location.search);
        //    var activityid = window.location.search.substr(window.location.search.indexOf("=") + 1);
        //    if (activityid.indexOf("&") > 0)
        //        activityid = activityid.substr(0, activityid.indexOf("&"));
        //    return activityid;
        //}


        var errorMsg = "";
        var jeMsg = "";
        var recordid = "";
        function getPrize() {
            var no = 0;
            jeMsg = "";
            var activityid = GetActivityid();
            $.ajax({
                url: "/API/VshopProcess.ashx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: { action: "GetPrize", "activityid": activityid },
                async: false,
                success: function (resultData) {
                    no = resultData.No;
                    jeMsg = resultData.JE;
                    errorMsg = resultData.Msg;
                    if (resultData.RecordId != null && resultData.RecordId != "") {
                        recordid = resultData.RecordId;
                    }
                }
            });
            return no;
        }

        String.prototype.trim = function () {
            return this.replace(/(^\s*)|(\s*$)/g, '');
        }