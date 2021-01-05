using DR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DR.EFCore
{
    public class EFCoreContextDR : DbContext
    {


        private readonly string strConn = Config.Configuration["ConnectionStrings:DR"];


        public DbSet<WordInfo> WordInfo { get; set; }

        /// <summary>
        /// 配置信息
        /// 数据库连接
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(strConn);
        }

        /// <summary>
        /// 数据库结构，关系映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //初始化数据
            //modelBuilder.Entity<Test>().Property(x => x.Id).ValueGeneratedNever();
        }
    }


}
