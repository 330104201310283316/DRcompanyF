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
    /// 图片分类
    /// </summary>
    public enum PictureType
    {
        /// <summary>
        /// 产品
        /// </summary>
        Product=0,
        /// <summary>
        /// 资讯
        /// </summary>
        News = 1,
    }



    /// <summary>
    /// 产品图片类型
    /// </summary>
    public enum PhotoType
    {
        /// <summary>
        /// 青年
        /// </summary>
        YOUTH = 0,
        /// <summary>
        /// 优雅的
        /// </summary>
        ELEGANT = 1,
    }
}
