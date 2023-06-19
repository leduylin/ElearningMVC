using System.ComponentModel.DataAnnotations;

namespace ElearningMVC.Models
{
    public class TeacherJoinClass
    {
        [Key]
        public int Id { get; set; }
        public int? TeacherId { get; set;}
        public int? CourseId { get; set; }

        public Teacher? Teacher { get; set; }
        public Course? Course { get; set;}
    }
}
