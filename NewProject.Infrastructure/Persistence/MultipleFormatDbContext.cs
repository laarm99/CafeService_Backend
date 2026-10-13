using Microsoft.EntityFrameworkCore;
using MultipleFormat.Domain.Entities;
using MultipleFormat.Domain.Entities.FlowRequests;
using NewProject.Domain.Entities;
using NewProject.Domain.Entities.IDM;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewProject.Infrastructure.Persistence
{
    public class MultipleFormatDbContext : DbContext
    {
        public MultipleFormatDbContext(DbContextOptions<MultipleFormatDbContext> options) : base(options)
        {
        }

        public DbSet<IDMAccount> IDMAccounts { get; set; }
        public DbSet<IDMRole> IDMRoles { get; set; }
        public DbSet<IDMAccountRole> IDMAccountRoles { get; set; }
        public DbSet<FormRequest> FormRequests { get; set; }
        public DbSet<VacationRequest> VacationRequests { get; set; }
        public DbSet<PermissionRequest> PermissionRequests { get; set; }
        public DbSet<TimeAdjustmentRequest> TimeAdjustmentRequests { get; set; }
        public DbSet<DepartmentShiftChangeRequest> DepartmentShiftChangeRequests { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IDMAccount>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.DomainUserName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.FullName)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(x => x.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(x => x.IsActive)
                      .IsRequired();

                entity.Property(x => x.CreatedDate)
                      .IsRequired();

                entity.HasIndex(x => x.DomainUserName)
                      .IsUnique();
            });

            modelBuilder.Entity<IDMRole>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.RoleName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.IsActive)
                      .IsRequired();

                entity.HasIndex(x => x.RoleName)
                      .IsUnique();
            });

            modelBuilder.Entity<IDMAccountRole>(entity =>
            {
                entity.HasKey(x => new { x.AccountId, x.RoleId });
                entity.HasOne(x => x.Account)
                      .WithMany()
                      .HasForeignKey(x => x.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Role)
                      .WithMany()
                      .HasForeignKey(x => x.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FormRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.EmployeeId)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.FormType)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(x => x.Status)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(x => x.CreatedDate)
                      .IsRequired();

                entity.Property(x => x.Comments)
                      .HasMaxLength(1000);
            });

            modelBuilder.Entity<VacationRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.HasOne(x => x.FormRequest)
                      .WithMany()
                      .HasForeignKey(x => x.FormRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PermissionRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.PermissionType)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(x => x.SwapDetail)
                      .HasMaxLength(1000);

                entity.HasOne(x => x.FormRequest)
                      .WithMany()
                      .HasForeignKey(x => x.FormRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TimeAdjustmentRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(x => x.MissingPunchType)
                      .HasConversion<string>()
                      .IsRequired();

                entity.HasOne(x => x.FormRequest)
                      .WithMany()
                      .HasForeignKey(x => x.FormRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DepartmentShiftChangeRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedOnAdd();

                entity.HasOne(x => x.FormRequest)
                      .WithMany()
                      .HasForeignKey(x => x.FormRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Employee>(entity =>
             {
                 entity.HasKey(x => x.Id);
                 entity.Property(x => x.Id)
                       .ValueGeneratedOnAdd();
                 entity.Property(x => x.EmployeeNumber)
                 .IsRequired()
                 .HasMaxLength(20);

                 entity.Property(x => x.DomainUserName)
                 .IsRequired()
                 .HasMaxLength(50);

                 entity.Property(x => x.FullName)
                 .IsRequired()
                 .HasMaxLength(200);

                 entity.Property(x => x.Email)
                 .HasMaxLength(200);

                 entity.Property(x => x.Department)
                 .HasMaxLength(100);

                 entity.Property(x => x.Shift)
                 .HasMaxLength(50);

                 entity.Property(x => x.Position)
                 .HasMaxLength(100);

                 // Supervisor name (nullable to not break existing data)
                 entity.Property(x => x.SupervisorName)
                       .HasMaxLength(200);

                 // Plant stored as string in DB (nullable)
                 entity.Property(x => x.Plant)
                       .HasConversion<string>()
                       .HasMaxLength(50);

                 entity.HasIndex(x => x.EmployeeNumber)
                 .IsUnique();

                 entity.HasIndex(x => x.DomainUserName)
                 .IsUnique();
             });

            base.OnModelCreating(modelBuilder);
        }
    }
}
