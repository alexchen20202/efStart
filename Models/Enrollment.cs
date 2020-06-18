using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efStart3.Models
{
    public enum Grade{ A, B, C, D, E, F }

    public partial class Enrollment
    {
        public int EnrollmentID{get;set;}
        [Required]
        public int CourseID{get;set;}        
        [Required]
        public int StudentID{get;set;}
        [Display(Name="Grade")]
        public Grade? Grade{get;set;}


        public virtual Course Course{get;set;}
        public virtual Student Student{get;set;}
    }
}
