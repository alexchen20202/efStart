namespace efStart3.Models.Params
{
    public class StudentParam
    {
        public int? StudentID{get;set;}
        public int? CourseID{get;set;}
        public string SortString{get;set;}
        public string SearchString{get;set;}
        public string SortLastName{get;set;}
        public string SortID{get;set;}
        public string SortEnrollmentDate{get;set;}
        public int PageIndex{get;set;}
    }    
}