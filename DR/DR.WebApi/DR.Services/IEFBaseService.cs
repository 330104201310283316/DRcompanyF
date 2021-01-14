using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DR.Services
{
  public  interface IEFBaseService
    {
        /// <summary>
        /// 主库公共新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add<T>(T model) where T : class;
        /// <summary>
        /// 主库公共修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        //int Modify<T>(Expression<Func<T, bool>> whereLambda) where T : class;
        /// <summary>
        /// 多条主库公共查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> GetListWriteBy<T>(Expression<Func<T, bool>> whereLambda) where T : class;
        /// <summary>
        /// 单条主库公共查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        T GetWriteBy<T>(Expression<Func<T, bool>> whereLambda) where T : class;

        /// <summary>
        /// 主库公共修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        int ModifyNo<T>(T model) where T : class;
        /// <summary>
        /// 主库公共删除_真删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        int TDeL<T>(Expression<Func<T, bool>> whereLambda) where T : class;

        /// <summary>
        /// 从库公共查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        List<T> GetListReadBy<T>(Expression<Func<T, bool>> whereLambda) where T : class;


    }
}
