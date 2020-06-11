using System;
using System.ComponentModel.DataAnnotations;

namespace efStart3.Models.SchoolViewModels
{
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate{get;set;}
        public int StudentCount{get;set;}
    }
}