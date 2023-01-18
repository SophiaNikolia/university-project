using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace university_project.Models
{
    public partial class universityContext : DbContext
    {
        public universityContext()
        {
        }

        public universityContext(DbContextOptions<universityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseHasStudent> CourseHasStudents { get; set; } = null!;
        public virtual DbSet<Professor> Professors { get; set; } = null!;
        public virtual DbSet<Secretary> Secretaties { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build()
                    .GetSection("ConnectionStrings")["universityContextConnection"];

                optionsBuilder.UseSqlite(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.IdCourse);

                entity.ToTable("course");

                entity.Property(e => e.IdCourse)
                    .ValueGeneratedNever()
                    .HasColumnName("idCOURSE");

                entity.Property(e => e.ProfessorsAfm).HasColumnName("PROFESSORS_AFM");

                entity.HasOne(d => d.ProfessorsAfmNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.ProfessorsAfm)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CourseHasStudent>(entity =>
            {
                entity.HasKey(e => new { e.CourseIdCourse, e.StudentsRegistrationNumber });

                entity.ToTable("course_has_students");

                entity.Property(e => e.CourseIdCourse).HasColumnName("COURSE_idCOURSE");

                entity.Property(e => e.StudentsRegistrationNumber).HasColumnName("STUDENTS_RegistrationNumber");

                entity.HasOne(d => d.CourseIdCourseNavigation)
                    .WithMany(p => p.CourseHasStudents)
                    .HasForeignKey(d => d.CourseIdCourse)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.StudentsRegistrationNumberNavigation)
                    .WithMany(p => p.CourseHasStudents)
                    .HasForeignKey(d => d.StudentsRegistrationNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.Afm);

                entity.ToTable("professors");

                entity.Property(e => e.Afm)
                    .ValueGeneratedNever()
                    .HasColumnName("AFM");

                entity.Property(e => e.UsersUsername).HasColumnName("USERS_username");

                entity.HasOne(d => d.UsersUsernameNavigation)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.UsersUsername)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Secretary>(entity =>
            {
                entity.HasKey(e => e.Phonenumber);

                entity.ToTable("secretaties");

                entity.Property(e => e.Phonenumber).ValueGeneratedNever();

                entity.Property(e => e.UsersUsername).HasColumnName("USERS_username");

                entity.HasOne(d => d.UsersUsernameNavigation)
                    .WithMany(p => p.Secretaties)
                    .HasForeignKey(d => d.UsersUsername)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.RegistrationNumber);

                entity.ToTable("students");

                entity.Property(e => e.RegistrationNumber).ValueGeneratedNever();

                entity.Property(e => e.UsersUsername).HasColumnName("USERS_username");

                entity.HasOne(d => d.UsersUsernameNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.UsersUsername)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("users");

                entity.Property(e => e.Username).HasColumnName("username");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
