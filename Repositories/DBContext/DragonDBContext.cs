using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class DragonDBContext : DbContext
    {
        //public DragonDBContext(DbContextOptions<DragonDBContext> options) : base(options)
        //{
        //}
        public DbSet<User> UserList { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseMySql(Appsetting.DragonConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(s=>s.UserID);
            modelBuilder.Entity<User>().Property("UserID").HasColumnName("ID").HasColumnType("char(36)");
            modelBuilder.Entity<User>().Property("UserName").HasColumnType("varchar(50)");
            modelBuilder.Entity<User>().Property("PassWord").HasColumnType("varchar(50)");
            modelBuilder.Entity<User>().Property("CreateTime").HasColumnType("datetime");
            modelBuilder.Entity<User>().Property("Type").HasColumnType("tinyint(1)");
            modelBuilder.Entity<User>().Property("ClassId").HasColumnType("int");

            base.OnModelCreating(modelBuilder);
        }

    }

    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);
        public void Dispose() { }
    }
    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                    && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                //TODO: 拿到日志内容想怎么玩就怎么玩吧
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}
