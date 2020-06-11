using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using efStart3.Models;

namespace efStart3.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public Instructor Instructor { get; set; }
    }
}