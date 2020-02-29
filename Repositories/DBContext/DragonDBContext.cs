using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Appsetting.DragonConnectionString);
        }

    }
}
