using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DR.EFCore
{
    public class EFCoreContextRead : DbContext
    {
        private readonly string strConn = Config.Configuration["ConnectionStrings:ReadDB"];
        public DbSet<Test> Tests { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<PictureInfo> PictureInfos { get; set; }
        public DbSet<WordInfo> WordInfos{ get; set; }
        /// <summary>
        /// 配置信息
        /// 数据库连接
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(RandConnStr());
        }

        /// <summary>
        /// 数据库结构，关系映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        /// <summary>
        /// 多个读取从库随机
        /// </summary>
        /// <returns></returns>
        private string RandConnStr()
        {
            string[] str = strConn.Split("|");
            int i = new Random().Next(0, str.Length);
            return str[i];
        }
    }
}
