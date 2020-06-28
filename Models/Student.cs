using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace efStart3.Models
{
    public partial class Student
    {
        [Display(Name="Student ID")]
        public int StudentId{get;set;}
        
        [Required]
        [StringLength(50)]
        [Display(Name="First Name")]
        public string FirstName{get;set;}
        
        [Required]
        [StringLength(50)]
        [Display(Name="Last Name")]
        public string LastName{get;set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name="Enrollment Date")]
        public DateTime EnrollmentDate{get;set;}
        
        [StringLength(100)]
        [Display(Name="Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        public virtual ICollection<Enrollment> Enrollments{get;set;}   
    }
}