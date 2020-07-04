using efStart3.Models.Params;

namespace efStart3.Models.Params
{
    public class InstructorParam
    {
        public int? InstructID{get;set;}
        public int? CourseID{get;set;}
        public int? PageIndex{get;set;}
        public string SearchString{get;set;}
    }
}