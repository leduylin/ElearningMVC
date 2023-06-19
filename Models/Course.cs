using System.ComponentModel.DataAnnotations;

namespace ElearningMVC.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? PhotoURL { get; set; }
        
        public int? TeacherJoinClassId { get; set; }
        public TeacherJoinClass? TeacherJoinClass { get; set;}
        ICollection<StudentJoinClass>? StudentList { get; set; } = new List<StudentJoinClass>();


    }
}
