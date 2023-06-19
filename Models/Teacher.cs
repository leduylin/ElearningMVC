using System.ComponentModel.DataAnnotations;

namespace ElearningMVC.Models
{
    public class Teacher
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        ICollection<TeacherJoinClass>? Classes { get; set; } = new List<TeacherJoinClass>();
    }
}
