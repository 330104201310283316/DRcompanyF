using System;
using System.Collections.Generic;
using System.Text;

namespace DR.Models
{
    /// <summary>
    /// Role
    /// </summary>
    public enum AuthRole
    {
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 0,
        /// <summary>
        /// 用户
        /// </summary>
        User = 1
    }
    /// <summary>
    /// 登录类型
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 限制账号
        /// </summary>
        LimitWeb = 0,
        /// <summary>
        /// 不限制账号
        /// </summary>
        FreeWeb = 1,

    }
    /// <summary>
    /// 推荐指数
    /// </summary>
    public enum RecommendIndex
    {
        /// <summary>
        ///一星
        /// </summary>
        one = 1,
        /// <summary>
        /// 二星
        /// </summary>
        Two = 2,
        /// <summary>
        /// 三星
        /// </summary>
        Three = 3,
        /// <summary>
        /// 四星
        /// </summary>
        Four = 4,
        /// <summary>
        ///五星
        /// </summary>
        Five = 5,

    }
    /// <summary>
    /// 文章类型，暂保留
    /// </summary>
    public enum ArticleType
    {
    }
}
