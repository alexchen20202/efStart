using Microsoft.EntityFrameworkCore;
using efStart3.Models;

namespace efStart3.DAL
{
    public class SchoolContext: DbContext
    {
        public SchoolContext()
        {
        }
        public SchoolContext(DbContextOptions<SchoolContext> options): base(options)
        {    
        }
        public DbSet<Student> Students{get;set;}
        public DbSet<Enrollment> Enrollments{get;set;}
        public DbSet<Course> Courses{get;set;}
        public DbSet<Department> Departments{get;set;}
        public DbSet<Instructor> Instructors{get;set;}
        public DbSet<OfficeAssignment> OfficeAssignments{get;set;}
        public DbSet<CourseAssignment> CourseAssignments{get;set;}
        // public DbSet<Person> People{get;set;}
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                string path = "Data Source=.\\School.db;";
                optionsBuilder.UseSqlite(path);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Department>()
                .Property(p => p.RowVersion).IsConcurrencyToken();
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            // modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });   
        }

    }
}