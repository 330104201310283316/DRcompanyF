﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// 返回到前端的状态码和信息
    /// </summary>
    public enum CodeAndMessage
    {
        #region 基础
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 未登录
        /// </summary>
        请登录后访问 = 401,
        /// <summary>
        /// 权限不足
        /// </summary>
        权限不足 = 403,
        /// <summary>
        /// 系统报错
        /// </summary>
        UnKnownError = 500,
        #endregion

        #region 身份验证相关
        /// <summary>
        /// Token失效
        /// </summary>
        Token已过期请重新登录 = 40101,
        /// <summary>
        /// 账号密码错误
        /// </summary>
        用户名不存在 = 4002,
        /// <summary>
        /// 超时
        /// </summary>
        注册时间已经超过2小时 = 4003,
        /// <summary>
        /// 成功
        /// </summary>
        注册成功 = 4004,
        /// <summary>
        /// 重复
        /// </summary>
        用户名重复 = 4005,
      
        /// <summary>
        /// 账号密码错误
        /// </summary>
        密码错误 = 4006,
        /// <summary>
        /// 重复账号在俩小时内注册
        /// </summary>
        重复邮箱在俩小时内注册 = 4007,
        #endregion

        /// <summary>
        /// 修改失败
        /// </summary>
        修改失败 = 5001,
        /// <summary>
        /// 新增失败
        /// </summary>
        新增失败 = 5002,
        /// <summary>
        /// 已存在word文档，只能修改
        /// </summary>
        已存在对应的资讯文档 = 6001,
    }
}
