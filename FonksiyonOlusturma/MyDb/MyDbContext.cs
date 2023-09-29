﻿using FonksiyonOlusturma.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonksiyonOlusturma.MyDb
{
    public class MyDbContext:DbContext
    {
        public DbSet<Projects> projects { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<Staffs> staffs { get; set; }
        public DbSet<Functions> functions { get; set; }
        public DbSet<Modules>modules { get; set; }  
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Status> status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Burada veritabanı bağlantı bilgilerini tanımlayın.
            // Örnek olarak SQL Server kullanalım:
            string connectionString = "Data Source=SRVMIKRO;Initial Catalog=Muh_Plan_Prog1;Integrated Security=True;Connect Timeout=10;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projects>().ToTable("Projects").HasKey(x => x.ProjectId);
            modelBuilder.Entity<Categories>().ToTable("Categories").HasKey(x=>x.CategoryID);
            modelBuilder.Entity<Staffs>().ToTable("Staffs").HasKey(x=>x.StaffId);
            modelBuilder.Entity<Functions>().ToTable("Functions").HasKey(x => x.FunctionId);
            modelBuilder.Entity<Modules>().ToTable("Modules").HasKey(x => x.ModuleId);
            modelBuilder.Entity<Accounts>().ToTable("Accounts").HasKey(x => x.KullaniciId);
            modelBuilder.Entity<Status>().ToTable("Status").HasKey(x => x.StatusId);
        }
    }

}
