using System;
using System.Collections.Generic;
using System.Text;
using DR.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DR.EFCore
{
    public class EFCoreContextWrite : DbContext
    {
        private readonly string strConn = Config.Configuration["ConnectionStrings:WriteDB"];
        public DbSet<Test> Tests { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<PictureInfo> PictureInfo { get; set; }
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

        private void Seed(EFCoreContextWrite context)
        {
            Settings Settings = new Settings()
            {
                Id = 1,
                ConfigKey = "Consul",
                ConfigValues = "{\"ConsulUrl\":\"http://localhost:8500\",\"ConsulInterval\":1,\"ConsulTimeout\":10}"
            };
            context.Settings.Add(Settings);
            context.SaveChanges();
        }

    }
}
