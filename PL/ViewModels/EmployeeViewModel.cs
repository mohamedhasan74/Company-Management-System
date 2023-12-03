using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [MinLength(5, ErrorMessage ="Min Length Is 5 Char")]
        public string Name { get; set; }
        [Range(22,30,ErrorMessage ="Age Must Be Betwwen (22,30)")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-z]{4,10}-[a-zA-z]{4,10}-[a-zA-z]{4,10}$",ErrorMessage ="Address Must Be Like 123-Street-City-Countr")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime HireDate { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
