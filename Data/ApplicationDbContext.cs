using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using ElearningMVC.Models;
using System.Reflection.Metadata;

namespace ElearningMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(e => e.TeacherJoinClass)
                .WithOne(e => e.Course)
                .HasForeignKey<TeacherJoinClass>(e => e.Id)
                .IsRequired();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<StudentJoinClass> StudentJoinClasses { get; set; }
        public DbSet<TeacherJoinClass> TeachersJoinClasses { get; set; }


    }
}