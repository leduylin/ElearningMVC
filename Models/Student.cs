using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ElearningMVC.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        ICollection<StudentJoinClass>? JoinClasses { get; set; } = new List<StudentJoinClass>();
    }
}
