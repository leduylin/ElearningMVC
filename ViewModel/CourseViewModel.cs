using Microsoft.AspNetCore.Mvc;

using System.Drawing;

namespace ElearningMVC.ViewModel
{
    public class CourseViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Image Image { get; set; }
        public string Key { get; set; }
    }
}
