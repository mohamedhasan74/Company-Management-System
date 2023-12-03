using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [MinLength(2, ErrorMessage ="Min Length Is 2 Char")]
        public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public IEnumerable<Employee>? Employees { get; set; }
    }
}
