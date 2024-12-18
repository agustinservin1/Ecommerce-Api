﻿using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly bool _isTestingEnvironment;
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payments> Payments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool isTestingEnviroment = false) : base(options)
        {
            _isTestingEnvironment = isTestingEnviroment;

            var dbCreator = this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbCreator != null)
            {
                // Verificar si la base de datos no existe
                if (!this.Database.EnsureCreated())
                {
                    // Solo ejecuta si no existe
                    if (!dbCreator.CanConnect())
                    {
                        dbCreator.Create();
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            modelBuilder.Entity<Payments>().ToTable("Payments");


            var UserType = new EnumToStringConverter<Role>();
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion(UserType);

            var UserStatus = new EnumToStringConverter<Status>();
            modelBuilder.Entity<User>()
                .Property(e => e.Status)
                .HasConversion(UserStatus);

            var ProductStatus = new EnumToStringConverter<Status>();
            modelBuilder.Entity<Product>()
                .Property(e => e.Status)
                .HasConversion(ProductStatus);

            var OrderStatus = new EnumToStringConverter<StatusOrder>();
            modelBuilder.Entity<Order>()
                .Property(e => e.StatusOrder)
                .HasConversion(OrderStatus);

            var PaymentStatusEnum = new EnumToStringConverter<PaymentStatusEnum>();
            modelBuilder.Entity<Payments>()
                .Property(e => e.PaymentStatus)
                .HasConversion(PaymentStatusEnum);

            modelBuilder.Entity<Order>()
                .HasOne(s => s.User)
                .WithMany(u => u.OrdersList)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<OrderDetail>()
                .HasOne(sd => sd.Order)
                .WithMany(s => s.Details)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<OrderDetail>()
                .HasOne(sd => sd.Product)
                .WithMany(p => p.OrderDetailsList)
                .OnDelete(DeleteBehavior.Restrict); 


            modelBuilder.Entity<Product>()
                        .HasOne(p => p.Category)              
                        .WithMany(c => c.Products)              
                        .HasForeignKey(p => p.CategoryId)   
                        .IsRequired(); 

            modelBuilder.Entity<Order>()
                        .HasOne(o => o.Payment)
                        .WithOne(p => p.Order)
                        .HasForeignKey<Payments>(p => p.OrderId);

            modelBuilder.Entity<Payments>() .HasKey(p => p.Id); 
        
        
        }  
    
    }


}
   