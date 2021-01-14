using DR.EFCore;
using DR.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DR.Services
{
     
    /// <summary>
    /// EFcore增删改查公共方法类
    /// </summary>
    public class EFBaseService: IEFBaseService
    {
        private EFCoreContextRead contextRead = new EFCore.EFCoreContextRead();
        private  EFCoreContextWrite context = new EFCore.EFCoreContextWrite();
        /// <summary>
        /// 增加(主库操作)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add<T>(T model) where T : class
        {
            context.Entry(model).State = EntityState.Added;
            return context.SaveChanges();
        }

        /// <summary>
        /// 删除_真删(主库操作)
        /// </summary>
        /// <param name="model">修改后的实体</param>
        /// <returns></returns>
        public int TDeL<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
           
            List<T> listDeleting = context.Set<T>().Where(whereLambda).ToList();
          
            listDeleting.ForEach(u =>
            {
                context.Set<T>().Attach(u);//先附加到 EF容器
                context.Set<T>().Remove(u);//标识为 删除 状态
            });
            return context.SaveChanges();
        }
        /// <summary>
        /// 修改(主库操作)
        /// </summary>
        /// <param name="model">修改后的实体</param>
        /// <returns></returns>
        public int ModifyNo<T>(T model) where T : class
        {
            context.Entry(model).State = EntityState.Modified;
            return context.SaveChanges();
        }
        /// <summary>
        /// 多条查询(主库操作)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListWriteBy<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            return context.Set<T>().Where(whereLambda).ToList();
        }
        /// <summary>
        /// 单条查询(主库操作)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetWriteBy<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            return context.Set<T>().Single(whereLambda);
        }
        /// <summary>
        /// 查询(从库操作)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListReadBy<T>(Expression<Func<T, bool>> whereLambda) where T : class
        {
            return contextRead.Set<T>().Where(whereLambda).ToList();
        }
    }
}
