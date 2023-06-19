using System.ComponentModel.DataAnnotations;

namespace ElearningMVC.Models
{
    public class StudentJoinClass
    {
        [Key]
        public int Id { get; set; }
        public int? StudentID { get; set; }
        public int? CourseID { get; set; }

        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
