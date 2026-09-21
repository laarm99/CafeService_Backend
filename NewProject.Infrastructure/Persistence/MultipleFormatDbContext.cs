using Microsoft.EntityFrameworkCore;
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
